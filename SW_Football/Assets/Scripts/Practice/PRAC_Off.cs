/*************************************************************************************
Test
*************************************************************************************/
using UnityEngine;

public class PRAC_Off : PRAC_Ath
{
    private OFF_RouteLog        cRouteLog;
    private OFF_BlockLog        cBlockLog;

    void Start()
    {
        cRouteLog = GetComponent<OFF_RouteLog>();
        cBlockLog = GetComponent<OFF_BlockLog>();
    }

    // have already figured out that we're 
    protected override void RUN_Job()
    {
        switch(mJob.mRole)
        {
            case "Route": cRouteLog.FRunRoute(); break;
            case "Pass Block": cBlockLog.FRunBlocking(); break;
        }

    }
}
