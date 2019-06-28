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
    private float                   mThrowChrg;
    private float                   mThrowChrgTm = 1f;          // in seconds
    private bool                    mChargingThrow = false;

    public float                    mSpd = 5f;
    public float                    mThrowSpd = 100f;

    private Rigidbody               mRigid;
    private PC_Camera               mCam;

    // if false, then we're doing vehicle-style controls
    private bool                    mFPSVision = true;

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
        if(Input.GetMouseButton(0)){
            mChargingThrow = true;
            mThrowChrg += Time.deltaTime;
            if(mThrowChrg > mThrowChrgTm){
                Debug.Log("Limiting Charge");
                mThrowChrg = mThrowChrgTm;
            }
        }
        if(mChargingThrow){
            if(Input.GetMouseButtonUp(0)){
                PROJ_Football clone = Instantiate(PF_Football, mThrowPoint.transform.position, transform.rotation);
                clone.GetComponent<Rigidbody>().velocity = mCam.transform.forward * mThrowSpd * mThrowChrg;
                mThrowChrg = 0f;
                mChargingThrow = false;
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
}
