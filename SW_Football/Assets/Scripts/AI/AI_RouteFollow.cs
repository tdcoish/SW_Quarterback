/*********************************************************
AI has a path, it follows the nodes of the path, until it hits one,
then it goes to the next node.
****************************************************** */

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AI_Route))]
public class AI_RouteFollow : MonoBehaviour
{
    private Rigidbody           mRigid;

    public float                mSpd = 2f;

    public AI_Route             mRoute;
    private Vector3             mCurGoal;

    [SerializeField]
    private GameObject          mSpotGoal;

    void Start()
    {
        mRigid = GetComponent<Rigidbody>();  
        mRoute = GetComponentInChildren<AI_Route>();  

        if(mRoute.mPath.Count != 0){
            mCurGoal = mRoute.mPath[0] + transform.position;
            mCurGoal.y = 0f;
            Instantiate(mSpotGoal, mCurGoal, transform.rotation);
            Debug.Log("Goal: " + mCurGoal);
        }
    }

    void Update()
    {
        if(mRoute.mPath.Count == 0){
            mRigid.velocity = Vector3.zero;
            return;
        }

        // set it's rotation to the next node.
        transform.rotation = Quaternion.LookRotation((mCurGoal - transform.position), Vector3.up);

        mRigid.velocity = transform.forward * mSpd;

        // if we're somewhat close to our node, then now move to the next one.
        if(Vector3.Distance(transform.position, mCurGoal) < 1.5f){
            mRoute.mPath.RemoveAt(0);
            if(mRoute.mPath.Count == 0){
                return;
            }
            mCurGoal = mRoute.mPath[0] + transform.position;
            Instantiate(mSpotGoal, mCurGoal, transform.rotation);
            Debug.Log("Goal: " + mCurGoal);

        }
    }
}
