/*************************************************************************************
I think they only really need a role for now.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class PRAC_Off_Ply : PRAC_Ath
{
    public string                               mTag;
    public string                               mRole;
    // Remove this from here
    public List<Vector2>                        mSpots;

    private OFF_RouteLog                        cRouteLog;
    private OFF_BlockLog                        cBlockLog;
    private OFF_RunLog                          cRunLog;

    // Planning on using this to calculate where we need to go to catch the ball.
    private TRG_Catch                           cCatchRadius;

    void Start()
    {
        cRouteLog = GetComponent<OFF_RouteLog>();
        cBlockLog = GetComponent<OFF_BlockLog>();
        cCatchRadius = GetComponentInChildren<TRG_Catch>();
        cRunLog = GetComponent<OFF_RunLog>();

        cCatchRadius.gameObject.SetActive(false);

        SetupRouteSpots();
    }

    private void SetupRouteSpots(){
        cRouteLog.mRouteSpots = new List<Vector3>();
        for(int i=0; i<mSpots.Count; i++){
            Vector3 v = new Vector3();
            v = transform.position;
            v.x += mSpots[i].x * 2f;
            v.z += mSpots[i].y * 2f;
            v.y = 1f;
            cRouteLog.mRouteSpots.Add(v);
        }
    }

    protected override void RUN_Job()
    {
        switch(mRole)
        {
            case "ROUTE": cRouteLog.FRunRoute(); break;
            case "BLOCK": cBlockLog.FRunBlocking(); break;
        }
    }

    // Get to the endzone.
    protected override void RUN_RunWithBall()
    {
        cRunLog.FRunWithBall();
    }

    public override void FCaughtBall()
    {
        mState = PRAC_ATH_STATE.SRUN_WITH_BALL;
        cAud.mCatch.Play();
    }
}
