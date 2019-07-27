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
    public int          mDown = 1;
    public int          mDis = 10;
}

public class QuarterTime
{
    public int              mQuarter = 1;       // convert 5+ to OT1, OT2...
    public int              mTime = 300;          // time left in quarter.
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

    public AI_Athlete[]         rAthletes;

    public GAME_STATE           mGameState;

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
            rPlayRes.text = "Play Res: Catch";
            PlayOver();
        }
        
        Vector3 snapPos = FindObjectOfType<PROJ_Football>().transform.position;
        snapPos.y = 0.2f;
        mSnapSpot.transform.position = snapPos;
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
    }

    public void PlayRestart()
    {
        mGameState = GAME_STATE.PRESNAP;
        mPlayOngoing = true;
        rPlayRes.text = "Play Res: Ongoing";

        SetSnapBetweenHashes();
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
        rDownAndDis.text = NumContraction(mDownAndDis.mDown) + " and " + mDownAndDis.mDis;
    }
    private void SetTimeAndQuarterText()
    {
        rQuarter.text = NumContraction(mQuarTime.mQuarter);
        rTime.text = mQuarTime.mTime.ToString();
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
}
