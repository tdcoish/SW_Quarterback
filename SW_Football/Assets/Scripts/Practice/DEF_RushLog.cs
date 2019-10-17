/*************************************************************************************
Logic for pass rush.
*************************************************************************************/
using UnityEngine;

public class DEF_RushLog : MonoBehaviour
{
    private PRAC_Ath            cAth;
    private Rigidbody           cRigid;
    private PRAC_AI_Acc         cAcc;

    public bool                 mEngaged = false;

    // Maybe have this as a function that we call.
    void Start()
    {   
        cRigid = GetComponent<Rigidbody>();
        cAth = GetComponent<PRAC_Ath>();
        cAcc = GetComponent<PRAC_AI_Acc>();
    }

    // We just straight up run to the quarterback.
    public void FRunRush()
    {
        PC_Controller rPC = FindObjectOfType<PC_Controller>();
        if(rPC == null){
            Debug.Log("No pc");
            cRigid.velocity = Vector3.zero;
            return;
        }

        // "tackling" the QB.
        Vector3 vDis = rPC.transform.position - transform.position;
        Debug.DrawLine(rPC.transform.position, transform.position, Color.green);
        if(vDis.magnitude < 1f){
            TDC_EventManager.FBroadcast(TDC_GE.GE_Sack);        
            return;
        }

        // ---------------------------------- Here we simulate them getting "pushed" by the sphere's I'm pretending are olineman.
        TEST_OLineObj[] blockers = FindObjectsOfType<TEST_OLineObj>();
        // Vector3 vPush = FuncCalcPushForce(blockers, transform.position);
        // --------------------------------- Simplest implementation, offensive line just slows the rusher.
        float fSpd = cAcc.mSpd;
        if(FuncBlockerInRange(blockers, transform.position, 2f)){
            fSpd *= 0.4f;
        }
        
        // cRigid.velocity += vPush;

        vDis = Vector3.Normalize(vDis);
        Vector3 vAcc = cAcc.FCalcAccFunc(vDis, fSpd);
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > fSpd){
            cRigid.velocity *= fSpd/cRigid.velocity.magnitude;
        }
        transform.forward = cRigid.velocity.normalized;

    }

    private Vector3 FuncCalcPushForce(TEST_OLineObj[] blockers, Vector3 ourPos)
    {
        float fRange = 2f;
        float fMaxAcc = 0.5f;

        Vector3 vPush = new Vector3();
        foreach(TEST_OLineObj b in blockers)
        {
            Vector3 vDisB = ourPos - b.transform.position;
            vDisB = Vector3.Normalize(vDisB);
            // let's say no pushing until they're really close.
            float fDis = Vector3.Distance(ourPos, b.transform.position);
            if(fDis < fRange){
                float fInv = (fRange - fDis) / fRange;
                fInv = Mathf.Pow(fInv, 2f);
                vPush += vDisB * fMaxAcc * fInv;
            }
        }

        return vPush;
    }

    private bool FuncBlockerInRange(TEST_OLineObj[] blockers, Vector3 ourPos, float fRange)
    {
        foreach(TEST_OLineObj b in blockers){
            if(Vector3.Distance(ourPos, b.transform.position) < fRange){
                return true;
            }
        }

        return false;
    }
}
