/*************************************************************************************
Game Management controls everything about the play on the field, minus the actual AI. 
So, place the ball, bring up the play selection, snap the ball, figure out when the play
is over, and do it again. Halftime, end of game, etcetera. Basically the metagame.

For now, just figure out when the play is over and whether it was successful or not.

Need to keep track of where the "football" is. 
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


// for now just the basic loop of 
// Select play
// Pre snap
// Hike ball, play running.
// Play ends somehow.
// Over and over again.

public enum GAME_STATE
{
    PLAYSELECT,
    PRESNAP,
    RUNNING,
    RESOLUTION
}

public class DownAndDistance
{
    public int              mDown = 1;
    public float            mDis = 10;
}

public class QuarterTime
{
    public int              mQuarter = 1;       // convert 5+ to OT1, OT2...
    public float            mTime = 301;          // time left in quarter.
}

public class GameScore
{
    public int              mHomeScore = 0;
    public int              mAwayScore = 0;
}

public class GM_Manager : MonoBehaviour
{
    public bool                 mPlayOngoing = true;

    [SerializeField]
    private Text                rPlayRes;

    [SerializeField]
    private Text                rPlayState;

    private bool                mBallSnapped = false;
    public GE_Event             GE_BALL_SNAP;

    public PLY_SnapSpot         mSnapSpot;
    public PLY_FirstDownMark    mFirstDownSpot;

    public AI_Athlete[]         rAthletes;

    public GAME_STATE           mGameState;

    public PC_Controller        rPlayer;

    // quarter, time left.
    // down, distance, etcetera
    // game score.
    public QuarterTime          mQuarTime;
    public DownAndDistance      mDownAndDis;
    public GameScore            mScores;

    public Text                 rHomeScore;
    public Text                 rAwayScore;
    public Text                 rDownAndDis;
    public Text                 rTime;
    public Text                 rQuarter;

    public GE_Event             GE_HOME_SCORE;

    void Start()
    {
        mQuarTime = new QuarterTime();
        mDownAndDis = new DownAndDistance();
        mScores = new GameScore();
        mGameState = GAME_STATE.PRESNAP;

        // kind of a hack, move this later.
        SetFirstDownSpot();

        PlayRestart();
    }

    void Update()
    {
        if(!mBallSnapped && mGameState == GAME_STATE.PRESNAP){
            if(Input.GetKeyDown(KeyCode.Space))
            {
                GE_BALL_SNAP.Raise(null);
                mBallSnapped = true;
                mGameState = GAME_STATE.RUNNING;
            }

        }

        rPlayState.text = "STATE: " + mGameState;

        SetScoreText();
        SetDownAndDisText();
        SetTimeAndQuarterText();
        // testing scores
        if(Input.GetKeyDown(KeyCode.U)){
            GE_HOME_SCORE.Raise(null);
        }

        // make the timer count down.
        if(mGameState == GAME_STATE.RUNNING)
        {
            mQuarTime.mTime -= Time.deltaTime;
        }
    }

    public void OnIncompletion()
    {
        if(mPlayOngoing){
            rPlayRes.text = "Play Res: Incompletion";
            PlayOver();
        }
    }

    public void OnIntercept()
    {
        if(mPlayOngoing){
            rPlayRes.text = "Play Res: Interception";
            PlayOver();
        }
    }

    public void OnCatch()
    {
        if(mPlayOngoing){
            Vector3 snapPos = FindObjectOfType<PROJ_Football>().transform.position;
            snapPos.y = 0.2f;
            mSnapSpot.transform.position = snapPos;
            rPlayRes.text = "Play Res: Catch";
            PlayOver();
        }      
    }

    private void PlayOver()
    {
        mGameState = GAME_STATE.RESOLUTION;
        mBallSnapped = false;
        mPlayOngoing = false;
        rAthletes = FindObjectsOfType<AI_Athlete>();
        for(int i=0; i<rAthletes.Length; i++){
            rAthletes[i].OnPlayOver();
        }

        // manage down, distance gets updated when we render the text
        mDownAndDis.mDown++;

        // alright, now some logic, if we've gone past the chains, set up new downs.
        if(mSnapSpot.transform.position.z > mFirstDownSpot.transform.position.z){
            SetUpNewDowns();
        }
        // if we've gone past fourth down, turnover on downs.
    }

    public void PlayRestart()
    {
        mGameState = GAME_STATE.PRESNAP;
        mPlayOngoing = true;
        rPlayRes.text = "Play Res: Ongoing";

        SetSnapBetweenHashes();
        Vector3 pos = mSnapSpot.transform.position;
        pos.z -= 3;
        rPlayer.transform.position = pos;
    }

    private string NumContraction(int num)
    {
        if(num == 1){
            return "1st";
        }
        if(num == 2){
            return "2nd";
        }
        if(num == 3){
            return "3rd";
        }
        if(num == 4){
            return "4th";
        }
        else{
            return "OT";
        }
    }

    private void SetDownAndDisText()
    {
        mDownAndDis.mDis = mFirstDownSpot.transform.position.z - mSnapSpot.transform.position.z;
        double roundedDis = System.Math.Round((double)mDownAndDis.mDis, 0);
        string disText = roundedDis.ToString();
        if(roundedDis == 0){
            disText = "inches";
        }
        rDownAndDis.text = NumContraction(mDownAndDis.mDown) + " and " + disText;
    }
    private void SetTimeAndQuarterText()
    {
        rQuarter.text = NumContraction(mQuarTime.mQuarter);
        // setting time is a little more involved, since we need to convert from say, 320, to 5:20
        // since we do need a second 0, or it will look weird if we have 5:01, 5:0, 4:59, that's why the extra weirdness.
        int tm = (int)mQuarTime.mTime;
        int minTm = tm/60;
        int secTenTm = (tm%60)/10;
        int secTm = (tm%60) %10;
        rTime.text = minTm + ":" + secTenTm + secTm;
    }
    private void SetScoreText()
    {
        rHomeScore.text = "Home: " + mScores.mHomeScore;
        rAwayScore.text = "Away: " + mScores.mAwayScore;
    }
    // kinda need to know who scored.
    public void OnScore()
    {
        mScores.mHomeScore += 7;
        rHomeScore.text = "Home: " + mScores.mHomeScore;

        // now we need to shove the snap spot back to the 20 yard line.
        Vector3 pos = mSnapSpot.transform.position;
        pos.z = 30;
        mSnapSpot.transform.position = pos;

        SetUpNewDowns();
    }

    // here we need to make sure the snap spot is between the hashes.
    // hashes are 25.8 yards from the sidelines. We're doing everything in meters, which is fine.
    private void SetSnapBetweenHashes()
    {
        Vector3 snapPos = mSnapSpot.transform.position;
        if(snapPos.x < 25.8f){
            snapPos.x = 25.8f;
        }
        if(snapPos.x > 53-25.8f){
            snapPos.x = 53-25.8f;
        }

        mSnapSpot.transform.position = snapPos;
    }

    private void SetFirstDownSpot()
    {
        Vector3 spot = mSnapSpot.transform.position;
        spot.z += 10f;
        mFirstDownSpot.transform.position = spot;
    }

    private void SetUpNewDowns()
    {
        mDownAndDis.mDis = 10;
        mDownAndDis.mDown = 1;
        SetFirstDownSpot();
    }
}
