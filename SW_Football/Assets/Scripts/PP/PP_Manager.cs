/*************************************************************************************
Start simple.

Just rip from madden for now. So the receivers come open randomly, stay open for x seconds.

If you throw to a receiver who isn't open, subtract points. If you miss, subtract points.
If you step out of the pocket, subtract points.

Alright, when sacked, you lose points, and the receivers essentially reset. In order to build up
anticipation, and give the player a chance, we need a second in between receivers. Also, the 
second the player throws the ball, we should be switching receivers. 
--- Maybe for now only when they actually hit the target.
So, wait a second, then make a receiver "hot". Repeat this every x seconds if not interrupted.
Interruptions are 
1) Getting sacked
2) Hitting the target.

Two things need to be added. First, if you make a throw, you can't be sacked while the ball is
in the air. However, if the throw does not hit the target, then you lose points. Maybe throwaway?
Nah, it's pocket passer.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum PP_State
{
    DISPLAY_INSTRUCTIONS,       // when we start, display instructions for the player to read.
    GAME_ACTIVE
}

// basically this is only if we need to activate a receiver or not.
public enum PP_GAME_STATE
{
    CHILLING,
    REC_ACTIVE
}

public class PP_Manager : MonoBehaviour
{
    
    public SO_Int               mScoreGlobal;
    public int                  mScore;

    public PP_UI                refUI;
    public GameObject           refInstrUI;

    private bool                mIsOutOfPocket = false;
    private float               mLastTimeInPocket;

    public float                mTimeLeft = 60f;

    // Need a list of the "receivers"
    public PP_Target[]          mTargets;
    private float               mLastReceiverSwitch;  
    public float                mReceiverSwitchInterval = 5f;  
    private float               mReceiverCatchableCountdown;
    public int                  mActiveTarget;

    public float                mWaitToMakeRecHot = 2f;
    public PP_State             mState;
    public PP_GAME_STATE        mGameState;         // only care when actually running.

    public GameObject           PF_Arrow;

    public GameObject           MN_PauseScreen;

    public bool                 mSackImmunity = false;
    public int                  mStreak = 0;
    public int                  mStreakBonus;

    private void Start()
    {
        SetStateInstructions();
    }

    private void Update()
    {

        switch(mState){
            case(PP_State.DISPLAY_INSTRUCTIONS): STATE_INSTRUCTIONS(); break;
            case(PP_State.GAME_ACTIVE): STATE_GAMERUNNING(); break;
        }

    }

    private void SetStateInstructions()
    {
        MN_PauseScreen.SetActive(false);
        refUI.gameObject.SetActive(false);

        mGameState = PP_GAME_STATE.CHILLING;
        mState = PP_State.DISPLAY_INSTRUCTIONS;

        // deactivate all the turrets and the pc in the scene.
        PP_Turret[] refTurrets = FindObjectsOfType<PP_Turret>();
        for(int i=0; i<refTurrets.Length; i++){
            refTurrets[i].FDeactivate();
        }
        PC_Controller refPC = FindObjectOfType<PC_Controller>();
        refPC.mActive = false;
    }

    private void SetStateGaming()
    {
        mState = PP_State.GAME_ACTIVE;
        mGameState = PP_GAME_STATE.CHILLING;

        refUI.gameObject.SetActive(true);
        refInstrUI.gameObject.SetActive(false);

        // Activate all the turrets and the pc in the scene.
        PP_Turret[] refTurrets = FindObjectsOfType<PP_Turret>();
        for(int i=0; i<refTurrets.Length; i++){
            refTurrets[i].FActivate();
        }
        PC_Controller refPC = FindObjectOfType<PC_Controller>();
        refPC.mActive = true;

        DeactivateReceiver();
    }

    private void STATE_INSTRUCTIONS()
    {
        if(Input.anyKey)
        {
            SetStateGaming();
        }
    }

    private void STATE_GAMERUNNING()
    {
        HandlePocketPosition();

        refUI.mScoreTxt.text = "Score: " + mScore;

        HandleSwitchingReceiverIfTimeRunsOut();

        HandleTimeLeft();

        // refUI.mScoreTxt.text = "STATE: " + mState;

        // for the build
        if(Input.GetKeyDown(KeyCode.L))
        {
            Application.Quit();
        }

        // if the user presses m, then they bring up the pause menu.
        if(Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0f;
            MN_PauseScreen.SetActive(true);

            // have to show the mouse, as well as disable the player camera.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // set streak text
        refUI.FSetStreakText(mStreakBonus);
    }

    private void HandlePocketPosition()
    {
        if(mIsOutOfPocket)
        {
            if(Time.time - mLastTimeInPocket >= 1f)
            {
                // this could be confusing, mLastTimeInPocket isn't actually the last time in pocket after a while.
                mLastTimeInPocket = Time.time;
                // this doesn't actually change the streak though.
                ChangeScore(-25, false);
            }
        }
    }

    private void HandleSwitchingReceiverIfTimeRunsOut()
    {
        // now we focus on switching the receiver or not.
        if(Time.time - mLastReceiverSwitch > mReceiverSwitchInterval && mGameState == PP_GAME_STATE.REC_ACTIVE)
        {
            // There's no active target for a sec.
            DeactivateReceiver();
        }
    }

    private void HandleTimeLeft()
    {
        mTimeLeft -= Time.deltaTime;
        refUI.FSetTimeText(mTimeLeft);

        if(mTimeLeft <= 0f)
        {
            mScoreGlobal.Val = mScore;
            SceneManager.LoadScene("SN_PP_Score");
        }
    }

    private void DeactivateReceiver()
    {
        mActiveTarget = -1;

        PP_Arrow[] arrows = FindObjectsOfType<PP_Arrow>();
        for(int i=0; i<arrows.Length; i++)
        {
            Destroy(arrows[i].gameObject);
        }

        Debug.Log("De-activating");
        Invoke("SetNewActiveTarget", mWaitToMakeRecHot);

        mGameState = PP_GAME_STATE.CHILLING;
    }

    private void SetNewActiveTarget()
    {
        Debug.Log("Activateing");
        if(mActiveTarget != -1)
        {
            Debug.Log("Already active receiver.");
            return;
        }
        // now we switch which receiver is active.
        // later, make it so it can't be the smae receiver.
        int ind = mActiveTarget;
        while(ind == mActiveTarget)
        {
            ind = (int)Random.Range(0, mTargets.Length);
        }
        mActiveTarget = ind;
        Vector3 vPos = mTargets[mActiveTarget].transform.position;
        vPos.y += 2f;

        var clone = Instantiate(PF_Arrow, vPos, mTargets[ind].transform.rotation);
        SetArrowMaterialColour(clone);

        mLastReceiverSwitch = Time.time;

        mGameState = PP_GAME_STATE.REC_ACTIVE;
    }

    private void SetArrowMaterialColour(GameObject arrow)
    {
        Renderer[] renderers = arrow.GetComponentsInChildren<Renderer>();

        Color col = Color.red;
        if(mStreakBonus == 1){
            col = Color.blue;
        }else if(mStreakBonus == 2){
            col = Color.green;
        }else if(mStreakBonus > 2){
            col = Color.yellow;
        }
    
        for(int i=0; i<renderers.Length; i++){
            renderers[i].material.SetColor("_Color", col);
        }
    }

    public void OnTargetHit()
    {
        DestroyFootballs();

        if(mActiveTarget == -1)
        {
            refUI.TXT_Instr.text = "No active receivers";
            ChangeScore(-50);
            return;
        }

        if(Time.time - mTargets[mActiveTarget].mLastTimeHit < 0.1f)
        {
            refUI.TXT_Instr.text = "NICE!";
            ChangeScore(100);
            DeactivateReceiver();
            return;
        }

        ChangeScore(-50);
        refUI.TXT_Instr.text = "Hit Wrong Receiver";
    }

    public void OnStepOutOfPocket()
    {
        mIsOutOfPocket = true;
        mLastTimeInPocket = Time.time;
    }

    public void OnStepIntoPocket()
    {
        mIsOutOfPocket = false;
    }

    public void OnBallHitPlayer()
    {
        if(mSackImmunity)
        {
            Debug.Log("Sacked while immune");
            return;
        }

        Color col = refUI.mSackedTxt.color;
        col.a = 1f;
        refUI.mSackedTxt.color = col;
                Debug.Log("Heree");

        ChangeScore(-100);

        DeactivateReceiver();
    }

    public void OnQuitPressed()
    {
        SceneManager.LoadScene("SN_MN_Main");
    }
    public void OnResumePressed()
    {
        MN_PauseScreen.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnRestartPressed()
    {
        Debug.Log("Gotta figure out how to restart");
    }
    
    // called all the time, when we hit a target, when the ball hits the ground, etcetera.
    // you can also be sacked again.
    private void DestroyFootballs()
    {
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        for(int i=0; i<footballs.Length; i++)
        {
            Destroy(footballs[i].gameObject);
        }

        mSackImmunity = false;
        refUI.mSackImmunityTxt.text = "Sack Immunity: NO";
    }

    public void OnBallThrown()
    {
        mSackImmunity = true;
        refUI.mSackImmunityTxt.text = "Sack Immunity: YES";
    }

    public void OnBallHitGround()
    {
        refUI.mSackImmunityTxt.text = "Sack Immunity: NO";
        mSackImmunity = false;
        DestroyFootballs();
        DeactivateReceiver();
        ChangeScore(-25);
    }

    // we factor in streak right here.
    // Has side effects, changes streak.
    // change can be negative
    private void ChangeScore(int chng, bool affectStreak = true)
    {
        if(chng < 0)
        {
            mStreak = 0;
            if(affectStreak){
                mScore += chng;
            }
        }
        else{
            if(affectStreak){
                mStreak++;
            }
            mScore += mStreak * chng;
        }

        mStreakBonus = mStreak;
        if(mStreakBonus < 0)
        {
            mStreakBonus = 0;
        }

        if(mStreakBonus > 4)
        {
            mStreakBonus = 4;
        }
    }
}
