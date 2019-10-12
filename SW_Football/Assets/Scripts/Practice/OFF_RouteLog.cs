/*************************************************************************************
Now receivers only look for the ball when they only have one node left to hit, AND they 
are within 20 meters of it. That way you can have a two node go route, but they don't start 
looking immediately.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class OFF_RouteLog : MonoBehaviour
{
    public List<Vector3>            mRouteSpots;

    public enum STATE{
        S_BLIND,                // they just run the route.
        S_CAN_REACT_TO_THROW,     // they will now react to the ball being in the air.
        S_CATCHING_BALL_TRY,        // They have reacted to the throw, and are now trying to catch the ball.
        S_GET_OPEN                  // they have to do some "get open" behaviour.
    }
    public STATE                    mState;

    private Rigidbody                           cRigid;
    private PRAC_Ath                            cAth;
    private RP_CatchLog                         cCatchLog;
    private TRG_Catch                           cCatchRadius;
    private PRAC_AI_Acc                         cAcc;

    private void Awake()
    {
        cRigid = GetComponent<Rigidbody>();
        cCatchLog = GetComponent<RP_CatchLog>();
        cCatchRadius = GetComponentInChildren<TRG_Catch>();
        cAcc = GetComponent<PRAC_AI_Acc>();
        cAth = GetComponent<PRAC_Ath>();

        mState = STATE.S_BLIND;
    }

    // Call this when the play is actually running.
    public void FRunRoute()
    {
        switch(mState)
        {
            case STATE.S_BLIND: RUN_BlindlyFollowing(); break;
            case STATE.S_CAN_REACT_TO_THROW: RUN_CanReactToThrowWhileRunningRoute(); break;
            case STATE.S_GET_OPEN: RUN_GetOpen(); break;
            case STATE.S_CATCHING_BALL_TRY: RUN_TryCatchBall(); break;
        }

    }

    private void RUN_BlindlyFollowing()
    {
        // If they've made the last move, and are decently close to the destination.
        if(mRouteSpots.Count <= 1){
            if(Vector3.Distance(transform.position, mRouteSpots[mRouteSpots.Count-1]) < 10f){
                ENTER_CanReactToThrow();
                return;
            }
        }

        Vector3 dis = mRouteSpots[0] - transform.position;
        dis.y = 0f;
        dis = Vector3.Normalize(dis);
        Vector3 vAcc = cAcc.FCalcAccFunc(dis, cAcc.mSpd);
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > cAcc.mSpd){
            cRigid.velocity *= cAcc.mSpd/cRigid.velocity.magnitude;
        }
        transform.forward = cRigid.velocity.normalized;

        if(Vector3.Distance(mRouteSpots[0], transform.position) < 2f){
            mRouteSpots.RemoveAt(0);
        }

    }

    private void ENTER_CanReactToThrow()
    {
        mState = STATE.S_CAN_REACT_TO_THROW;
        cCatchRadius.gameObject.SetActive(true);
    }

    // Basically, they're not done running the route yet, but they are looking for the ball.
    private void RUN_CanReactToThrowWhileRunningRoute()
    {
        if(mRouteSpots.Count <= 0)
        {
            ENTER_GetOpen();
            return;
        }
        Vector3 dis = mRouteSpots[0] - transform.position;
        dis.y = 0f;
        dis = Vector3.Normalize(dis);
        Vector3 vAcc = cAcc.FCalcAccFunc(dis, cAcc.mSpd);
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > cAcc.mSpd){
            cRigid.velocity *= cAcc.mSpd/cRigid.velocity.magnitude;
        }
        transform.forward = cRigid.velocity.normalized;

        if(Vector3.Distance(mRouteSpots[0], transform.position) < 2f){
            mRouteSpots.RemoveAt(0);
        }

        // Somewhere here, if we find the ball is being thrown to us, we need to enter a state where we just
        // try to get that ball.
        if(cAth.FCheckIfBallThrown()){
            ENTER_TryCatchBall();
        }
    }

    private void ENTER_TryCatchBall()
    {
        mState = STATE.S_CATCHING_BALL_TRY;
    }

    /***********************************************************************
    The issue here is that we don't want the player to be running too fast
    to the spot. Currently, they will run to the spot and only then start slowing
    down. We need them to anticipate this.

    So we need to figure out WHEN they need to be in that spot, and move at the 
    appropriate velocity. So they should accelerate relative to what that velocity
    should be.
    ********************************************************************* */
    void RUN_TryCatchBall()
    {
        Vector3 vSpotToGetTo = cCatchLog.FCalcInterceptSpot(0.5f);
        Vector3 dis = vSpotToGetTo - transform.position;
        dis.y = 0f;
        float airTime = cCatchLog.FCalcInterceptTime(0.5f);
        // since I know how far I need to go, I also know the exact velocity I should be using.
        Vector3 vVel = dis / airTime;

        // Now subtract our current velocity, and we have the direction to accelerate in.
        Vector3 vAccDir = vVel - cRigid.velocity;
        vAccDir = Vector3.Normalize(vAccDir);

        Vector3 vAcc = cAcc.FCalcAccFunc(vAccDir, cAcc.mSpd);
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > cAcc.mSpd){
            cRigid.velocity *= cAcc.mSpd/cRigid.velocity.magnitude;
        }
        transform.forward = cRigid.velocity.normalized;

    }

    private void ENTER_GetOpen()
    {
        mState = STATE.S_GET_OPEN;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        // look at the player.
        Vector3 vPos = FindObjectOfType<PC_Controller>().transform.position;
        Vector3 vDir = transform.position - vPos; vDir.y = 0f;
        vDir = Vector3.Normalize(vDir);
        transform.rotation = Quaternion.Euler(vDir);
    }
    private void RUN_GetOpen()
    {
        if(cAth.FCheckIfBallThrown()){
            ENTER_TryCatchBall();
        }
    }
    
    // Called in both get open and look for ball while running route
    // Try for no side effects.
    // But it's also if they specifically can see the ball, which they might not due to the angle or whatever.

}
