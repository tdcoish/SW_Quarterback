/*************************************************************************************
Testing out some things. For now these guys are going to move to the nearest rusher, and 
then try to block them.
*************************************************************************************/
using UnityEngine;

public class TEST_OLineObj : MonoBehaviour
{
    private Rigidbody                       cRigid;
    private bool                            mActive = false;
    private float                           mLastShoveTime;
    public float                            mShoveIntervalTime = 0.1f;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallSnapped, E_BallSnapped);
    }

    void Update()
    {
        if(false){
            // ----------------------- Move to nearest.
            PRAC_Def_Ply[] rushers = FindObjectsOfType<PRAC_Def_Ply>();
            if(rushers.Length == 0){
                return;
            }
            PRAC_Def_Ply closest = FuncFindClosest(rushers, transform.position);

            Vector3 vGoalSpot = closest.transform.position + closest.GetComponent<Rigidbody>().velocity * 0.2f;
            Vector3 vDis = vGoalSpot - transform.position;
            Vector3 vAcc = Vector3.Normalize(vDis) * 10f * Time.deltaTime;
            cRigid.velocity += vAcc;
            if(cRigid.velocity.magnitude > 2f){
                cRigid.velocity *= 2f / cRigid.velocity.magnitude;
            }

            // ---------------------- Shove nearest.
            if(Time.time - mLastShoveTime > mShoveIntervalTime){
                mLastShoveTime = Time.time;
                Vector3 vShove = closest.transform.position - transform.position;
                vShove = Vector3.Normalize(vShove);
                vShove *= 2f;

                closest.GetComponent<Rigidbody>().velocity += vShove;
            }

            // --------------------- And get shoved if we get really really close.
            Vector3 vShoveAcc = FuncCalcUltraCloseSeparationForce(rushers, transform.position);
            cRigid.velocity += vShoveAcc;
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
