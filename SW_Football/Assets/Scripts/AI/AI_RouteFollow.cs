/*********************************************************
AI has a path, it follows the nodes of the path, until it hits one,
then it goes to the next node.
****************************************************** */

using UnityEngine;
using System.Collections.Generic;

public class AI_RouteFollow : MonoBehaviour
{
    private Rigidbody           mRigid;

    private float               mSpd = 2f;

    // for now setting in the editor, eventually it will be a hash or string that is used to load in the route.
    public List<AI_RouteNode>       mPath;

    void Start()
    {
        mRigid = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        if(mPath.Count == 0){
            mRigid.velocity = Vector3.zero;
            return;
        }

        // set it's rotation to the next node.
        transform.rotation = Quaternion.LookRotation((mPath[0].transform.position - transform.position), Vector3.up);

        mRigid.velocity = transform.forward * mSpd;

        // if we're somewhat close to our node, then now move to the next one.
        if(Vector3.Distance(transform.position, mPath[0].transform.position) < 3f){
            mPath.RemoveAt(0);
        }
    }
}
