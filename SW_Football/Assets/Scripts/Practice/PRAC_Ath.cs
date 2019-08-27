/*************************************************************************************
Just stores the damn info for the play.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;


public class PRAC_Ath : MonoBehaviour
{
    public enum PRAC_ATH_STATE
    {
        SPRE_SNAP,
        SDOING_JOB,
        SPOST_PLAY
    }
    public PRAC_ATH_STATE           mState;

    private Rigidbody               cRigid;


    public DT_PlayerRole            mJob;

    public List<Vector3>            mRouteSpots;

    void Awake()
    {
        cRigid = GetComponent<Rigidbody>();
        mRouteSpots = new List<Vector3>();

        mState = PRAC_ATH_STATE.SPRE_SNAP;
    }

    // I guess we'll give him a list of his route spots?

    void Update()
    {        
        switch(mState)
        {
            case PRAC_ATH_STATE.SPRE_SNAP: RUN_PreSnap(); break;
            case PRAC_ATH_STATE.SDOING_JOB: RUN_Job(); break;
            case PRAC_ATH_STATE.SPOST_PLAY: RUN_PostPlay(); break;
        }


    }

    private void RUN_PreSnap()
    {
        // Actually we just don't really do anything here, for now.

    }

    private void RUN_Job()
    {
        if(mJob.mRole == "Route")
        {

            // Make him move towards his next point.
            if(mRouteSpots.Count <= 0)
            {
                return;
            }

            Vector3 dis = mRouteSpots[0] - transform.position;
            dis.y = 0f;
            dis = Vector3.Normalize(dis);
            cRigid.velocity = dis * 5f;

            // now if he gets real close, then we chop that node off.
            if(Vector3.Distance(mRouteSpots[0], transform.position) < 2f)
            {
                mRouteSpots.RemoveAt(0);
            }
        }
    }

    // Again, nothing. Eventually some animations or something.
    private void RUN_PostPlay()
    {

    }
}
