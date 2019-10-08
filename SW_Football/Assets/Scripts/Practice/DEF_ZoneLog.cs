/*************************************************************************************
Logic for zone.
*************************************************************************************/
using UnityEngine;

[RequireComponent(typeof(AI_Acc))]
[RequireComponent(typeof(PRAC_Ath))]
public class DEF_ZoneLog : MonoBehaviour
{
    private PRAC_Ath            cAth;
    private PRAC_AI_Acc         cAcc;
    private Rigidbody           cRigid;

    public Vector3              mZoneSpot;

    // Maybe have this as a function that we call.
    void Start()
    {   
        cAth = GetComponent<PRAC_Ath>();
        cAcc = GetComponent<PRAC_AI_Acc>();
        cRigid = GetComponent<Rigidbody>();
        
        if(cAth.mJob.mRole == "Zone")
        {
            mZoneSpot = IO_ZoneList.FLOAD_ZONE_BY_NAME(cAth.mJob.mDetail).mSpot;
            mZoneSpot.z = mZoneSpot.y;
            mZoneSpot.y = 0f;
            PLY_SnapSpot snap = FindObjectOfType<PLY_SnapSpot>();
            mZoneSpot += snap.transform.position;
        }
    }

    public void FRunZone()
    {
        // make it run to it's zone spot every time.
        Vector3 dis = mZoneSpot - transform.position;
        dis.y = 0f;
        dis = Vector3.Normalize(dis);

        Vector3 vAcc = cAcc.FCalcAccFunc(dis, cAcc.mSpd);
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > cAcc.mSpd){
            cRigid.velocity *= cAcc.mSpd/cRigid.velocity.magnitude;
        }
        transform.forward = cRigid.velocity.normalized;
    }
}
