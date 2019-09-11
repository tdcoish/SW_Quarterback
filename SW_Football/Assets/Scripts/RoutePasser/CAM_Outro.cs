/*************************************************************************************
Guess we'll just spin around the field looking at the center.
*************************************************************************************/
using UnityEngine;

public class CAM_Outro : MonoBehaviour
{

    public enum STATE{
        S_ACTIVE,
        S_INACTIVE
    }
    public STATE                    mState = STATE.S_INACTIVE;

    public Vector3              mCenteredPoint;
    public float                mRad = 10f;
    public float                mPeriod = 10f;              // time for 1 revolution
    public float                mHermiteSpd = 0.1f;

    private void Update()
    {
        switch(mState)
        {
            case STATE.S_ACTIVE: RUN_Active(); break;
            case STATE.S_INACTIVE: RUN_InActive(); break;
        }
    }

    private void RUN_Active()
    {
        Vector3 vCurPoint = mCenteredPoint;
        vCurPoint.z += mRad * Mathf.Cos(Time.time/mPeriod);
        vCurPoint.x += mRad * Mathf.Sin(Time.time/mPeriod);
        vCurPoint.y = 5f;
		
        transform.position = transform.position.Hermite(vCurPoint, mHermiteSpd);
        //transform.position = vCurPoint;
        transform.LookAt(mCenteredPoint);
    }
    private void RUN_InActive()
    {

    }

    public void FActivate()
    {
        GetComponent<Camera>().enabled = true;
        GetComponent<AudioListener>().enabled = true;

        mState = STATE.S_ACTIVE;

        mCenteredPoint = FindObjectOfType<PC_Controller>().transform.position;
    }
}