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
    public float                    mThrowChrg;
    public SO_Float                 mCurThrowPwr;
    public SO_Vec3                  mThrowAngle;
    public SO_Float                 mCurThrowMaxChrg;
    private bool                    mChargingThrow = false;

    [SerializeField]
    private DT_Player               PlayerData;

    public float                    mSpd = 15f;

    private Rigidbody               mRigid;
    private PC_Camera               mCam;
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

    // Start is called before the first frame update
    void Start()
    {
        mRigid = GetComponent<Rigidbody>();
        mCam = GetComponentInChildren<PC_Camera>();

        mThrowChrg = 0f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
        HandleMovement();
        HandleThrowing();

        // so everyone knows our position.
        RefPlayerPos.Val = transform;
    }

    private void SetRotation()
    {
        // want to get rid of y component of the cameras rotation.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // rotate camera view around y axis.
        transform.RotateAround(transform.position, Vector3.up, mouseX);
        Vector3 xAx = Vector3.Cross(transform.forward, Vector3.up);
        mCam.transform.RotateAround(transform.position, xAx, mouseY);
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
                    mCurThrowMaxChrg.Val = PlayerData._ThrowSpd;
                }
                mChargingThrow = true;

                mThrowChrg += Time.deltaTime;
                if(mThrowChrg > PlayerData._ThrowChargeTime){
                    mThrowChrg = PlayerData._ThrowChargeTime;
                }

                // now we update the vector3 representing the angle we're throwing at.
                mThrowAngle.Val = mCam.transform.forward;
                mCurThrowPwr.Val = mThrowChrg * mCurThrowMaxChrg.Val;
            }

            if(mChargingThrow){
                // if they hold down the shift key, then we make the charge go down.
                if(Input.GetKey(KeyCode.LeftShift)){
                    mCurThrowMaxChrg.Val -= Time.deltaTime * 10f;        // you take off 5 force per second
                    if(mThrowChrg < 0f) mThrowChrg = 0f;
                }

                if(Input.GetMouseButtonUp(0)){
                    PROJ_Football clone = Instantiate(PF_Football, mThrowPoint.transform.position, transform.rotation);
                    clone.GetComponent<Rigidbody>().velocity = mCam.transform.forward * mCurThrowMaxChrg.Val * (mThrowChrg/PlayerData._ThrowChargeTime);
                    mThrowChrg = 0f;
                    mChargingThrow = false;

                    GE_QB_ReleaseBall.Raise(null);
                }
            }

        }
    }

    private void HandleMovement()
    { 
        float sideVel = 0f;
        float fwdVel = 0f;

        if(Input.GetKey(KeyCode.A)){
            sideVel -= mSpd;
        }
        if(Input.GetKey(KeyCode.D)){
            sideVel += mSpd;
        }
        if(Input.GetKey(KeyCode.W)){
            fwdVel += mSpd;
        }
        if(Input.GetKey(KeyCode.S)){
            fwdVel -= mSpd;
        }

        Vector3 fwd = transform.forward * fwdVel;
        Vector3 right = transform.right * sideVel;

        mRigid.velocity = Vector3.Normalize(fwd+right) * mSpd;
    }

    // They clutch the ball and decide not to throw.
    public void ThrowStopped()
    {
        mCanThrow = false;
        mChargingThrow = false;
        mThrowChrg = 0f;
        Invoke("CanThrowAgain", 1.0f);
    }

    private void CanThrowAgain()
    {
        mCanThrow = true;
    }
}
