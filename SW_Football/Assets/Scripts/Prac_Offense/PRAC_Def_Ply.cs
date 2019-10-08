/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PRAC_Def_Ply : PRAC_Ath
{
    private DEF_ZoneLog                     cZoneLog;

    void Start()
    {
        cZoneLog = GetComponent<DEF_ZoneLog>();    
    }

    protected override void RUN_Job()
    {
        cZoneLog.FRunZone();
    }
}
