/*************************************************************************************
Logic for pass rush.
*************************************************************************************/
using UnityEngine;

public class DEF_RushLog : MonoBehaviour
{
    private PRAC_Ath            cAth;
    private Rigidbody           cRigid;
    private PRAC_AI_Acc         cAcc;

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
            cRigid.velocity = Vector3.zero;
            return;
        }

        Vector3 vDis = rPC.transform.position - transform.position;
        vDis = Vector3.Normalize(vDis);
        Vector3 vAcc = cAcc.FCalcAccFunc(vDis, cAcc.mSpd);
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > cAcc.mSpd){
            cRigid.velocity *= cAcc.mSpd/cRigid.velocity.magnitude;
        }
        
    }
}
