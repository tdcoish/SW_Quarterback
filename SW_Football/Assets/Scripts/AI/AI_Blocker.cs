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
    private AI_Burst            cBurst;

    // for now I'll just give him the defender to block
    private AI_Rusher           refRusher;
    private PC_Controller       refPlayer;

    private bool                mActive = false;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();

        cAthlete = GetComponent<AI_Athlete>();
        cTakeShove = GetComponent<AI_TakesShove>();
        cBurst = GetComponent<AI_Burst>();
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
            cBurst.FCalcBurst(midSpot - transform.position);
 
            // for now, just calculate if our rusher is within our sphere of influence.
            if(Vector3.Distance(transform.position, refRusher.transform.position) < 2f){

                Vector3 shoveDir = transform.position - refRusher.transform.position;
                shoveDir.y = 0f;
                shoveDir = Vector3.Normalize(shoveDir);
                shoveDir *= refRusher.GetComponent<AI_Athlete>().mBull;
                AI_Shove shove = new AI_Shove(shoveDir, refRusher.GetComponent<AI_Athlete>().mTag);
                cTakeShove.FTakeShove(shove);
            }

            // now we apply the affect of force to our velocity.
            cTakeShove.FRecalculateShoves();
            if(cTakeShove.mAllForces.magnitude > 0f){
                
                Debug.Log("Shove force to vel: " + cTakeShove.mAllForces);
                cRigid.velocity += cTakeShove.mAllForces;
            }else{
                Debug.Log("No shoving forces");
            }

        }


    }

    public void OnSnap()
    {
        cAthlete.mSpd = 0.5f;
        cAthlete.mBull = 900f;
        cAthlete.mWgt = 300f;
        cAthlete.mAnc = 100f; 
        cAthlete.mBks = 80f;  
        cAthlete.mBrst = 100f;  

        mActive = true;

        refRusher = FindObjectOfType<AI_Rusher>();
        refPlayer = FindObjectOfType<PC_Controller>();
    }
}
