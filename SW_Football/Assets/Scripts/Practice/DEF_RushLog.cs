/*************************************************************************************
Logic for pass rush.

When engaged, for now they can only bull rush. They take pushes, and they receive pushes,
but we don't apply Newtons second to their pushes, it just happens.

Net acceleration is then provided by our leftover force added into our weight.

New system. We run our own rush, sure, but a player manager figures out who's pushing on whom.

*************************************************************************************/
using UnityEngine;

public class DEF_RushLog : MonoBehaviour
{
    private PRAC_Ath            cAth;
    private Rigidbody           cRigid;
    private ATH_Forces          cForces;

    // Maybe have this as a function that we call.
    void Start()
    {   
        cRigid = GetComponent<Rigidbody>();
        cAth = GetComponent<PRAC_Ath>();
        cForces = GetComponent<ATH_Forces>();
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
        if(FuncBlockerInRange(blockers, transform.position, 2f)){
            TEST_OLineObj b = FuncGetClosestBlocker(blockers, transform.position);
            // now we apply a shove from them to us.
            Vector3 vDisDif = transform.position - b.transform.position;
            vDisDif = Vector3.Normalize(vDisDif);
            cForces.FTakePush(b.GetComponent<ATH_Forces>().mArmPwr * vDisDif, Time.time);

            // -------------------- And apply a shove from us to them.
            vDisDif *= -1;
            b.GetComponent<ATH_Forces>().FTakePush(cForces.mArmPwr * vDisDif, Time.time);
        }

        // ---------------------------------------------- Now we calculate our movement based on our net forces.
        vDis = rPC.transform.position - transform.position;        
        Vector3 vPushForce = cForces.FFuncCalcInternalPush(vDis.normalized, cRigid.velocity, cForces.mInternalPwr, cForces.mTopSpd);
        if(Time.time - cForces.mLastForeignPushTime < 0.1f){
            vPushForce += cForces.mNetForces;
        }
        Vector3 vAcc = vPushForce / cForces.mWgt;
        vAcc *= Time.deltaTime;
        cRigid.velocity += vAcc;

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

    private TEST_OLineObj FuncGetClosestBlocker(TEST_OLineObj[] blockers, Vector3 ourPos)
    {
        float fDis = Vector3.Distance(ourPos, blockers[0].transform.position);
        int ixClose = 0;
        for(int i=1; i<blockers.Length; i++){
            float temp = Vector3.Distance(ourPos, blockers[i].transform.position);
            if(temp < fDis){
                ixClose = i;
                fDis = temp;
            }
        }
        return blockers[ixClose];
    }
}
