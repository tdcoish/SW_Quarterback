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

    private RP_CatchLog                         cCatchLog;

    private void Start()
    {
        cCatchLog = GetComponent<RP_CatchLog>();

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
        GetComponent<PRAC_AI_Acc>().FCalcAcc(dis);

        if(Vector3.Distance(mRouteSpots[0], transform.position) < 2f){
            mRouteSpots.RemoveAt(0);
        }

    }

    private void ENTER_CanReactToThrow()
    {
        mState = STATE.S_CAN_REACT_TO_THROW;

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
        GetComponent<PRAC_AI_Acc>().FCalcAcc(dis);

        if(Vector3.Distance(mRouteSpots[0], transform.position) < 2f){
            mRouteSpots.RemoveAt(0);
        }

        // Somewhere here, if we find the ball is being thrown to us, we need to enter a state where we just
        // try to get that ball.
        if(CheckIfBallThrown()){
            ENTER_TryCatchBall();
        }
    }

    private void ENTER_TryCatchBall()
    {
        mState = STATE.S_CATCHING_BALL_TRY;
    }
    void RUN_TryCatchBall()
    {
        Vector3 vSpotToGetTo = cCatchLog.FCalcInterceptSpot();
        Vector3 dis = vSpotToGetTo - transform.position;
        dis.y = 0f;
        dis = Vector3.Normalize(dis);
        GetComponent<PRAC_AI_Acc>().FCalcAcc(dis);          // god, it's so ugly that we have these side effects.
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
        if(CheckIfBallThrown()){
            ENTER_TryCatchBall();
        }
    }
    
    // Called in both get open and look for ball while running route
    // Try for no side effects.
    // But it's also if they specifically can see the ball, which they might not due to the angle or whatever.
    private bool CheckIfBallThrown()
    {
        PROJ_Football f = FindObjectOfType<PROJ_Football>();
        if(f!=null){
            return true;
        }

        return false;
    }

}
