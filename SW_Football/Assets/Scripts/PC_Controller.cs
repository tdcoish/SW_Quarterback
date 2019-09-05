﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************************
Two camera modes, one handles like FPS, other handles like Half Life 2 vehicle controls, where 
movement is independent of where the camera is looking.

New update for inaccuracy. We have instantaneous bump, say of +5, then we have over time effects,
on top of that. As an example, if moving gives you +5, and you're already at +3 inaccuracy, you jump
to +5, then you start stacking the over time effects.
**************************************************************************************** */

public class PC_Controller : MonoBehaviour
{

    public enum PC_STATE
    {
        SINACTIVE,
        SPRE_SNAP,
        SACTIVE
    }
    public PC_STATE                 mState;

    [SerializeField]
    private PROJ_Football           PF_Football;

    [SerializeField]
    private GameObject              mThrowPoint;
    public SO_Float                 mThrowChrg;         // from 0-1. Factor in power later.
    public SO_Vec3                  mThrowAngle;
    private bool                    mChargingThrow = false;

    private Rigidbody               cRigid;
    private PC_Camera               cCam;

    public GE_Event                 GE_QB_StartThrow;
    public GE_Event                 GE_QB_ReleaseBall;
    public GE_Event                 GE_QB_StopThrow;

    // if false, then we're doing vehicle-style controls
    private bool                    mFPSVision = true;

    [SerializeField]
    private SO_Transform            RefPlayerPos;

    private bool                    mCanThrow = true;
    public Vector3                  mThrowStartAngle;

    public SO_Float                 GB_LookInaccuracy;
    public SO_Float                 GB_MoveInaccuracy;
    public SO_Float                 GB_TotalInaccuracy;

    // Start is called before the first frame update
    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        cCam = GetComponentInChildren<PC_Camera>();

