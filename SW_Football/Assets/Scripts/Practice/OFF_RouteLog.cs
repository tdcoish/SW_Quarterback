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
        S_LOOKING_FOR_BALL,     // they will now react to the ball being in the air.
        S_GET_OPEN                  // they have to do some "get open" behaviour.
    }
    public STATE                    mState;

    private void Start()
    {
        mState = STATE.S_BLIND;
    }

    // Call this when the play is actually running.
    public void FRunRoute()
    {
        switch(mState)
        {
            case STATE.S_BLIND: RUN_BlindlyFollowing(); break;
            case STATE.S_LOOKING_FOR_BALL: RUN_LookingForBallWhileRunningRoute(); break;
            case STATE.S_GET_OPEN: RUN_GetOpen(); break;
        }

    }

    private void RUN_BlindlyFollowing()
    {
        // If they've made the last move, and are decently close to the destination.
        if(mRouteSpots.Count <= 1){
            if(Vector3.Distance(transform.position, mRouteSpots[mRouteSpots.Count-1]) < 10f){
                ENTER_LookingForBall();
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

    private void ENTER_LookingForBall()
    {
        mState = STATE.S_LOOKING_FOR_BALL;

    }

    // Basically, they're not done running the route yet, but they are looking for the ball.
    private void RUN_LookingForBallWhileRunningRoute()
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

    }
    
}
