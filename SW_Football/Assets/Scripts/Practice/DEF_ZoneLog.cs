/*************************************************************************************
Logic for zone.
*************************************************************************************/
using UnityEngine;

public class DEF_ZoneLog : MonoBehaviour
{
    private PRAC_Ath            cAth;
    private Rigidbody           cRigid;

    public Vector3              mZoneSpot;

    // Maybe have this as a function that we call.
    void Start()
    {   
        cRigid = GetComponent<Rigidbody>();
        cAth = GetComponent<PRAC_Ath>();

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

        cRigid.velocity = dis * 5f;
    }
}
