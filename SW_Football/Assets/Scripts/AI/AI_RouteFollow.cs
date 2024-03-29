/*********************************************************
AI has a path, it follows the nodes of the path, until it hits one,
then it goes to the next node.

Bug. The first spot in the node should always be the starting 
position of the player.

Could fix this in a few different ways.
1) In RT_PlayWriter save plays not as specific field positions,
but as instructions from a generic position.
ie. 5 yards up, 2 yards out, etc.
2) Go through the path list at start and fix it so that all the nodes are
relative to the starting node.
3) 
****************************************************** */

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AI_Route))]
public class AI_RouteFollow : MonoBehaviour
{
    private Rigidbody           cRigid;

    private AI_Athlete          cAthlete;
    private AI_Acc              cAcc;

    public AI_Route             mRoute;
    private Vector3             mCurGoal;

    [SerializeField]
    private GameObject          mPathMarker;

    // kind of a proto-state machine here.
    public bool                 mActive = false;

    void Start()
    {
        cAthlete = GetComponent<AI_Athlete>();
        cAcc = GetComponent<AI_Acc>();

        cRigid = GetComponent<Rigidbody>();  
        mRoute = GetComponentInChildren<AI_Route>();  

        if(mRoute.mPath.Count != 0){
            mCurGoal = mRoute.mPath[0] + transform.position;
            mCurGoal.y = 0f;
            Instantiate(mPathMarker, mCurGoal, transform.rotation);
        }
        // ground entity.
        UT_Grounded grounder = GetComponentInChildren<UT_Grounded>();
        Vector3 pos = transform.position;
        pos.y -= grounder.DisFromGround();
        transform.position = pos;
    }

    void Update()
    {
        if(!mActive) return;

        if(mRoute.mPath.Count == 0){
            cRigid.velocity = Vector3.zero;
            return;
        }

        cAcc.FCalcAcc(mCurGoal - transform.position);

        // if we're somewhat close to our node, then now move to the next one.
        if(Vector3.Distance(transform.position, mCurGoal) < 1.5f){
            mRoute.mPath.RemoveAt(0);
            if(mRoute.mPath.Count == 0){
                return;
            }
            mCurGoal = mRoute.mPath[0] + transform.position;
            Instantiate(mPathMarker, mCurGoal, transform.rotation);
        }
    }

    public void OnSnap()
    {
        mActive = true;
    }

    public void ResetPath(){

    }
}
