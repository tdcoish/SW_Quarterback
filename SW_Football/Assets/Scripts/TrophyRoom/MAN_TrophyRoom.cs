﻿/*************************************************************************************
You just go back and forth between minigame trophies.

Gonna need to load in some trophies.
Gonna need to show trophies for different difficulties.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAN_TrophyRoom : MonoBehaviour
{
    
    public enum STATE{
        SINTRO,
        SMAIN,
        SOUTRO
    }
    public STATE                        mState;

    private CAM_TrophyRoom              rCam;
    public GameObject                   rPauseMenu;
    public GE_Event                     GE_PauseMenuOpened;
    public GE_Event                     GE_PauseMenuClosed;

    public int                          mActRoom = 0;
    public int                          mNumRooms = 2;
    public float                        mIntroTime = 2f;
    public float                        mTime;

    public DATA_PP_Dif                  mEasy;
    public DATA_PP_Dif                  mNorm;
    public DATA_PP_Dif                  mHard;
    public DATA_PP_Dif                  mPete;

    public DISPLAY_Minigame[]           mDisplayers;
    public GameObject                   PF_BronzeTrophy;
    public GameObject                   PF_SilverTrophy;
    public GameObject                   PF_GoldTrophy;


    void Start()
    {
        mState = STATE.SINTRO;
        rCam = FindObjectOfType<CAM_TrophyRoom>();
        mTime = Time.time;

        // ------------------------------
        // mAchievs = new DATA_Gamer();
        // mAchievs.mPPHighScores = new MinigameAchievement();
        // mAchievs.mRPHighScores = new MinigameAchievement();
        // mAchievs.mPPHighScores.mEasyScore = 2800;
        // mAchievs.mPPHighScores.mNormalScore = 2800;
        // mAchievs.mPPHighScores.mHardScore = 1200;
        // mAchievs.mPPHighScores.mPetermanScore = 250;

        // IO_GamerInfo.FWriteGamerData(mAchievs);
        IO_GamerInfo.FLoadGamerData();

        mEasy = IO_PP_Dif.FLoadDifficulty("EASY");
        mNorm = IO_PP_Dif.FLoadDifficulty("NORMAL");
        mHard = IO_PP_Dif.FLoadDifficulty("HARD");
        mPete = IO_PP_Dif.FLoadDifficulty("PETERMAN");

        // ---------------------------- SETUP easy trophies to start.
        SpawnTrophiesForDifficulty(IO_GamerInfo.mInfo.mPPHighScores.mEasyScore, mEasy, mDisplayers[0].mClusters[0]);
        SpawnTrophiesForDifficulty(IO_GamerInfo.mInfo.mPPHighScores.mNormalScore, mNorm, mDisplayers[0].mClusters[1]);
        SpawnTrophiesForDifficulty(IO_GamerInfo.mInfo.mPPHighScores.mHardScore, mHard, mDisplayers[0].mClusters[2]);
        SpawnTrophiesForDifficulty(IO_GamerInfo.mInfo.mPPHighScores.mPetermanScore, mPete, mDisplayers[0].mClusters[3]);

    }

    private void SpawnTrophiesForDifficulty(int playerScore, DATA_PP_Dif difData, TP_Cluster cluster)
    {
        if(playerScore > difData.mBronzeTrophy){
            Instantiate(PF_BronzeTrophy, cluster.mBronzeSpot.transform.position, transform.rotation);
        }
        if(playerScore > difData.mSilverTrophy){
            Instantiate(PF_SilverTrophy, cluster.mSilverSpot.transform.position, transform.rotation);
        }
        if(playerScore > difData.mGoldTrophy){
            Instantiate(PF_GoldTrophy, cluster.mGoldSpot.transform.position, transform.rotation);
        }
    }

    void Update()
    {
        switch(mState)
        {
            case STATE.SINTRO: FRUN_INTRO(); break;
            case STATE.SMAIN: FRUN_MAIN(); break;
        }
    }

    public void FENTER_INTRO()
    {
        mState = STATE.SINTRO;
        rCam.transform.position = rCam.mStart.mPos;
        rCam.transform.rotation = Quaternion.Euler(rCam.mStart.mRot);
    }
    public void FENTER_MAIN()
    {
        mState = STATE.SMAIN;
    }
    public void FRUN_INTRO()
    {
        float timePercent = (Time.time - mTime) / mIntroTime;
        if(timePercent > 1f){
            FENTER_MAIN();
            return;
        }

        Vector3 vRot = Vector3.Lerp(rCam.mStart.mRot, rCam.mSpots[0].mRot, timePercent);
        Vector3 vSpot = Vector3.Lerp(rCam.mStart.mPos, rCam.mSpots[0].mPos, timePercent);

        rCam.transform.position = vSpot;
        rCam.transform.rotation = Quaternion.Euler(vRot);
    }
    public void FRUN_MAIN()
    {
        
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            mActRoom++;
            if(mActRoom > mNumRooms-1){
                mActRoom = 0;
            }
        }

        // Update the camera
        rCam.transform.position = rCam.transform.position.Hermite(rCam.mSpots[mActRoom].mPos, 0.1f);
        rCam.transform.rotation = Quaternion.Euler(rCam.transform.rotation.eulerAngles.Hermite(rCam.mSpots[mActRoom].mRot, 0.1f));

        // if the user presses m, then they bring up the pause menu.
        if(Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0f;
            rPauseMenu.SetActive(true);

            // have to show the mouse, as well as disable the player camera.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GE_PauseMenuOpened.Raise(null);
        }
    }


    public void BT_Resume()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rPauseMenu.SetActive(false);
        GE_PauseMenuClosed.Raise(null);
    }

    public void BT_Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SN_MN_Main");
    }

}
