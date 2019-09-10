/*************************************************************************************
All the receivers, their starting positions, and their routes.
Also their hoops.
*************************************************************************************/
using UnityEngine;

public class RP_ReceiverList : MonoBehaviour
{
    public Vector3                  mPCSpot;
    public Vector3[]                mRecStartPos;
    public string[]                 mRecRoutes;
    public string[]                 mRecTags;
    public Vector3[]                mRingPos;
    public string[]                 mRingTags;

    public void FStoreSet()
    {
        mPCSpot = FindObjectOfType<PC_Controller>().transform.position;

        RP_Receiver[] recs = FindObjectsOfType<RP_Receiver>();
        RP_Hoop[] hoops = FindObjectsOfType<RP_Hoop>();
        if(recs.Length != hoops.Length)
        {
            Debug.Log("Hoop receiver mismatch, not even numbers");
            Debug.Log("Hoops: " + hoops.Length);
            Debug.Log("Recs: " + recs.Length);
            return;
        }

        mRecStartPos = new Vector3[recs.Length];
        mRecRoutes = new string[recs.Length];
        mRecTags = new string[recs.Length];
        for(int i=0; i<recs.Length; i++)
        {
            mRecStartPos[i] = recs[i].transform.position;
            mRecRoutes[i] = recs[i].mRoute;
            mRecTags[i] = recs[i].mTag;
        }

        mRingPos = new Vector3[hoops.Length];
        mRingTags = new string[hoops.Length];
        for(int i=0; i<hoops.Length; i++)
        {
            mRingTags[i] = hoops[i].mWRTag;
            mRingPos[i] = hoops[i].transform.position;
        }
    }

    public Vector3 FGetRecSpot(string wrTag)
    {
        for(int i=0; i<mRecStartPos.Length; i++)
        {
            if(mRecTags[i] == wrTag)
            {
                return mRecStartPos[i];
            }
        }

        Debug.Log("No spot with that tag found");
        return Vector3.zero;
    }

    public Vector3 FGetRingSpot(string ringTag)
    {
        for(int i=0; i<mRingPos.Length; i++)
        {
            if(mRingTags[i] == ringTag)
            {
                return mRingPos[i];
            }
        }

        Debug.Log("No ring spot with that tag found");
        return Vector3.zero;
    }
}
