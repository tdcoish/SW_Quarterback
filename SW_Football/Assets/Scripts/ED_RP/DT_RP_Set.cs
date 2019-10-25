/*************************************************************************************
Individual set. Needs to save the starting positions of players and hoops. The height of 
each hoop. Which one corresponds to which receiver. Etc.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public struct DT_RP_Hoop
{
    // height included here.
    public Vector3                  mStart;
    public float                    mSize;
    public string                   mTag;           // must match the player 
}
public struct DT_RP_Rec
{
    public Vector3                  mStart;         // just make y = 1f;
    public string                   mTag;
    public DATA_ORoute              mRoute;
}

public class DT_RP_Set
{
    public List<DT_RP_Hoop>                         mHoops;
    public List<DT_RP_Rec>                          mRecs;
    public List<DATA_ORoute>                        mRoutes;
}
