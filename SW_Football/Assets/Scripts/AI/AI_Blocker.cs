/*************************************************************************************
Given to all players that need to block? Maybe just all players period, but we disable this
unless they need to block. That makes more sense because almost everyone needs to block
at some point, interceptions, receptions, etcetera.

In pass protection he's trying to both get between the rusher and the quarterback, as well
as trying his best push the defender away from the quarterback.

Calculating zone of influence. Basically, when do we affect other players. Then we can do things
like apply a slow or something.


Calculate the "power" of all movements in all directions. Then calcuate the new direction of
the player. So for example, if we have no "shoves" against us, 100% of our movement is intentional.
If we have a shove that our stats lower to 50, then 50% of our movement is in the direction of the shove,
and 50% in the direction we want, so halfway between the two.
*************************************************************************************/
using UnityEngine;

public class AI_Blocker : MonoBehaviour
{
    private Rigidbody           cRigid;

    private AI_Athlete          cAthlete;
    private AI_TakesShove       cTakeShove;
    private AI_Acc              cAcc;
    private AI_GivesShove       cGivShv;

    // for now I'll just give him the defender to block
    private AI_Rusher           refRusher;
    private PC_Controller       refPlayer;

    private bool                mActive = false;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();

        cAthlete = GetComponent<AI_Athlete>();
        cTakeShove = GetComponent<AI_TakesShove>();
        cGivShv = GetComponent<AI_GivesShove>();
        cAcc = GetComponent<AI_Acc>();
    }

    void Update()
    {
        // 1) get midway between the rusher and the passer.
        if(mActive)
        {
            Vector3 midSpot = refPlayer.transform.position;
            Vector3 dis = refRusher.transform.position - refPlayer.transform.position;
            dis/=2f;
            midSpot += dis;

            // get our burst to accelerate us in that direction.
            cAcc.FCalcAcc(midSpot - transform.position);
 
            // for now, just calculate if our rusher is within our sphere of influence.
            if(Vector3.Distance(transform.position, refRusher.transform.position) < 2f){

                cGivShv.FGiveShove(refRusher.GetComponent<AI_Athlete>());
            }

        }


    }

    public void OnSnap()
    {
        cAthlete.mSpd = 0.5f;
        cAthlete.mBull = 50f;
        cAthlete.mWgt = 300f;
        cAthlete.mAnc = 0f; 
        cAthlete.mBks = 80f;  
        cAthlete.mAcc = 0.5f;  

        mActive = true;

        refRusher = FindObjectOfType<AI_Rusher>();
        refPlayer = FindObjectOfType<PC_Controller>();
    }
}
