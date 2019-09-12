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
using System.Collections.Generic;

public enum PP_State
{
    DISPLAY_INSTRUCTIONS,       // when we start, display instructions for the player to read.
    GAME_ACTIVE,
    SCORE_SCREEN
}

// basically this is only if we need to activate a receiver or not.
public enum PP_GAME_STATE
{
    CHILLING,
    REC_ACTIVE
}

public class PP_Manager : MonoBehaviour
{
    private PP_Man_Tur          cTurMan;
    private PP_Man_Targ         cTargMan;
    private PP_Man_Arr          cArrMan;
    private PP_Man_Trophy       cTrophMan;
    private AD_PP               cAud;

    public SO_Int               mScoreGlobal;
    public int                  mScore;

    public PP_UI                refUI;
    public GameObject           refInstrUI;
    public GameObject           refScoreboardUI;
    public GameObject           refQuitUI;

    private PC_Controller       refPC;

    private bool                mIsOutOfPocket = false;
    private float               mLastTimeInPocket;

    public float                mGameTime = 60f;
    public float                mTimeLeft;

    public PP_State             mState;
    public PP_GAME_STATE        mGameState;         // only care when actually running.

    public GameObject           MN_PauseScreen;

    public bool                 mSackImmunity = false;
    public int                  mStreak = 0;
    public int                  mStreakBonus = 1;

    public GE_Event             GE_PauseMenuOpened;
    public GE_Event             GE_PauseMenuClosed;

    // -------------------------------------- Get rid of eventually.
    public DATA_PP_Dif          lDifData;
    public PP_Turret            PF_Turret;
    public GameObject           PF_Target;
    public string               mDif = "PETERMAN";
    public bool                 mSaveCurrent = true;

    private void Start()
    {
        IO_Settings.FLOAD_SETTINGS();

        cTurMan = GetComponent<PP_Man_Tur>();
        cTargMan = GetComponent<PP_Man_Targ>();
        cArrMan = GetComponent<PP_Man_Arr>();
        cTrophMan = GetComponent<PP_Man_Trophy>();
        cAud = GetComponent<AD_PP>();

        refPC = FindObjectOfType<PC_Controller>();

        SetStateInstructions();

        // ------------------------------
        if(mSaveCurrent){
            lDifData = IO_PP_Dif.FGetCurrent();
            IO_PP_Dif.FSaveCurrent(lDifData);
        }
        // lDifficultyData = IO_PP_Dif.FLoadDifficulty(lDifficultyData.mName);
        lDifData = IO_PP_Dif.FLoadDifficulty(mDif);
        SetUpDifficulty();
    }

    private void Update()
    {
        cTurMan.refTurrets = FindObjectsOfType<PP_Turret>();
        cTargMan.refTargets = FindObjectsOfType<PP_Target>();
        
        switch(mState){
            case(PP_State.DISPLAY_INSTRUCTIONS): STATE_INSTRUCTIONS(); break;
            case(PP_State.GAME_ACTIVE): STATE_GAMERUNNING(); break;
            case(PP_State.SCORE_SCREEN): STATE_SCORESCREEN(); break;
        }

    }

    private void SetStateInstructions()
    {
        Cursor.lockState = CursorLockMode.Locked;

        MN_PauseScreen.SetActive(false);
        refUI.gameObject.SetActive(false);
        refInstrUI.SetActive(true);
        refScoreboardUI.SetActive(false);

        mGameState = PP_GAME_STATE.CHILLING;
        mState = PP_State.DISPLAY_INSTRUCTIONS;

        // Note, this needs to be polished, the rotations can get wonky.
        refPC.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        refPC.GetComponentInChildren<PC_Camera>().transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        refPC.mState = PC_Controller.PC_STATE.SINACTIVE;
        refPC.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // this is kind of to solve a bug with respect to throwing.
        refPC.GE_QB_StopThrow.Raise(null);
    
        DestroyExistingProjectilesArrowsAndDeactivateTurrets();
        // Also destroy all footballs
        PROJ_Football[] refFootballs = FindObjectsOfType<PROJ_Football>();
        for(int i=0; i<refFootballs.Length; i++){
            Destroy(refFootballs[i].gameObject);
        }
    }

