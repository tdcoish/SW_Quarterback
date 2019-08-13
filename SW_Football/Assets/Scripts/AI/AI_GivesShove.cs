/*************************************************************************************
Got kind of tired of having to write out the same shoving code.
*************************************************************************************/
using UnityEngine;

public class AI_GivesShove : MonoBehaviour
{
    private AI_Athlete          cAth;
    private AI_TakesShove       cTakesShove;

    private void Start()
    {
        cAth = GetComponent<AI_Athlete>();
        cTakesShove = GetComponent<AI_TakesShove>();
    }

    /***************************************************************************
    Alrighty. Now we're about to add momentum here to the collisions/shoves. What that 
    means is if there's a large velocity mismatch between players, the hit power increases 
    quite a bit, proportionally.
    ************************************************************************* */

    // We need to get the person we're shoving passed in to us.
    public void FGiveShove(AI_Athlete other)
    {
        // First shove them for full shoving.
        Vector3 vShoveDir = other.transform.position - transform.position;
        vShoveDir.y = 0f;
        Vector3 vShovePow = Vector3.Normalize(vShoveDir) * cAth.mBull;
        AI_Shove shv = new AI_Shove(vShovePow, cAth.mTag);
        other.GetComponent<AI_TakesShove>().FTakeShove(shv);

        // now, receive some small percent of Newtons second, as a shoveback, factoring in us bracing for impact.
        AI_Shove selfShove = new AI_Shove(shv.mForce*-0.25f, cAth.mTag);
        cTakesShove.FTakeShove(selfShove);
    }
}
