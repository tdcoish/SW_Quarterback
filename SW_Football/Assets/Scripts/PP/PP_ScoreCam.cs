/*************************************************************************************
All it does is just smoothly rotate around the arena.
*************************************************************************************/
using UnityEngine;

public class PP_ScoreCam : MonoBehaviour
{

    public Vector3              mCenteredPoint;
    public float                mRad = 10f;
    public float                mPeriod = 10f;      // time for 1 revolution
    public float                mHermiteSpd = 0.1f;

    void Start()
    {
        transform.position = FindObjectOfType<PC_Controller>().transform.position;
    }

    void Update()
    {
        Vector3 vCurPoint = mCenteredPoint;
        vCurPoint.z += mRad * Mathf.Cos(Time.time/mPeriod);
        vCurPoint.x += mRad * Mathf.Sin(Time.time/mPeriod);
        vCurPoint.y = 5f;
		
        // transform.position = transform.position.Hermite(vCurPoint, mHermiteSpd);
        transform.position = vCurPoint;
        transform.LookAt(mCenteredPoint);
        
    }
}
