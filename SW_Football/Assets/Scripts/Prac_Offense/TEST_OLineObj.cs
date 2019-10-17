/*************************************************************************************
Testing out some things. For now these guys are going to move to the nearest rusher, and 
then try to block them.
*************************************************************************************/
using UnityEngine;

public class TEST_OLineObj : MonoBehaviour
{
    private Rigidbody                       cRigid;
    private ATH_Forces                      cForces;
    private bool                            mActive = false;
    private float                           mLastShoveTime;
    public float                            mShoveIntervalTime = 0.1f;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        cForces = GetComponent<ATH_Forces>();
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallSnapped, E_BallSnapped);
    }

    void Update()
    {
        if(mActive)
        {
            // ---------------------------------------------- Now we calculate our movement based on our net forces.
            // The offensive lineman are currently always trying to get back to not moving.  
            Vector3 vDir = cRigid.velocity.normalized;
            vDir *= -1f;
            Vector3 vPushForce = cForces.FFuncCalcInternalPush(vDir, cRigid.velocity, cForces.mInternalPwr, cForces.mTopSpd);
            if(Time.time - cForces.mLastForeignPushTime < 0.1f){
                vPushForce += cForces.mNetForces;
            }
            Vector3 vAcc = vPushForce / cForces.mWgt;
            vAcc *= Time.deltaTime;
            cRigid.velocity += vAcc;
        }
    }

    private void E_BallSnapped()
    {
        mActive = true;
    }

    private PRAC_Def_Ply FuncFindClosest(PRAC_Def_Ply[] players, Vector3 vOurPos)
    {
        int ixClose = 0;
        float fClosest = Vector3.Distance(players[0].transform.position, transform.position);
        for(int i=1; i<players.Length; i++){
            if(Vector3.Distance(players[i].transform.position, transform.position) < fClosest){
                fClosest = Vector3.Distance(players[i].transform.position, transform.position);
                ixClose = i;
            }
        }

        return players[ixClose];
    }

    private Vector3 FuncCalcUltraCloseSeparationForce(PRAC_Def_Ply[] players, Vector3 vOurPos)
    {
        Vector3 vShoveForce = new Vector3();
        foreach(PRAC_Def_Ply p in players){
            if(Vector3.Distance(transform.position, p.transform.position) < 0.2f){
                Vector3 vDir = transform.position - p.transform.position;
                vShoveForce += vDir.normalized;
            }
        }

        return vShoveForce;
    }
}
