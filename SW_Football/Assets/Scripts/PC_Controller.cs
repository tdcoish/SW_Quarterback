using System.Collections;
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
    [SerializeField]
    private PROJ_Football           PF_Football;

    [SerializeField]
    private GameObject              mThrowPoint;
    public SO_Float                 mThrowChrg;         // from 0-1. Factor in power later.
    public SO_Vec3                  mThrowAngle;
    private bool                    mChargingThrow = false;

    [SerializeField]
    private DT_Player               PlayerData;
    public SO_Float                 GB_LookSensitivity;
    public SO_Float                 GB_SET_MoveInaccRate;
    public SO_Float                 GB_SET_LookInaccRate;
    public SO_Float                 GB_SET_InaccuracyBias;      // bias towards x over y

    private Rigidbody               cRigid;
    private PC_Camera               cCam;
    [SerializeField]
    private PC_UI                   mUI;

    public GE_Event                 GE_QB_StartThrow;
    public GE_Event                 GE_QB_ReleaseBall;
    public GE_Event                 GE_QB_StopThrow;

    // if false, then we're doing vehicle-style controls
    private bool                    mFPSVision = true;

    [SerializeField]
    private SO_Transform            RefPlayerPos;

    private bool                    mCanThrow = true;
    public Vector3                  mThrowStartAngle;

    // these are built up over the time of the throw.
    public SO_Float                 GB_MoveInaccuracy;
    public SO_Float                 GB_LookInaccuracy;
    public SO_Float                 GB_TotalInaccuracy;

    public bool                     mActive = true;

    // Start is called before the first frame update
    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        cCam = GetComponentInChildren<PC_Camera>();

        mThrowChrg.Val = 0f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SetInaccuraciesToZero();
    }

    // Update is called once per frame
    void Update()
    {
        if(!mActive) return;

        SetRotation();
        HandleThrowing();

        // so everyone knows our position.
        RefPlayerPos.Val = transform;
    }

    void FixedUpdate()
    {
        if(!mActive) return;

        HandleMovement();
    }

    private void SetRotation()
    {
        // want to get rid of y component of the cameras rotation.
        float mouseX = Input.GetAxis("Mouse X") * GB_LookSensitivity.Val;
        float mouseY = Input.GetAxis("Mouse Y") * GB_LookSensitivity.Val;
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
                float fChargeAmt = Time.deltaTime * (1/PlayerData._ThrowChargeTime);
                // but now we also have to factor in that we charge faster when closer to 0.
                fChargeAmt *= (1-mThrowChrg.Val) * 2f;              // so when at 0, twice as fast. When at 1, 0 charge speed.
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
                    fYAcc /= GB_SET_InaccuracyBias.Val;
                    Vector3 vThrowDir = cCam.transform.forward;
                    vThrowDir.x += fXAcc; vThrowDir.y += fYAcc;
                    vThrowDir = Vector3.Normalize(vThrowDir);

                    clone.GetComponent<Rigidbody>().velocity = vThrowDir * mThrowChrg.Val * PlayerData._ThrowSpd;
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
            fSideAcc -= PlayerData._AccRate * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.D)){
            fSideAcc += PlayerData._AccRate * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.W)){
            fForAcc += PlayerData._AccRate * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.S)){
            fForAcc -= PlayerData._AccRate * Time.fixedDeltaTime;
        }

        if(Mathf.Abs(fForAcc) + Mathf.Abs(fSideAcc) > PlayerData._AccRate)
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
            vVel -= vVel * Time.fixedDeltaTime * PlayerData._MoveSpd;
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
        if(Vector3.Magnitude(vVel) > PlayerData._MoveSpd)
        {
            vVel *= PlayerData._MoveSpd / Vector3.Magnitude(vVel);
        }

        cRigid.velocity = vVel;

    }

    // This is assuming that we are in the process of throwing already.
    // If they're below the minimal threshold for the base penalty, then we rapidly accelerate them there.
    private void HandleThrowModifiers()
    {
        if(cRigid.velocity.magnitude > 0.1f){
            // Handle movement inaccuracy here.
            float fSpdPct = cRigid.velocity.magnitude/PlayerData._MoveSpd;
            float fInstInac = fSpdPct * GB_SET_MoveInaccRate.Val;

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
        float fLookInstInac = (1-fLookDot) *  GB_SET_LookInaccRate.Val;
        if(GB_LookInaccuracy.Val < fLookInstInac)
        {
            GB_LookInaccuracy.Val += fLookInstInac * Time.deltaTime * 5f;
        }
        GB_LookInaccuracy.Val += fLookInstInac * Time.deltaTime;

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
