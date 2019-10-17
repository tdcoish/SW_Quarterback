/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class ATH_Forces : MonoBehaviour
{
    public float                mTopSpd = 2f;
    public float                mWgt = 300f;
    public float                mInternalPwr = 600f;
    public float                mArmPwr = 600f;

    // for simplification, can only have a single net push force.
    public Vector3              mNetForces;
    public float                mLastForeignPushTime;

    public void FTakePush(Vector3 vPush, float fTime)
    {
        mLastForeignPushTime = fTime;
        mNetForces = vPush;
    }

    // internal power depends on how fast we are going.
    public Vector3 FFuncCalcInternalPush(Vector3 vDir, Vector3 vCurVel, float fInPwr, float fMaxSpd)
    {
        Vector3 vPush = new Vector3();
        vDir = Vector3.Normalize(vDir);
        // if we're going backwards, then we just get the full push.
        if(Vector3.Dot(vDir, vCurVel) < 0f){
            vPush = fInPwr * vDir;
            return vPush; 
        }

        // now we need to find out how fast we're going in the axis we want to accelerate in.
        Vector3 vVelInDir = vDir * Vector3.Dot(vDir, vCurVel);
        float fAxMag = vVelInDir.magnitude;
        float fAxMagInv = (fMaxSpd - fAxMag) / fMaxSpd;

        vPush = fAxMagInv * fInPwr * vDir;
        return vPush;
    }
}
