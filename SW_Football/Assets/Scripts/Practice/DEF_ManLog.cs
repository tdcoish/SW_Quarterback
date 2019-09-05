/*************************************************************************************
Logic behind playing man.

For now I'm just having them try to stay 5 yards above the guy they're covering.

If they don't have a guy running routes, then they do their backup zone responsibility.
Maybe they also rush, idk.

*************************************************************************************/
using UnityEngine;

public class DEF_ManLog : MonoBehaviour
{
    private DEF_ZoneLog         cZoneLog;

    // Gets assigned to us somehow.
    public PRAC_Off             rMan;

    private void Start()
    {
        cZoneLog = GetComponent<DEF_ZoneLog>();

        SetBackupZoneSpot();
    }

    // Call this when the play is actually running.
    public void FRunMan()
    {
        if(rMan == null)
        {
            cZoneLog.FRunZone();
            return;
        }
        // just straight up run to the guy.
        Vector3 vSpotToGoTo = rMan.transform.position;
        vSpotToGoTo.z += 5f;
        Vector3 dis = vSpotToGoTo - transform.position;
        dis.y = 0f;
        dis = Vector3.Normalize(dis);

        GetComponent<PRAC_AI_Acc>().FCalcAcc(dis);
    }

    // Just a zone 10 yards back of where we start.
    private void SetBackupZoneSpot()
    {
        Vector3 vZoneSpot = transform.position;
        vZoneSpot.z += 10f;
        cZoneLog.mZoneSpot = vZoneSpot;
    }
}
