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
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum PP_State
{
    CHILLING,
    REC_ACTIVE
}

public class PP_Manager : MonoBehaviour
{
    
    public SO_Int               mScoreGlobal;
    public int                  mScore;

    public PP_UI                refUI;

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
    public PP_State             mState = PP_State.CHILLING;

    public GameObject           PF_Arrow;

    public GameObject           MN_PauseScreen;

    private void Start()
    {
        DeactivateReceiver();

        MN_PauseScreen.SetActive(false);
    }

    private void Update()
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
    }

    private void HandlePocketPosition()
    {
        if(mIsOutOfPocket)
        {
            if(Time.time - mLastTimeInPocket >= 1f)
            {
                // this could be confusing, mLastTimeInPocket isn't actually the last time in pocket after a while.
                mLastTimeInPocket = Time.time;
                mScore -= 25;
            }
        }
    }

    private void HandleSwitchingReceiverIfTimeRunsOut()
    {
        // now we focus on switching the receiver or not.
        if(Time.time - mLastReceiverSwitch > mReceiverSwitchInterval && mState == PP_State.REC_ACTIVE)
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

        mState = PP_State.CHILLING;
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

        Instantiate(PF_Arrow, vPos, mTargets[ind].transform.rotation);

        mLastReceiverSwitch = Time.time;

        mState = PP_State.REC_ACTIVE;
    }

    public void OnTargetHit()
    {
        if(mActiveTarget == -1)
        {
            refUI.TXT_Instr.text = "No active receivers";
            mScore -= 50;
            return;
        }

        if(Time.time - mTargets[mActiveTarget].mLastTimeHit < 0.1f)
        {
            refUI.TXT_Instr.text = "NICE!";
            mScore += 100;
            DeactivateReceiver();
            return;
        }

        mScore -= 50;
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
        Color col = refUI.mSackedTxt.color;
        col.a = 1f;
        refUI.mSackedTxt.color = col;

        mScore -= 100;

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
}
