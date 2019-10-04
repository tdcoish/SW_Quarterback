/*************************************************************************************
All it does is rotate around slowly, pointing at the center of the field.
*************************************************************************************/
using UnityEngine;

public class CAM_Sim : MonoBehaviour
{
    public Vector3              mCenteredPoint;
    public float                mRad = 100f;
    public float                mPeriod = 100f;      // time for 1 revolution
    public float                mHeight = 50f;
    public float                mHermiteSpd = 0.1f;

    void Update()
    {
        Vector3 vCurPoint = mCenteredPoint;
        vCurPoint.z += mRad * Mathf.Cos(Time.time/mPeriod);
        vCurPoint.x += mRad * Mathf.Sin(Time.time/mPeriod);
        vCurPoint.y = mHeight;
		
        transform.position = transform.position.Hermite(vCurPoint, mHermiteSpd);
        // transform.position = vCurPoint;
        transform.LookAt(mCenteredPoint);
        
    }
}
