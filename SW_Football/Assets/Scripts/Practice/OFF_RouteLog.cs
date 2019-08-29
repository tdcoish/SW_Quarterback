/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class OFF_RouteLog : MonoBehaviour
{
    private Rigidbody               cRigid;

    public List<Vector3>            mRouteSpots;

    private void Start()
    {
        cRigid = GetComponent<Rigidbody>();

        // mRouteSpots = new List<Vector3>();
    }

    // Call this when the play is actually running.
    public void FRunRoute()
    {
        if(mRouteSpots.Count <= 0){
            cRigid.velocity = Vector3.zero;
            Debug.Log("No routes");
            return;
        }

        Vector3 dis = mRouteSpots[0] - transform.position;
        dis.y = 0f;
        dis = Vector3.Normalize(dis);
        cRigid.velocity = dis * 5f;

        if(Vector3.Distance(mRouteSpots[0], transform.position) < 2f){
            mRouteSpots.RemoveAt(0);
        }
    }
}
