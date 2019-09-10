/*************************************************************************************
Is given a route to run. Runs it.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class RP_Receiver : MonoBehaviour
{
    public string                   mRoute;
    public string                   mTag;
    private RP_Manager              rManager;

    private Rigidbody               cRigid;
    private OFF_RouteLog            cRouteLog;
    private RP_CatchLog             cCatchLog;

    public enum STATE{
        SPRE_SNAP,
        SDOING_JOB,
        SPOST_PLAY
    }
    public STATE                    mState;
    // public enum JOB_STATE{
    //     S_ASSIGNMENT,
    //     S_BREAK_ON_BALL
    // }

    void Start()
    {
        cRouteLog = GetComponent<OFF_RouteLog>();
        cCatchLog = GetComponent<RP_CatchLog>();
        cRigid = GetComponent<Rigidbody>();
        rManager = FindObjectOfType<RP_Manager>();
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
        cRigid.constraints = RigidbodyConstraints.FreezeAll;
        cRigid.velocity = Vector3.zero;
    }

    private void RUN_Job()
    {
        cRigid.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionY;

        RP_Manager rpMan = FindObjectOfType<RP_Manager>();
        if(rpMan.mBallThrown)
        {
            // ---------------------------- Get the spot to move to
            Vector3 vSpot = cCatchLog.FCalcInterceptSpot();
            Vector3 vDir = vSpot - transform.position; vDir.y = 0f;
            vDir = Vector3.Normalize(vDir);
            // ---------------------------- Find out if we're going to overrun it. If so, accelerate in the opposite direction.
            // But that gets complicated if we're at a weird angle, so only do this if we're basically going straight there.
            if(Vector3.Dot(vDir, Vector3.Normalize(cRigid.velocity)) > 0.99f){
                Vector3 vProjectedSpot = vDir * (cRigid.velocity.magnitude/(vSpot - transform.position).magnitude);
                if(Vector3.Dot(vSpot - vProjectedSpot, vDir) < 0f){
                    Debug.Log("Overrunning");
                    GetComponent<PRAC_AI_Acc>().FCalcAcc(Vector3.Normalize(-vDir));
                }
            }
            GetComponent<PRAC_AI_Acc>().FCalcAcc(Vector3.Normalize(vDir));
        }else{
            cRouteLog.FRunRoute();
        }
    }

    // Again, nothing. Eventually some animations or something.
    private void RUN_PostPlay()
    {
        cRigid.velocity = Vector3.zero;
        cRigid.constraints = RigidbodyConstraints.FreezeAll;
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

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>())
        {
            rManager.OnBallCaught();
        }
    }

}
