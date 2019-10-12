/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public struct PRAC_PlayInfo{
    public float                        mYardsGained;
    public float                        mTackleSpot;
    public bool                         mWasTackled;
    public bool                         mWasInterception;
    public bool                         mWasIncompletion;
    public bool                         mWasCatch;
    public bool                         mWasSack;
    public bool                         mWasTouchdown;
}

public class PROFST_Live : PROFST_St
{
    private bool                                mNotLeft;
    public bool                                 mMakeCamFollowBall = false;
    private bool                                mBallThrown = false;
    public float                                mLastShotFire;

    public PRAC_PlayInfo                        mInfo;

    public override void Start()
    {
        base.Start();
        TDC_EventManager.FAddHandler(TDC_GE.GE_QB_ReleaseBall, E_BallThrown);
        TDC_EventManager.FAddHandler(TDC_GE.GE_PP_SackBallHit, E_SackBallHits);
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
        mBallThrown = false;
        mLastShotFire = Time.time;
        mInfo = ResetPlayInfo(mInfo);
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

        HandleTurrets();
    }
    public override void FExit()
    {
        cMan.cAud.FPlayWhistle();
    }

    private void HandleTurrets()
    {
        if(!cMan.mLineExists){
            return;
        }
        if(mBallThrown){
            return;
        }
        if(Time.time - mLastShotFire > 0.5f){
            int ind = Random.Range(0, cMan.rTurrets.Count);
            cMan.rTurrets[ind].FFireTurret();
            mLastShotFire = Time.time;
        }
    }

    public void E_SackBallHits()
    {
        mInfo.mWasSack = true;
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }
        cMan.cAud.FSacked();
        Invoke("EnterPost", 1f);
    }

    public void E_BallHitsGround()
    {
        mInfo.mWasIncompletion = true;
        
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }

        if(cMan.mState == PRAC_STATE.SPLAY_RUNNING){
            cPost.FEnter();
            FExit();
        }
    }

    // Super prototype-y, disable pc cam, enable football cam.
    public void E_BallThrown()
    {
        mBallThrown = true;

        if(mMakeCamFollowBall){
            PC_Controller refPC = FindObjectOfType<PC_Controller>();
            if(refPC == null){
                return;
            }
            refPC.GetComponentInChildren<Camera>().enabled = false;
            refPC.GetComponentInChildren<AudioListener>().enabled = false;
        }

        // ------------------ Destroy all the turrets projectiles.
        PP_Projectile[] balls = FindObjectsOfType<PP_Projectile>();
        foreach(PP_Projectile b in balls){
            Destroy(b.gameObject);
        }

    }

    public void E_ReceiverCatchesBall()
    {
        mInfo.mWasCatch = true;

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
        mInfo.mWasInterception = true;

        cMan.cAud.FInterception();
        Invoke("EnterPost", 3f);
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }
    }

    public void E_RunnerTackled()
    {
        // well shit, gonna need reference to that player.
        PRAC_Off_Ply[] offs = FindObjectsOfType<PRAC_Off_Ply>();
        foreach(PRAC_Off_Ply o in offs){
            if(o.mState == PRAC_Ath.PRAC_ATH_STATE.STACKLED || o.mState == PRAC_Ath.PRAC_ATH_STATE.SRUN_WITH_BALL){
                mInfo.mWasTackled = true;
                mInfo.mTackleSpot = o.transform.position.z;             // should convert to field position. eg. HOME 35.
                mInfo.mYardsGained = Mathf.Abs(cMan.rSnapSpot.transform.position.z - o.transform.position.z);
                Debug.Log("Yards gained: " + mInfo.mYardsGained);
            }
        }
        cMan.cAud.FTackle();
        Invoke("EnterPost", 3f);
    }

    private void EnterPost()
    {
        // because we could invoke this multiple times. What a stupid name.
        if(!mNotLeft){
            return;
        }
        PP_Projectile[] balls = FindObjectsOfType<PP_Projectile>();
        foreach(PP_Projectile b in balls){
            Destroy(b.gameObject);
        }
        mNotLeft = false;
        cMan.cAud.FPlayWhistle();
        cPost.FEnter();
    }

    private PRAC_PlayInfo ResetPlayInfo(PRAC_PlayInfo info)
    {
        info.mYardsGained = 0f;
        info.mTackleSpot = 0f;
        info.mWasTackled = false;
        info.mWasInterception = false;
        info.mWasIncompletion = false;
        info.mWasCatch = false;
        info.mWasSack = false;
        info.mWasTouchdown = false;
        return info;
    }
}
