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
    private bool                                mCountdownActive;
    private float                               mCountdownTimer;
    private bool                                mBallThrown = false;
    public float                                mLastShotFire;
    public bool                                 mMakeCamFollowBall = false;

    public GFX_Star                             PF_StarGFX;

    public PRAC_PlayInfo                        mInfo;

    public override void Start()
    {
        base.Start();
        TDC_EventManager.FAddHandler(TDC_GE.GE_QB_ReleaseBall, E_BallThrown);
        TDC_EventManager.FAddHandler(TDC_GE.GE_Sack, E_Sack);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallHitFingers, E_BallHitsFingerTips);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallChangesHands, E_BallChangesHands);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallDropped, E_BallDropped);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallCaught_Rec, E_ReceiverCatchesBall);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallHitGround, E_BallHitsGround);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallCaught_Int, E_DefenderCatchesBall);
        TDC_EventManager.FAddHandler(TDC_GE.GE_Tackled, E_RunnerTackled);
    }

    public override void FEnter(){
        cMan.mState = PRAC_STATE.SPLAY_RUNNING;
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;
        foreach(PRAC_Ath a in cMan.rAths){
            a.mState = PRAC_Ath.PRAC_ATH_STATE.SDOING_JOB;
        }
        mCountdownActive = false;
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

        if(mCountdownActive){
            mCountdownTimer -= Time.deltaTime;
            if(mCountdownTimer <= 0f){
                EnterPost();
            }
        }
    }
    public override void FExit()
    {
        cMan.cAud.FPlayWhistle();
    }

    public void E_Sack()
    {
        if(mCountdownActive){
            return;
        }
        mInfo.mWasSack = true;
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }
        cMan.cAud.FSacked();
        mCountdownActive = true;
        mCountdownTimer = 1f;
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
            PROJ_Football refF = FindObjectOfType<PROJ_Football>();
            if(refF == null){
                return;
            }

            refF.FActivateCam();
        }

        // ------------------ Destroy all the turrets projectiles.
        PP_Projectile[] balls = FindObjectsOfType<PP_Projectile>();
        foreach(PP_Projectile b in balls){
            Destroy(b.gameObject);
        }
    }

    public void E_BallChangesHands()
    {
        // spawn a star on that guy.
        PRAC_Ath athWithBall = cMan.FGetBallCarrier();
        if(athWithBall != null){
            Vector3 vPos = athWithBall.transform.position; vPos.y = 0.1f;
            var clone = Instantiate(PF_StarGFX, vPos, athWithBall.transform.rotation);
            clone.transform.SetParent(athWithBall.transform);
        }
    }

    public void E_BallHitsFingerTips()
    {
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }

        // make all the defenders try to tackle now
        PRAC_Def_Ply[] defenders = FindObjectsOfType<PRAC_Def_Ply>();
        foreach(PRAC_Def_Ply d in defenders){
            // shit, even I don't know who the ball carrier is.
            d.GetComponent<DEF_TackLog>().FEnter();
            d.mTimeToTackle = true;
        }

        TDC_EventManager.FBroadcast(TDC_GE.GE_BallChangesHands);
    }
    public void E_BallDropped()
    {
        Debug.Log("Dropped");
        mInfo.mWasIncompletion = true;
        cMan.cAud.FSacked();
        mCountdownActive = true;
        mCountdownTimer = 1f;

        GFX_Star[] stars = FindObjectsOfType<GFX_Star>();
        foreach(GFX_Star s in stars){
            Destroy(s.gameObject);
        }
    }

    public void E_ReceiverCatchesBall()
    {
        mInfo.mWasCatch = true;

        mCountdownActive = true;
        mCountdownTimer = 5f;

        cMan.cAud.FCatch();
    }
    public void E_DefenderCatchesBall()
    {
        mInfo.mWasInterception = true;

        cMan.cAud.FInterception();
        mCountdownActive = true;
        mCountdownTimer = 3f;
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }
    }

    public void E_RunnerTackled()
    {
        if(cMan.FGetBallCarrier() == null){
            Debug.Log("Nobody has ball");
            return;
        }
        PRAC_Off_Ply guyWithBall = cMan.FGetBallCarrier().GetComponent<PRAC_Off_Ply>();

        if(guyWithBall.mState == PRAC_Ath.PRAC_ATH_STATE.STACKLED || guyWithBall.mState == PRAC_Ath.PRAC_ATH_STATE.SRUN_WITH_BALL){
            mInfo.mWasTackled = true;
            mInfo.mTackleSpot = guyWithBall.transform.position.z;             // should convert to field position. eg. HOME 35.
            mInfo.mYardsGained = Mathf.Abs(cMan.rSnapSpot.transform.position.z - guyWithBall.transform.position.z);
            Debug.Log("Yards gained: " + mInfo.mYardsGained);
        }

        cMan.cAud.FTackle();
        mCountdownActive = true;
        mCountdownTimer = 3f;
    }

    private void EnterPost()
    {

        PP_Projectile[] balls = FindObjectsOfType<PP_Projectile>();
        foreach(PP_Projectile b in balls){
            Destroy(b.gameObject);
        }
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
