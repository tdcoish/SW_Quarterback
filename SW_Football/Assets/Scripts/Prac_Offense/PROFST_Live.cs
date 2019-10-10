/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PROFST_Live : PROFST_St
{

    public override void Start()
    {
        base.Start();
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallCaught_Rec, E_ReceiverCatchesBall);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallHitGround, E_BallHitsGround);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallCaught_Int, E_DefenderCatchesBall);
    }

    public override void FEnter(){
        cMan.mState = PRAC_STATE.SPLAY_RUNNING;
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;
        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            a.mState = PRAC_Ath.PRAC_ATH_STATE.SDOING_JOB;
        }
    }
    public override void FRun()
    {
        // Debug.Log("Running");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Now we just repeat the whole shebang.
            cPost.FEnter();
            return;
        }
    }

    public void E_BallHitsGround()
    {
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }

        if(cMan.mState == PRAC_STATE.SPLAY_RUNNING){
            cPost.FEnter();
        }
    }

    public void E_ReceiverCatchesBall()
    {
        Debug.Log("Receiver Caught ball!");
        cMan.mRes.mBallCaught = true;
        cPost.FEnter();
    }
    public void E_DefenderCatchesBall()
    {
        Debug.Log("Defender caught ball");
        cMan.mRes.mInt = true;
        cPost.FEnter();
    }
}
