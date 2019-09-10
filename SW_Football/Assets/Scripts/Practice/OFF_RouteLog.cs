/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class OFF_RouteLog : MonoBehaviour
{
    public List<Vector3>            mRouteSpots;

    public enum STATE{
        S_FOLLOWING,
        S_DONE
    }
    public STATE                    mState;

    private void Start()
    {
        mState = STATE.S_FOLLOWING;
    }

    // Call this when the play is actually running.
    public void FRunRoute()
    {
        if(mState == STATE.S_DONE)
        {
            Debug.Log("Done");
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            // look at the player.
            Vector3 vPos = FindObjectOfType<PC_Controller>().transform.position;
            Vector3 vDir = transform.position - vPos; vDir.y = 0f;
            vDir = Vector3.Normalize(vDir);
            transform.rotation = Quaternion.Euler(vDir);
        }

        if(mState == STATE.S_FOLLOWING)
        {
            if(mRouteSpots.Count <= 0){
                mState = STATE.S_DONE;
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
}
