/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class OFF_RouteLog : MonoBehaviour
{
    public List<Vector3>            mRouteSpots;

    private void Start()
    {

        // mRouteSpots = new List<Vector3>();
    }

    // Call this when the play is actually running.
    public void FRunRoute()
    {
        if(mRouteSpots.Count <= 0){
            // GetComponent<PRAC_AI_Acc>().FCalcAcc(Vector3.zero);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
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
}
