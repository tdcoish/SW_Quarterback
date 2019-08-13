/*************************************************************************************
Got kind of tired of having to write out the same shoving code.
*************************************************************************************/
using UnityEngine;

public class AI_GivesShove : MonoBehaviour
{
    private AI_Athlete          cAth;
    private AI_TakesShove       cTakesShove;
    private Rigidbody           cRigid;             // for momentum

    private void Start()
    {
        cAth = GetComponent<AI_Athlete>();
        cTakesShove = GetComponent<AI_TakesShove>();
        cRigid = GetComponent<Rigidbody>();
    }

    /***************************************************************************
    Alrighty. Now we're about to add momentum here to the collisions/shoves. What that 
    means is if there's a large velocity mismatch between players, the hit power increases 
    quite a bit, proportionally.

    Question, do they really transfer 100% of their momentum into each other? 
    People aren't billiard balls, we have arms and balance, but for now I'll just pretend that 
    momentum is totally transfered, and dampen this later.

    Probably a better version would be to have momentum power increasing exponentially the closer
    to each other we get. That way, shoves at a distance are mostly arms, but bodies together are 
    mostly momentum. That sort of intuitively makes more sense.

    Ah, small bug, if their momentums are away from each other, then we should just ignore them?

    For now I'm just going to add some fudge factor where the strength grows exponentially from 2f-0f 
    distance.

    For us, we shouldn't be calculating their momentum, only the difference in velocity.
    ************************************************************************* */

    // We need to get the person we're shoving passed in to us.
    public void FGiveShove(AI_Athlete other)
    {
        // First shove them for full shoving.
        Vector3 vShoveDir = other.transform.position - transform.position;
        vShoveDir.y = 0f;
        vShoveDir = Vector3.Normalize(vShoveDir);
        Vector3 vShovePow = vShoveDir * cAth.mBull;

        // add momentum into shove. Remember, this is just us hitting him, we can have him hit us later.
        // it's the component of our velocity into him, + his component of velocity into us.
        float fDotVIntoHim = Vector3.Dot(cRigid.velocity, vShoveDir);
        float fDotVIntoUs = Vector3.Dot(other.GetComponent<Rigidbody>().velocity, -vShoveDir);

        // this confused me, but we're already factoring in the non-normalized velocities here.
        // if the combined vel is negative, they are moving away from each other.
        float fCombinedVel = (fDotVIntoHim + fDotVIntoUs);
        Vector3 vCombinedVel = fCombinedVel * vShoveDir;

        // remember, he hits us as well, so we don't factor in his weight.
        // Also, imagine if a 400 lbs guy hits a 100 lbs guy, there' no shared "500 lbs of momentum" like there's shared differential velocity
        Vector3 vMomPow = vCombinedVel * cAth.mWgt;
        vMomPow /= 10f;         // cause why not have some magic numbers so they don't fly off the screen.

        // // make momentum transfer exponentially less powerful the further from each other they are.
        float fMomPwr = Vector3.Distance(transform.position, other.transform.position);
        fMomPwr /= 2f;          // since they can effect each other from 2f away. fMomPwr is now the inverse percentage.
        fMomPwr = 1-fMomPwr;
        // fMomPwr*=fMomPwr;
        vMomPow *= fMomPwr;
        Debug.Log("Momentum transfer percentage allowed: " + fMomPwr);

        // visualizing strength of each shove component.
        Debug.DrawLine(transform.position, transform.position + Vector3.Cross(vShovePow, Vector3.forward), Color.green, 1f);
        Debug.DrawLine(transform.position, transform.position + Vector3.Cross(vMomPow, Vector3.forward), Color.red, 0.2f);

        vShovePow += vMomPow;

        AI_Shove shv = new AI_Shove(vShovePow, cAth.mTag);
        other.GetComponent<AI_TakesShove>().FTakeShove(shv);

        // now, receive some small percent of Newtons second, as a shoveback, factoring in us bracing for impact.
        AI_Shove selfShove = new AI_Shove(shv.mForce*-0.25f, cAth.mTag);
        cTakesShove.FTakeShove(selfShove);
    }
}
