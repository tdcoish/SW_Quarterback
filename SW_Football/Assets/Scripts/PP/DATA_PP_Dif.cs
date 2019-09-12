/*************************************************************************************
The data for a pocket passer difficulty.
*************************************************************************************/
using UnityEngine;

[System.Serializable]
public class DATA_PP_Dif
{
    public string                   mName;

    public Vector3                  mPocketScale;
    public float                    mTimeBetweenTargetChanges;
    
    public int                      mNumTurrets;
    public Vector3[]                mTurretSpots;
    public float                    mTurretFireRate;

    public int                      mNumTargets;
    public Vector3[]                mTargetSpots;
    public Vector3[]                mTargetScales;

    public int                      mBronzeTrophy;
    public int                      mSilverTrophy;
    public int                      mGoldTrophy;

}
