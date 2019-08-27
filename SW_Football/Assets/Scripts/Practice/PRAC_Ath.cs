/*************************************************************************************
Just stores the damn info for the play.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class PRAC_Ath : MonoBehaviour
{
    private Rigidbody               cRigid;


    public DT_PlayerRole            mJob;

    public bool                     mActive = false;

    public List<Vector3>            mRouteSpots;

    void Awake()
    {
        cRigid = GetComponent<Rigidbody>();
        mRouteSpots = new List<Vector3>();
    }

    // I guess we'll give him a list of his route spots?

    void Update()
    {        
        if(!mActive)
        {
            return;
        }

        if(mJob.mRole == "Route")
        {
            Debug.Log("This receiver should follow his routes");

            // Make him move towards his next point.
            if(mRouteSpots.Count <= 0)
            {
                Debug.Log("Route finished");
                return;
            }

            Vector3 dis = mRouteSpots[0] - transform.position;
            dis.y = 0f;
            dis = Vector3.Normalize(dis);
            cRigid.velocity = dis * 5f;

            // now if he gets real close, then we chop that node off.
            if(Vector3.Distance(mRouteSpots[0], transform.position) < 2f)
            {
                mRouteSpots.RemoveAt(0);
            }
        }
    }
}
