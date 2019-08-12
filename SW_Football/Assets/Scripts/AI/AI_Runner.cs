/*************************************************************************************
This script takes over when any designated runner has the ball. For now all we do is 
run directly to the endzone.
*************************************************************************************/
using UnityEngine;

public class AI_Runner : MonoBehaviour
{

    public GE_Event             GE_Tackled;

    private Rigidbody           cRigid;

    public bool                 mActivated = false;

    // will get filled in if they are carrying the ball. For some reason transform.parent is not working.
    public PROJ_Football        rFootball;

    private AI_RouteFollow      cRouteFollow;
    private AI_Athlete          cAthlete;
    private AI_Acc              cAcc;

    public GE_Event             GE_Touchdown;

    void Start()
    {
        cAthlete = GetComponent<AI_Athlete>();
        cRouteFollow = GetComponent<AI_RouteFollow>();
        cAcc = GetComponent<AI_Acc>();

        cRigid = GetComponent<Rigidbody>();
    }

    // just make us run to the endzone for now.
    void Update()
    {
        // wow this was actually fairly easy.
        // when we get tackled, set activated to false.
        if(mActivated){
            Vector3 runDir = new Vector3(0f,0f,1f);
            cAcc.FCalcAcc(runDir);

            if(rFootball != null){
                Vector3 pos = transform.position;
                pos.y += 2f;
                rFootball.transform.position = Vector3.MoveTowards(rFootball.transform.position, pos, 2f);
            }
        }
    }

    // something feels vaguely off with this.
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.GetComponent<AI_Tackler>()){


            // now we want to release the ball from our grasp.
            PROJ_Football fBall = GetComponentInChildren<PROJ_Football>();
            if(fBall){
                GE_Tackled.Raise(null);
                mActivated = false;
                fBall.transform.parent = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(mActivated){
            if(other.GetComponent<GM_Endzone>()){
                Debug.Log("Here");
                GE_Touchdown.Raise(null);
                mActivated = false;
            }
        }
    }
}
