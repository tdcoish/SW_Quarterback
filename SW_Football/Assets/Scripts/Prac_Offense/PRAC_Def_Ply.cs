/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PRAC_Def_Ply : PRAC_Ath
{
    private DEF_ZoneLog                     cZoneLog;
    private DEF_TackLog                     cTackLog;
    public bool                             mTimeToTackle = false;

    void Start()
    {
        cZoneLog = GetComponent<DEF_ZoneLog>();    
        cTackLog = GetComponent<DEF_TackLog>();
    }

    protected override void RUN_Job()
    {
        if(mTimeToTackle){ 
            cTackLog.FRun();
        }else{
            cZoneLog.FRunZone();
        }
    }

    // Run interception logic.
    public override void FCaughtBall()
    {
        
    }
}