    private void SetStateGaming()
    {
        Cursor.lockState = CursorLockMode.Locked;

        mState = PP_State.GAME_ACTIVE;
        mGameState = PP_GAME_STATE.CHILLING;

        refUI.gameObject.SetActive(true);
        refInstrUI.gameObject.SetActive(false);
        refScoreboardUI.SetActive(false);

        // testing this bug.
        Time.timeScale = 1f;

        // Activate all the turrets and the pc in the scene.
        cTurMan.FActivateTurrets();

        PC_Controller refPC = FindObjectOfType<PC_Controller>();
        refPC.mState = PC_Controller.PC_STATE.SACTIVE;
        Vector3 vPCPos = FindObjectOfType<PP_Pocket>().transform.position;
        vPCPos.y = 1f;
        refPC.transform.position = vPCPos;

        mScore = 0;
        ChangeScore(0);     // ah side effects, lovely aren't they?
        mStreak = 0;
        mStreakBonus = 1;

        mTimeLeft = mGameTime;

        cTargMan.FDeactivateReceiver();
    }

    private void SetStateScoreScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mState = PP_State.SCORE_SCREEN;

        refUI.gameObject.SetActive(false);
        refInstrUI.gameObject.SetActive(false);
        refScoreboardUI.SetActive(true);

        DestroyExistingProjectilesArrowsAndDeactivateTurrets();

        PC_Controller refPC = FindObjectOfType<PC_Controller>();
        refPC.mState = PC_Controller.PC_STATE.SINACTIVE;
        refPC.GetComponent<Rigidbody>().velocity = Vector3.zero;
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

        cTargMan.FHandleSwitchingReceiverIfTimeRunsOut();

        HandleTimeLeft();

        cTurMan.FHandleTurrets();

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

            FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SINACTIVE;

