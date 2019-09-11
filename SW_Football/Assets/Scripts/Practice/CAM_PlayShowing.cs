/*************************************************************************************
This is the camera that we use to show the play on the field pre-snap.

Disable the players camera, and enable our camera.

Eventually do a nice LERP, but not right now.
*************************************************************************************/
using UnityEngine;

public class CAM_PlayShowing : MonoBehaviour
{

    private Camera                  cCam;

    private Vector3                 mPosToSnapTo;

    private enum CAM_STATE{
        SINACTIVE,
        S_DEACTIVATING,
        SACTIVE
    }
    private CAM_STATE               mState = CAM_STATE.SINACTIVE;


    void Start()
    {
        cCam = GetComponent<Camera>();
    }

    // All it does is hermite up and down, because snapping there is a little harsh.
    void Update()
    {
        switch(mState)
        {
            case CAM_STATE.SACTIVE: RUN_Activated(); break;
            case CAM_STATE.S_DEACTIVATING: RUN_Deactivating(); break;
            case CAM_STATE.SINACTIVE: RUN_Inactive(); break;
        }
                
    }

    private void RUN_Activated()
    {
        transform.position = transform.position.Hermite(mPosToSnapTo, 0.1f);

        // Now we want to look roughly at the snap spot, for now.
        PLY_SnapSpot snap = FindObjectOfType<PLY_SnapSpot>();
        if(snap != null){
            transform.LookAt(snap.transform.position, Vector3.up);
        }
    }

    // On deactivate, when we get close is when we actually change everything else.
    private void RUN_Deactivating()
    {
        transform.position = transform.position.Hermite(mPosToSnapTo, 0.2f);

        PLY_SnapSpot snap = FindObjectOfType<PLY_SnapSpot>();
        if(snap != null){
            transform.LookAt(snap.transform.position, Vector3.up);
        }

        if(Vector3.Distance(transform.position, mPosToSnapTo) < 0.1f)
        {
            TrueDeactivate();
        }
    }

    private void RUN_Inactive()
    {

    }

    public void FActivate()
    {
        PC_Controller pc = FindObjectOfType<PC_Controller>();
        mPosToSnapTo = pc.transform.position;
        mPosToSnapTo.y += 20f;
        mPosToSnapTo.z -= 15f;
        transform.position = pc.GetComponentInChildren<PC_Camera>().transform.position;

        pc.GetComponentInChildren<Camera>().enabled = false;
        pc.GetComponentInChildren<AudioListener>().enabled = false;
        cCam.enabled = true;
        GetComponent<AudioListener>().enabled = true;

        pc.mState = PC_Controller.PC_STATE.SINACTIVE;
        mState = CAM_STATE.SACTIVE;
    }

    public void FDeactivate()
    {
        PC_Controller pc = FindObjectOfType<PC_Controller>();
        mPosToSnapTo = pc.GetComponentInChildren<PC_Camera>().transform.position;
        pc.mState = PC_Controller.PC_STATE.SPRE_SNAP;
        mState = CAM_STATE.S_DEACTIVATING;           // maybe not the best name
    }

    // This should be called ~0.5 seconds after FDeactivate, every time.
    private void TrueDeactivate()
    {
        cCam.enabled = false;
        GetComponent<AudioListener>().enabled = false;
        PC_Controller pc = FindObjectOfType<PC_Controller>();
        pc.GetComponentInChildren<Camera>().enabled = true;
        pc.GetComponentInChildren<AudioListener>().enabled = true;

        mState = CAM_STATE.SINACTIVE;
    }
}
