/*************************************************************************************
This stores an individual set for the Pitch And Catch minigame. We have three of these 
for each difficulty level.
*************************************************************************************/
using UnityEngine;

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

[CreateAssetMenu(fileName="RP_Set", menuName="DT/RP_Set")]
public class SO_RP_Set : ScriptableObject
{
    public string                   mDifficulty;
    public Vector3                  mPCSpot;
    public DATA_RP_Receiver[]       mReceiverData;
    public DATA_RP_Ring[]           mRingData;
}
