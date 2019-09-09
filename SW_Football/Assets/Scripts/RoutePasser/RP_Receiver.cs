/*************************************************************************************
Is given a route to run. Runs it.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class RP_Receiver : MonoBehaviour
{
    public string                   mRoute;

    private OFF_RouteLog            cRouteLog;

    public enum STATE{
        SPRE_SNAP,
        SDOING_JOB,
        SPOST_PLAY
    }
    public STATE                    mState;

    void Start()
    {
        cRouteLog = GetComponent<OFF_RouteLog>();
        mState = STATE.SPRE_SNAP;

        SetUpRoute();
    }

    void Update()
    {
        switch(mState)
        {
            case STATE.SPRE_SNAP: RUN_PreSnap(); break;
            case STATE.SDOING_JOB: RUN_Job(); break;
            case STATE.SPOST_PLAY: RUN_PostPlay(); break;
        }
    }

    private void RUN_PreSnap()
    {

    }

    private void RUN_Job()
    {
        cRouteLog.FRunRoute();
    }

    // Again, nothing. Eventually some animations or something.
    private void RUN_PostPlay()
    {

    }

    private void SetUpRoute()
    {
        DATA_Route rt = IO_RouteList.FLOAD_ROUTE_BY_NAME(mRoute);
        cRouteLog.mRouteSpots = new List<Vector3>();
        for(int i=0; i<rt.mSpots.Length; i++)
        {
            Vector3 rtSpot = UT_VecConversion.ConvertVec2(rt.mSpots[i]);
            rtSpot += transform.position;
            cRouteLog.mRouteSpots.Add(rtSpot);
        }
    }
}
