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
    private AI_Acc              cBurst;
    private AI_GivesShove       cGivShv;

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
        cGivShv = GetComponent<AI_GivesShove>();
    }

    // so we're trying to move towards the quarterback, let's just do that for now. Totally ignore blockers.
    void Update()
    {
        if(mActive)
        {
            cBurst.FCalcAcc(refPlayer.transform.position - transform.position);

            // Need some component that pushes characters that we want to push.
            if(Vector3.Distance(transform.position, refBlocker.transform.position) < 2f)
            {
                cGivShv.FGiveShove(refBlocker.GetComponent<AI_Athlete>());
            }
        }
    }

    public void OnSnap()
    {
        cAthlete.mSpd = 2f;
        cAthlete.mBull = 50f;      // x lbsm/s. Quit a big boy
        cAthlete.mWgt = 300f;       // big boy
        cAthlete.mAnc = 0f;        // internal power
        cAthlete.mAcc = 1f;

        refPlayer = FindObjectOfType<PC_Controller>();
        refBlocker = FindObjectOfType<AI_Blocker>();
        mActive = true;
    }
}
