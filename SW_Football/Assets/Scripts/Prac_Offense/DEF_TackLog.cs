/*************************************************************************************
Tackle logic.
1) Just run straight at the ball carrier.
2) Run to where you think he's going to be, less so based on your distance.
*************************************************************************************/
using UnityEngine;

public class DEF_TackLog : MonoBehaviour
{
    private Rigidbody                                   cRigid;
    private PRAC_Ath                                    cAth;
    private PRAC_AI_Acc                                 cAcc;

    public COL_Tackle                                   cTackleBox;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        cAcc = GetComponent<PRAC_AI_Acc>();
        cAth = GetComponent<PRAC_Ath>();

        cTackleBox.gameObject.SetActive(false);
    }

    public void FEnter()
    {
        cTackleBox.gameObject.SetActive(true);
    }

    public void FRun()
    {
        // Depends if they're running towards or away from me. -- MAYBE
        // Only matters when I add moves into the game. Then, if behind, always chase down full speed. 
        // But if ahead, then maybe hold up so you don't get deked out.
        PRAC_Ath rBallCarrier = cAth.rMan.FGetBallCarrier();
        if(rBallCarrier == null){
            return;
        }
        // Say, run to where they're going to be, in one second, unless you're within 5 m, then shorten that time proportionally.
        float fLeadPerc = 0f;
        float fDis = Vector3.Distance(transform.position, rBallCarrier.transform.position);
        if(fDis > 5f)
        {
            fLeadPerc = 1f;
        }else{
            fLeadPerc = 5f/fDis;
        }

        Vector3 vSpotToMoveTo = rBallCarrier.transform.position;
        vSpotToMoveTo += rBallCarrier.GetComponent<Rigidbody>().velocity * fLeadPerc;              // so move 1 second ahead of them, unless we're really close.

        // they also need to be keeping the endzone between them and the ball carrier.

        Vector3 vDir = vSpotToMoveTo - transform.position;
        vDir = Vector3.Normalize(vDir);
        Vector3 vAcc = cAcc.FCalcAccFunc(vDir, cAcc.mSpd);
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > cAcc.mSpd){
            cRigid.velocity *= cAcc.mSpd / cRigid.velocity.magnitude;
        }
        transform.forward = Vector3.Normalize(cRigid.velocity);
    }
    
}
