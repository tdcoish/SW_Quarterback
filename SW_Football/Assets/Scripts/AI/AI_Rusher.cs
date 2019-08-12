/*************************************************************************************
For now he just rushes directly at the quarterback. If "caught" in the sphere of influence
of a blocker, then he just tries to bullrush for now.

We need to get a reference to the quarterback.

More and more I think we need an overarching game manager that everything has a reference to.
Rusher needs a "strength of rush" or "power" or something so they can test that against a blockers
"anchor" or whatever.
*************************************************************************************/
using UnityEngine;

public class AI_Rusher : MonoBehaviour
{

    private Rigidbody           cRigid;

    private AI_Athlete          cAthlete;
    private AI_TakesShove       cTakesShove;
    private AI_Acc            cBurst;

    //private Transform           refQuarterback;
    private AI_Blocker              refBlocker;
    private PC_Controller           refPlayer;

    public bool                 mActive = false;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();

        cAthlete = GetComponent<AI_Athlete>();
        cTakesShove = GetComponent<AI_TakesShove>();
        cBurst = GetComponent<AI_Acc>();
    }

    // so we're trying to move towards the quarterback, let's just do that for now. Totally ignore blockers.
    void Update()
    {
        if(mActive)
        {
            // Vector3 vel = refPlayer.transform.position - transform.position;
            // vel = Vector3.Normalize(vel);
            // vel *= cAthlete.mSpd;
            // cRigid.velocity = vel;

            cBurst.FCalcAcc(refPlayer.transform.position - transform.position);

            // hack in him getting pushed by the blocker.
            // if(Vector3.Distance(transform.position, refBlocker.transform.position) < 2f)
            // {
            //     Vector3 shoveDir = transform.position - refBlocker.transform.position;
            //     shoveDir.y = 0f;
            //     shoveDir = Vector3.Normalize(shoveDir) * refBlocker.GetComponent<AI_Athlete>().mBull;
            //     AI_Shove shove = new AI_Shove(shoveDir, refBlocker.GetComponent<AI_Athlete>().mTag);
            //     cTakesShove.FTakeShove(shove);
            // }

            cTakesShove.FRecalculateShoves();
            // Debug.Log("Shove mags : " + cTakesShove.mAllForces);
            if(cTakesShove.mAllForces.magnitude > 0f){
                // Debug.Log("All shove forces: " + cTakesShove.mAllForces);
                cRigid.velocity += cTakesShove.mAllForces;
            }
        }
    }

    public void OnSnap()
    {
        cAthlete.mSpd = 10f;
        cAthlete.mBull = 600f;      // x lbsm/s. Quit a big boy
        cAthlete.mWgt = 300f;       // big boy
        cAthlete.mAnc = 200f;       // internal power
        cAthlete.mAcc = 5f;

        refPlayer = FindObjectOfType<PC_Controller>();
        refBlocker = FindObjectOfType<AI_Blocker>();
        mActive = true;
    }
}
