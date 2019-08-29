/*************************************************************************************
Gonna try to add man coverage responsibilities here now. I guess they just cover whomever
is closest to them?
*************************************************************************************/
using UnityEngine;

public class PRAC_Def : PRAC_Ath
{
    private DEF_ManLog          cManLog;
    private DEF_ZoneLog         cZoneLog;
    private DEF_RushLog         cRushLog;

    void Start()
    {
        cManLog = GetComponent<DEF_ManLog>();
        cZoneLog = GetComponent<DEF_ZoneLog>();
        cRushLog = GetComponent<DEF_RushLog>();
    }

    // have already figured out that we're 
    protected override void RUN_Job()
    {
        switch(mJob.mRole)
        {
            case "Zone": cZoneLog.FRunZone(); break;
            case "Man": cManLog.FRunMan(); break;
            case "Pass Rush": cRushLog.FRunRush(); break;
        }

    }

}
