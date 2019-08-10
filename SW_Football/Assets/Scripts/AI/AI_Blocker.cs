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

    // for now I'll just give him the defender to block
    private AI_Rusher           refRusher;
    private PC_Controller       refPlayer;

    // sort of simulating getting shoved.
    public float                mForceApplied;
    public Vector3              mForceDir;

    private bool                mActive = false;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();

        cAthlete = GetComponent<AI_Athlete>();
        cTakeShove = GetComponent<AI_TakesShove>();
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

            Vector3 vel = midSpot - transform.position;
            vel = Vector3.Normalize(vel);
            vel *= cAthlete.mSpd;
            cRigid.velocity = vel;

            // now we apply the effect of force.
            //cRigid.velocity *= (100f-mForceApplied)/100f;
 
            // for now, just calculate if our rusher is within our sphere of influence.
            if(Vector3.Distance(transform.position, refRusher.transform.position) < 2f && !mShoved){

                // now we need to simulate for both of them the effects of each other on them.
                // force is applied, divided by handfighting skill, then we minus mAnc, then we get the real force.
                mForceApplied = refRusher.GetComponent<AI_Athlete>().mBull;
                mForceApplied -= cAthlete.mAnc;

                mForceDir = Vector3.Normalize(transform.position - refRusher.transform.position);
            }

            // now we apply the affect of force to our velocity.
            if(mForceApplied > 0f){

                Vector3 forceVel = mForceDir;
                forceVel *= cAthlete.mSpd;
                forceVel *= mForceApplied;

                // have to account for weight of course.
                forceVel /= cAthlete.mWgt;
                cRigid.velocity += forceVel;
            }

            // shoves decay to zero after 1 second max.
            mForceApplied -= Time.deltaTime * 100f;
            if(mForceApplied < 0f) mForceApplied = 0f;
        }


    }

    public void OnSnap()
    {
        cAthlete.mSpd = 0.5f;
        cAthlete.mWgt = 300f;
        cAthlete.mAnc = 100f;     

        mActive = true;

        refRusher = FindObjectOfType<AI_Rusher>();
        refPlayer = FindObjectOfType<PC_Controller>();
    }
}
