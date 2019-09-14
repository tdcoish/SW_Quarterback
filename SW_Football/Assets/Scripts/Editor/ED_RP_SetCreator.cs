/*************************************************************************************
Takes whatevers in the scene, and stores all that data in a set.
*************************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ED_RP_SetCreator : MonoBehaviour
{
    
    [MenuItem("PitAndCat/StoreCurrent")]
    private static void StoreCurrent()
    {
        DATA_RP_Set s = new DATA_RP_Set();

        s.mTimeToThrow = 10f;

        RP_Receiver[] recs = FindObjectsOfType<RP_Receiver>();
        s.mReceiverData = new List<DATA_RP_Receiver>();
        Debug.Log("Num Receivers in scene: " + recs.Length);
        for(int i=0; i<recs.Length; i++)
        {
            DATA_RP_Receiver r = new DATA_RP_Receiver();
            r.mTag = recs[i].mTag;
            r.mRoute = recs[i].mRoute;
            r.mStartPos = recs[i].transform.position;
            s.mReceiverData.Add(r);
        }

        RP_Hoop[] hoops = FindObjectsOfType<RP_Hoop>();
        s.mRingData = new List<DATA_RP_Ring>();
        Debug.Log("Num rings in the scene: " + hoops.Length);
        for(int i=0; i<hoops.Length; i++)
        {
            DATA_RP_Ring r = new DATA_RP_Ring();
            r.mDir = hoops[i].transform.rotation.eulerAngles;
            r.mScale = hoops[i].transform.localScale;
            r.mTag = hoops[i].mWRTag;
            r.mStartPos = hoops[i].transform.position;
            s.mRingData.Add(r);
        }

        s.mPCSpot = FindObjectOfType<PC_Controller>().transform.position;

        RP_Manager man = FindObjectOfType<RP_Manager>();
        s.mName = man.mActiveSet.mName;
        man.mActiveSet = s;

        return;
    }
}