        mState = PC_STATE.SINACTIVE;
        mThrowChrg.Val = 0f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SetInaccuraciesToZero();
    }

    // Update is called once per frame
    void Update()
    {
        switch(mState)
        {
            case PC_STATE.SINACTIVE: RUN_Inactive(); break;
            case PC_STATE.SPRE_SNAP: RUN_PreSnap(); break;
            case PC_STATE.SACTIVE: RUN_Active(); break;
        }
    }

    private void RUN_Inactive()
    {

    }

    private void RUN_PreSnap()
    {
        SetRotation();
    }

    private void RUN_Active()
    {
        SetRotation();
        HandleThrowing();

        // so everyone knows our position.
        RefPlayerPos.Val = transform;
    }

    void FixedUpdate()
    {
        if(mState != PC_STATE.SACTIVE) {
            cRigid.velocity = Vector3.zero;
            return;
        }

        HandleMovement();
    }

    private void SetRotation()
    {
        // want to get rid of y component of the cameras rotation.
        float mouseX = Input.GetAxis("Mouse X") * IO_Settings.mSet.lLookSensitity;
        float mouseY = Input.GetAxis("Mouse Y") * IO_Settings.mSet.lLookSensitity;
        // look sensitivity between 0.5 - 1.5

        // rotate camera view around y axis.
        transform.RotateAround(transform.position, Vector3.up, mouseX);
        Vector3 xAx = Vector3.Cross(transform.forward, Vector3.up);
        cCam.transform.RotateAround(transform.position, xAx, mouseY);
    }


    private void HandleThrowing()
    {
        if(mCanThrow){

            // RMB stops throw
            if(Input.GetMouseButton(1)){
                GE_QB_StopThrow.Raise(null);
                return;
            }

            if(Input.GetMouseButton(0)){
                if(!mChargingThrow){
                    GE_QB_StartThrow.Raise(null);
                    mThrowStartAngle = cCam.transform.forward;
                }
                mChargingThrow = true;

                // Alright, this is what needs to be changed. Throw power should not charge linearly, it should charge logarithmically.
                float fChrgPct = mThrowChrg.Val;
                float fChargeAmt = Time.deltaTime * (1/IO_Settings.mSet.lPlayerData.mThrowChargeTime);
                fChargeAmt -= fChargeAmt*(fChrgPct);           // gives us right side of bell curve.
                // but now we also have to factor in that we charge faster when closer to 0.
                // fChargeAmt *= (1-mThrowChrg.Val) * 2f;              // so when at 0, x as fast. When at 1, 0 charge speed.
                mThrowChrg.Val += fChargeAmt;
                if(mThrowChrg.Val > 1f){
                    mThrowChrg.Val = 1f;
                }
            }

            if(mChargingThrow){

                // Now handle the look inaccuracy
                mThrowAngle.Val = cCam.transform.forward;

                HandleThrowModifiers();

                if(Input.GetMouseButtonUp(0)){
                    PROJ_Football clone = Instantiate(PF_Football, mThrowPoint.transform.position, transform.rotation);

                    // now we add in the innacuracy.
                    // x inaccuracy feels better than y inaccuracy, which can look really stupid.
                    float fXAcc = Random.Range(-GB_TotalInaccuracy.Val, GB_TotalInaccuracy.Val) / 100f;
                    float fYAcc = Random.Range(-GB_TotalInaccuracy.Val, GB_TotalInaccuracy.Val) / 100f;
                    fYAcc /= IO_Settings.mSet.lInaccuracyBias;
                    Vector3 vThrowDir = cCam.transform.forward;
                    vThrowDir.x += fXAcc; vThrowDir.y += fYAcc;
                    vThrowDir = Vector3.Normalize(vThrowDir);

                    clone.GetComponent<Rigidbody>().velocity = vThrowDir * mThrowChrg.Val * IO_Settings.mSet.lPlayerData.mThrowSpd;
                    mThrowChrg.Val = 0f;
                    mChargingThrow = false;

                    GE_QB_ReleaseBall.Raise(null);

                    SetInaccuraciesToZero();
                }
            }

        }
    }

    /****************************************************************************************************
    For the sake of the throw accuracy stuff, we need a momentum system. So it takes a little time to get 
    moving, and it takes a little time to stop moving. While moving, the accuracy of the throw is proportional
    to the percentage of max speed, multiplied by the natural innacuracy, of perhaps 10 degrees or so.
    ************************************************************************************************** */
    private void HandleMovement()
    { 
        float fSideAcc = 0f;
        float fForAcc = 0f;

        if(Input.GetKey(KeyCode.A)){
            fSideAcc -= IO_Settings.mSet.lPlayerData.mAccRate * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.D)){
            fSideAcc += IO_Settings.mSet.lPlayerData.mAccRate * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.W)){
            fForAcc += IO_Settings.mSet.lPlayerData.mAccRate * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.S)){
            fForAcc -= IO_Settings.mSet.lPlayerData.mAccRate * Time.fixedDeltaTime;
        }

        if(Mathf.Abs(fForAcc) + Mathf.Abs(fSideAcc) > IO_Settings.mSet.lPlayerData.mAccRate)
        {
            fForAcc *= 0.707f;
            fSideAcc *= 0.707f;
        }


        Vector3 vVel = cRigid.velocity;
        // basically, if we're not accelerating, then make our velocity lowered.
        // other wise do so normally.
        if(Mathf.Abs(fForAcc) + Mathf.Abs(fSideAcc) < 0.1f)
        {
            // say we'll stop over 1 second, or so.
            vVel -= vVel * Time.fixedDeltaTime * IO_Settings.mSet.lPlayerData.mMoveSpd;
            // and if we go too far, then just set vel to zero.
            if(Vector3.Dot(vVel, cRigid.velocity) <= 0f)
            {
                cRigid.velocity = Vector3.zero;
            }
        }
        else
        {
            vVel += fForAcc * transform.forward;
            vVel += fSideAcc * transform.right;
        }
        if(Vector3.Magnitude(vVel) > IO_Settings.mSet.lPlayerData.mMoveSpd)
        {
            vVel *= IO_Settings.mSet.lPlayerData.mMoveSpd / Vector3.Magnitude(vVel);
        }

        cRigid.velocity = vVel;

    }

    // This is assuming that we are in the process of throwing already.
    // If they're below the minimal threshold for the base penalty, then we rapidly accelerate them there.
    private void HandleThrowModifiers()
    {
        if(cRigid.velocity.magnitude > 0.1f){
            // Handle movement inaccuracy here.
            float fSpdPct = cRigid.velocity.magnitude/IO_Settings.mSet.lPlayerData.mMoveSpd;
            float fInstInac = fSpdPct * IO_Settings.mSet.lMovementPenalty;

            if(GB_MoveInaccuracy.Val < fInstInac)
            {
                GB_MoveInaccuracy.Val += fInstInac * Time.deltaTime * 5f;
            }

            // we get instantaneous penalties, along with penalties over time.
            GB_MoveInaccuracy.Val += fInstInac * Time.deltaTime;            

        }

        // Now handle the looking inaccuracy
        // When the dot == 0, then we get full inaccuracy, even worse if greater than 90*
        float fLookDot = Vector3.Dot(mThrowAngle.Val, mThrowStartAngle);
        float fLookInstInac = (1-fLookDot) *  IO_Settings.mSet.lLookPenalty;
        if(GB_LookInaccuracy.Val < fLookInstInac)
        {
            GB_LookInaccuracy.Val = fLookInstInac;
        }

        GB_TotalInaccuracy.Val = GB_LookInaccuracy.Val + GB_MoveInaccuracy.Val;
    }

    private void SetInaccuraciesToZero()
    {
        GB_LookInaccuracy.Val = 0f;
        GB_MoveInaccuracy.Val = 0f;
        GB_TotalInaccuracy.Val = 0f;
    }

    // They clutch the ball and decide not to throw.
    public void ThrowStopped()
    {
        SetInaccuraciesToZero();
        mCanThrow = false;
        mChargingThrow = false;
        mThrowChrg.Val = 0f;
        Invoke("CanThrowAgain", 1.0f);
    }

    private void CanThrowAgain()
    {
        mCanThrow = true;
    }
}
