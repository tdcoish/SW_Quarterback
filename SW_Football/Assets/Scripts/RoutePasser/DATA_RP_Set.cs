/*************************************************************************************
This stores an individual set for the Pitch And Catch minigame. We have three of these 
for each difficulty level.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DATA_RP_Receiver
{
    public string               mTag;
    public string               mRoute;
    public Vector3              mStartPos;
}

[System.Serializable]
public class DATA_RP_Ring
{
    public string               mTag;
    public Vector3              mScale;
    public Vector3              mDir;
    public Vector3              mStartPos;
}

[System.Serializable]
public class DATA_RP_Set
{
    public string                   mName;
    public float                    mTimeToThrow;
    public Vector3                  mPCSpot;
    public List<DATA_RP_Receiver>   mReceiverData;
    public List<DATA_RP_Ring>       mRingData;
}
