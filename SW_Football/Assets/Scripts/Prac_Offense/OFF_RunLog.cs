/*************************************************************************************
This logic basically tells them to run into the end zone.
*************************************************************************************/
using UnityEngine;

public class OFF_RunLog : MonoBehaviour
{
    public enum STATE
    {
        SJustReceived,
        SNormal
    }
    public STATE                                mState;

    private Rigidbody                           cRigid;
    private PRAC_AI_Acc                         cAcc;

    private void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        cAcc = GetComponent<PRAC_AI_Acc>();
    }

    // Hack, just increase z for now.
    public void FRunWithBall()
    {
        // Debug.Log("Here");
        
        Vector3 vDir = new Vector3(0f, 0f, 1f);
        Vector3 vAcc = cAcc.FCalcAccFunc(vDir, cAcc.mSpd);
        
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > cAcc.mSpd){
            cRigid.velocity *= cAcc.mSpd/cRigid.velocity.magnitude;
        }
        transform.forward = cRigid.velocity.normalized;
    }

}
