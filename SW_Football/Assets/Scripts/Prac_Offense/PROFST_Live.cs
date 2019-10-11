/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PROFST_Live : PROFST_St
{
    private bool                                mNotLeft;

    public override void Start()
    {
        base.Start();
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallCaught_Rec, E_ReceiverCatchesBall);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallHitGround, E_BallHitsGround);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallCaught_Int, E_DefenderCatchesBall);
        TDC_EventManager.FAddHandler(TDC_GE.GE_Tackled, E_RunnerTackled);
    }

    public override void FEnter(){
        cMan.mState = PRAC_STATE.SPLAY_RUNNING;
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;
        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            a.mState = PRAC_Ath.PRAC_ATH_STATE.SDOING_JOB;
        }
        mNotLeft = true;
    }
    public override void FRun()
    {
        // Debug.Log("Running");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Now we just repeat the whole shebang.
            cPost.FEnter();
            FExit();
            return;
        }
    }
    public override void FExit()
    {
        cMan.cAud.FPlayWhistle();
    }

    public void E_BallHitsGround()
    {
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }

        if(cMan.mState == PRAC_STATE.SPLAY_RUNNING){
            cPost.FEnter();
            FExit();
        }
    }

    public void E_ReceiverCatchesBall()
    {
        Debug.Log("Receiver Caught ball!");
        cMan.mRes.mBallCaught = true;
        Invoke("EnterPost", 5f);
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }

        // ugh, this is a hack, I should be making a list of all the players, and keeping data about them.
        PRAC_Off_Ply[] offs = FindObjectsOfType<PRAC_Off_Ply>();
        PRAC_Off_Ply refP = null;
        foreach(PRAC_Off_Ply p in offs){
            if(p.mState == PRAC_Ath.PRAC_ATH_STATE.SRUN_WITH_BALL){
                refP = p;
            }
        }
        // make all the defenders try to tackle that guy.
        PRAC_Def_Ply[] defenders = FindObjectsOfType<PRAC_Def_Ply>();
        foreach(PRAC_Def_Ply d in defenders){
            // shit, even I don't know who the ball carrier is.
            d.GetComponent<DEF_TackLog>().FEnter();
            d.GetComponent<DEF_TackLog>().rBallCarrier = refP; 
            d.mTimeToTackle = true;
        }

        cMan.cAud.FCatch();
    }
    public void E_DefenderCatchesBall()
    {
        Debug.Log("Defender caught ball");
        cMan.mRes.mInt = true;
        cMan.cAud.FInterception();
        Invoke("EnterPost", 3f);
    }

    public void E_RunnerTackled()
    {
        cMan.cAud.FTackle();
        Invoke("EnterPost", 3f);
    }

    private void EnterPost()
    {
        // because we could invoke this multiple times. What a stupid name.
        if(!mNotLeft){
            return;
        }
        mNotLeft = false;
        Debug.Log("entered");
        cMan.cAud.FPlayWhistle();
        cPost.FEnter();
    }
}
