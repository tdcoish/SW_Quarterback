/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PRAC_Def_Ply : PRAC_Ath
{
    private PRAC_Ath                        cAth;
    private DEF_ZoneLog                     cZoneLog;
    private DEF_TackLog                     cTackLog;
    private DEF_RushLog                     cRushLog;
    public bool                             mTimeToTackle = false;

    void Start()
    {
        base.Start();
        cAth = GetComponent<PRAC_Ath>();
        cZoneLog = GetComponent<DEF_ZoneLog>();    
        cTackLog = GetComponent<DEF_TackLog>();
        cRushLog = GetComponent<DEF_RushLog>();
    }

    protected override void RUN_Job()
    {
        if(mTimeToTackle){ 
            cTackLog.FRun();
        }else{
            if(cAth.mJob.mRole == "Pass Rush"){
                cRushLog.FRunRush();
            }else{
                cZoneLog.FRunZone();
            }
        }
    }

    // Run interception logic.
    public override void FCaughtBall()
    {
        TDC_EventManager.FBroadcast(TDC_GE.GE_BallCaught_Int);   
    }
}
