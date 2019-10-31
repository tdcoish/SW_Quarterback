/*************************************************************************************
Is given a route to run. Runs it.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class RP_Receiver : MonoBehaviour
{
    public string                   mTag;
    private RP_Manager              rManager;

    private Rigidbody               cRigid;
    private OFF_RouteLog            cRouteLog;
    private RP_CatchLog             cCatchLog;
    private PRAC_Ath                cAth;               // super irritating and shitty.

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

    void Awake()
    {
        cAth = GetComponent<PRAC_Ath>();
        cRouteLog = GetComponent<OFF_RouteLog>();
        cCatchLog = GetComponent<RP_CatchLog>();
        cRigid = GetComponent<Rigidbody>();
        rManager = FindObjectOfType<RP_Manager>();
        mState = STATE.SPRE_SNAP;
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
        // cRigid.constraints = RigidbodyConstraints.FreezeAll;
        cRigid.velocity = Vector3.zero;
    }

    public void FENTER_PRE_SNAP()
    {
        cAth.mState = PRAC_Ath.PRAC_ATH_STATE.SPRE_SNAP;
        mState = STATE.SPRE_SNAP;
        cRouteLog.mState = OFF_RouteLog.STATE.S_BLIND;
    }
    public void FEnterRunJob()
    {
        cAth.mState = PRAC_Ath.PRAC_ATH_STATE.SDOING_JOB;
        mState = STATE.SDOING_JOB;
    }
    private void RUN_Job()
    {
        // cRigid.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionY;
        cRouteLog.FRunRoute();
    }

    // Again, nothing. Eventually some animations or something.
    private void RUN_PostPlay()
    {
        cRigid.velocity = Vector3.zero;
        // cRigid.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void FSetUpRoute(DATA_ORoute rt)
    {
        cRouteLog.mRouteSpots = new List<Vector3>();
        for(int i=0; i<rt.mSpots.Count; i++)
        {
            Vector3 rtSpot = UT_VecConversion.ConvertVec2(rt.mSpots[i]);
            rtSpot += transform.position;
            Debug.Log(rtSpot);
            cRouteLog.mRouteSpots.Add(rtSpot);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>())
        {
            rManager.OnBallCaught(mTag);
        }
    }

}
