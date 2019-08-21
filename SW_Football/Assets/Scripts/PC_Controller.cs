using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************************
Two camera modes, one handles like FPS, other handles like Half Life 2 vehicle controls, where 
movement is independent of where the camera is looking.
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

    public float                    mSpd = 10f;
    public float                    mAccPerSec = 10f;

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
    public SO_Float                 GB_Innaccuracy;
    public SO_Float                 GB_ThrowInnacuracy;

    public Vector3                  mThrowStartAngle;
    public SO_Float                 GB_ThrowLookInaccuracy;

    public bool                     mActive = true;

    // Start is called before the first frame update
    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        cCam = GetComponentInChildren<PC_Camera>();

        mThrowChrg.Val = 0f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GB_Innaccuracy.Val = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!mActive) return;

        SetRotation();
        HandleThrowModifiers();
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
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

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

                // now we update the vector3 representing the angle we're throwing at.
                mThrowAngle.Val = cCam.transform.forward;

                // technically this is actually the accuracy right now.
                float fAccDot = Vector3.Dot(mThrowAngle.Val, mThrowStartAngle);
                // let's say that inaccuracy translates to 1 degree per degree moved.
                GB_ThrowLookInaccuracy.Val = (1f-fAccDot) * 100f;
            }

            if(mChargingThrow){

                // add more innacuracy to our throw for every little frame.
                GB_ThrowInnacuracy.Val += Time.deltaTime * GB_Innaccuracy.Val;

                if(Input.GetMouseButtonUp(0)){
                    PROJ_Football clone = Instantiate(PF_Football, mThrowPoint.transform.position, transform.rotation);

                    // now we add in the innacuracy.
                    // in degrees for now. Technically this makes a box, maybe work on that.
                    float fCombinedInaccuracy = GB_ThrowInnacuracy.Val + GB_ThrowLookInaccuracy.Val;
                    float fXAcc = Random.Range(-fCombinedInaccuracy, fCombinedInaccuracy);
                    float fYAcc = Random.Range(-fCombinedInaccuracy, fCombinedInaccuracy);
                    fXAcc /= 90f;
                    fYAcc /= 90f;
                    Vector3 vThrowDir = cCam.transform.forward;
                    vThrowDir.x += fXAcc;
                    vThrowDir.y += fYAcc;
                    vThrowDir = Vector3.Normalize(vThrowDir);

                    clone.GetComponent<Rigidbody>().velocity = vThrowDir * mThrowChrg.Val * PlayerData._ThrowSpd;
                    mThrowChrg.Val = 0f;
                    mChargingThrow = false;

                    GE_QB_ReleaseBall.Raise(null);

                    GB_ThrowInnacuracy.Val = 0f;
                    GB_ThrowLookInaccuracy.Val = 0f;
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
            fSideAcc -= mAccPerSec * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.D)){
            fSideAcc += mAccPerSec * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.W)){
            fForAcc += mAccPerSec * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.S)){
            fForAcc -= mAccPerSec * Time.fixedDeltaTime;
        }

        if(Mathf.Abs(fForAcc) + Mathf.Abs(fSideAcc) > mAccPerSec)
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
            vVel -= vVel * Time.fixedDeltaTime * mSpd;
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
        if(Vector3.Magnitude(vVel) > mSpd)
        {
            vVel *= mSpd / Vector3.Magnitude(vVel);
        }

        cRigid.velocity = vVel;

    }

    // If we move, decrease accuracy, if we aim around, decrease accuracy.
    private void HandleThrowModifiers()
    {
        float fInnac = cRigid.velocity.magnitude/mSpd;
        fInnac -= 0.1f;
        fInnac *= 10f;
        GB_Innaccuracy.Val = fInnac;
        if(GB_Innaccuracy.Val < 0f){
            GB_Innaccuracy.Val = 0f;
        }
    }

    // They clutch the ball and decide not to throw.
    public void ThrowStopped()
    {
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