            GE_PauseMenuOpened.Raise(null);
        }

        // set streak text
        refUI.FSetStreakText(mStreakBonus);
    }

    // come to think of it, the buttons are what do everything, we just zoom a camera around.
    private void STATE_SCORESCREEN()
    {

    }

    private void DestroyExistingProjectilesArrowsAndDeactivateTurrets()
    {
        // deactivate all the turrets
        cTurMan.FDeactivateTurrets();
        // Destroy any projectiles that may be up.
        PP_Projectile[] refProjectiles = FindObjectsOfType<PP_Projectile>();
        for(int i = 0; i<refProjectiles.Length; i++){
            Destroy(refProjectiles[i].gameObject);
        }
        
        cArrMan.FDestroyArrows();
    }

    private void HandlePocketPosition()
    {
        if(mIsOutOfPocket)
        {
            if(Time.time - mLastTimeInPocket >= 1f)
            {
                cAud.FPlayClip(cAud.mOutOfPocket);
                // this could be confusing, mLastTimeInPocket isn't actually the last time in pocket after a while.
                mLastTimeInPocket = Time.time;
                // this doesn't actually change the streak though.
                ChangeScore(-25, false);
            }
        }
    }

    private void HandleTimeLeft()
    {
        mTimeLeft -= Time.deltaTime;
        refUI.FSetTimeText(mTimeLeft);

        if(mTimeLeft <= 0f)
        {
            // There's some unity weirdness here I think, where this wasn't updated until later.
            mScoreGlobal.Val = mScore;
            SetStateScoreScreen();
        }
    }

    // This needs a looksie
    public void OnTargetHit()
    {
        DestroyFootballs();

        if(cTargMan.mActiveTarget == -1)
        {
            refUI.TXT_Instr.text = "No active receivers";
            cAud.FPlayClip(cAud.mTargetFailure);
            ChangeScore(-50);
            return;
        }

        // bad way of saying "this target was hit this frame, or a frame or two ago
        if(Time.time - cTargMan.refTargets[cTargMan.mActiveTarget].mLastTimeHit < 0.1f)
        {
            refUI.TXT_Instr.text = "NICE!";
            cAud.FPlayClip(cAud.mTargetSuccess);
            ChangeScore(100);
            cTargMan.FDeactivateReceiver();
            return;
        }

        cAud.FPlayClip(cAud.mTargetFailure);
        ChangeScore(-50);
        refUI.TXT_Instr.text = "Hit Wrong Receiver";
        cTargMan.FDeactivateReceiver();
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

        cAud.FPlayClip(cAud.mSacked);
        Color col = refUI.mSackedTxt.color;
        col.a = 1f;
        refUI.mSackedTxt.color = col;

        ChangeScore(-100);

        cTargMan.FDeactivateReceiver();
    }

    // Since we're displaying the scoreboard screen, this is still fine
    public void OnQuitPressed()
    {
        refUI.gameObject.SetActive(false);
        MN_PauseScreen.SetActive(false);
        refScoreboardUI.SetActive(false);
        refQuitUI.SetActive(true);
    }
    public void OnResumePressed()
    {
        MN_PauseScreen.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;

        GE_PauseMenuClosed.Raise(null);
    }
    public void OnRestartPressed()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetStateInstructions();
        GE_PauseMenuClosed.Raise(null);
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
        cAud.FPlayClip(cAud.mBallHitGround);
        refUI.mSackImmunityTxt.text = "Sack Immunity: NO";
        mSackImmunity = false;
        DestroyFootballs();
        cTargMan.FDeactivateReceiver();
        ChangeScore(-25);
    }

    // we factor in streak right here.
    // Has side effects, changes streak.
    // change can be negative
    private void ChangeScore(int chng, bool affectStreak = true)
    {
        if(mState != PP_State.GAME_ACTIVE){
            Debug.Log("Can't score in this state");
            return;
        }

        if(chng < 0)
        {
            mScore += chng;
            if(affectStreak){
                mStreak = 0;
            }
        }
        else{
            mScore += mStreakBonus * chng;
            if(affectStreak){
                mStreak++;
            }
        }

        mStreakBonus = mStreak+1;
        if(mStreakBonus < 0)
        {
            mStreakBonus = 0;
        }

        if(mStreakBonus > 4)
        {
            mStreakBonus = 4;
        }

        cTrophMan.FHandleTrophyAfterScore();
    }

    private void SetUpDifficulty()
    {
        PP_Target[] targs = FindObjectsOfType<PP_Target>();
        foreach(PP_Target t in targs){
            Destroy(t.transform.parent.gameObject);
        }
        PP_Turret[] turs = FindObjectsOfType<PP_Turret>();
        foreach(PP_Turret t in turs){
            Destroy(t.gameObject);
        }

        PP_Pocket pock = FindObjectOfType<PP_Pocket>();
        pock.transform.localScale = lDifData.mPocketScale;

        for(int i=0; i<lDifData.mNumTurrets; i++){
            // yeah might have to store the rotation.
            Instantiate(PF_Turret, lDifData.mTurretSpots[i], transform.rotation);
        }

        for(int i=0; i<lDifData.mNumTargets; i++){
            var t = Instantiate(PF_Target, lDifData.mTargetSpots[i], transform.rotation);
            t.transform.localScale = lDifData.mTargetScales[i];
        }

        cTrophMan.GB_TrophyValues.mBronze = lDifData.mBronzeTrophy;
        cTrophMan.GB_TrophyValues.mSilver = lDifData.mSilverTrophy;
        cTrophMan.GB_TrophyValues.mGold = lDifData.mGoldTrophy;
    }

}
