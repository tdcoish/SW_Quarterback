/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PRAC_Def : MonoBehaviour
{
    public Vector3              mZoneSpot;

    private PRAC_Ath            cAth;
    private Rigidbody           cRigid;

    void Start()
    {
        cAth = GetComponent<PRAC_Ath>();
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

    void Update()
    {
        switch(cAth.mState)
        {
            case PRAC_Ath.PRAC_ATH_STATE.SDOING_JOB: RUN_Job(); break;
        }
    }

    // have already figured out that we're 
    private void RUN_Job()
    {
        switch(cAth.mJob.mRole)
        {
            case "Zone": LOGIC_Zone(); break;
            case "Man": LOGIC_Man(); break;
            case "Pass Rush": LOGIC_PassRush(); break;
        }

    }

    private void LOGIC_Zone()
    {
        // make it run to it's zone spot every time.
        Vector3 dis = mZoneSpot - transform.position;
        dis.y = 0f;
        dis = Vector3.Normalize(dis);

        cRigid.velocity = dis * 5f;
    }

    private void LOGIC_Man()
    {
 
    }

    private void LOGIC_PassRush()
    {
        
    }
}
