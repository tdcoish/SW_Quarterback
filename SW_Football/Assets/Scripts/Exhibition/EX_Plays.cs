/*************************************************************************************
The simulation of the actual plays. So, 2nd and 7, 14:45 to go in the fourth, opp 32 yard
line. That kind of thing.

Actually, I can already implement the fourth and 15 idea for keeping the ball instead of 
a kickoff.
*************************************************************************************/
using UnityEngine;

public enum PLAY_CHOICE{
    C_RUN,
    C_PASS,
    C_PUNT,
    C_KICK
}

public struct PLAY_RESULT{
    public PLAY_CHOICE                  mChoice;
    public string                       mInfo;              // eg. SACKED. Ran for 3 yards.
    public int                          mDis;
    public bool                         mTurnover;
    public float                        mTimeTaken;
    public bool                         mSuccessfulFieldGoal;
}

public struct GAME_SCORE{
    public int                          mHome;
    public int                          mAway;
}

public struct GameData{
    public TDC_Time                     mTimeStruct;
    public float                        mTimeInQuarter;
    public enum QUARTER{
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        OT,
        LENGTH
    }
    public QUARTER                      mQuarter;

    public enum DOWN{
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        LENGTH
    }
    public DOWN                         mDown;

    public enum POSSESSION{
        HOME,
        AWAY
    }
    public POSSESSION                   mPossession;

    public POSSESSION                   mReceivedFirst;

    // So AWAY 37, or HOME 43, or whatever.
    public struct FIELD_POS{
        public POSSESSION               mSide;
        public int                      mYardMark;
    }
    public FIELD_POS                    mBallLoc;
    public FIELD_POS                    mDownMark;

    public GAME_SCORE                   mScores;
}

public class EX_Plays : TDC_Component
{

    private AD_Exhibition               cAud;
    private EX_Man                      cMan;
    private EX_PL_Pick                  cPick;
    private EX_PL_Live                  cLive;
    private EX_PL_Res                   cRes;

    public enum STATE{
        S_PICKING,
        S_PLAYING,
        S_RESULT
    }
    public STATE                        mState;

    public PLAY_CHOICE                  mChoice;
    
    public GameData                     mGameData;
    public UI_Sim                       mUI;

    void Start()
    {
        cAud = GetComponentInChildren<AD_Exhibition>();
        cMan = GetComponent<EX_Man>();
        cPick = GetComponent<EX_PL_Pick>();
        cLive = GetComponent<EX_PL_Live>();
        cRes = GetComponent<EX_PL_Res>();

        mGameData = new GameData();

        mUI.gameObject.SetActive(false);
    }

    public override void FEnter()
    {
        mState = STATE.S_PICKING;
        cMan.mState = EX_Man.STATE.S_PLAYING;
        cPick.FEnter();

        cMan.mTxtInstructions.text = "Play the game";
        mUI.gameObject.SetActive(true);

        // Because the game starts with a fourth and 15 instead of a kickoff.
        mGameData.mDown = GameData.DOWN.FIRST;
        // I need a way of representing the ball location.
        // like OPP 40, or HOME 40, or something.
        mGameData.mBallLoc.mYardMark = 20;
        if(mGameData.mReceivedFirst == GameData.POSSESSION.HOME){
            mGameData.mBallLoc.mSide = GameData.POSSESSION.HOME;
            mGameData.mPossession = GameData.POSSESSION.HOME;
        }else{
            mGameData.mBallLoc.mSide = GameData.POSSESSION.AWAY;
            mGameData.mPossession = GameData.POSSESSION.AWAY;
        }
        mGameData.mDownMark = FCalcNewSpot(mGameData.mBallLoc, mGameData.mPossession, 10);
        cRes.FGFX_PlaceFirstDownMarkers(mGameData.mBallLoc, mGameData.mDownMark);
        // mGameData.mMarkerDown;      
        mGameData.mQuarter = GameData.QUARTER.FIRST;
        mGameData.mTimeStruct.mMin = 15;
        mGameData.mTimeStruct.mSec = 0;
        mGameData.mTimeInQuarter = UT_MinSec.FMinToSecs(mGameData.mTimeStruct);
        mUI.FSetTimeText(mGameData.mTimeStruct, mGameData.mQuarter);
    }

    public override void FExit()
    {
        mUI.gameObject.SetActive(false);
        cAud.FGameOver();
    }

    // Alright, here's where we're gonna need a state inside this state.
    public override void FRunUpdate()
    {
        switch(mState){
            case STATE.S_PICKING: cPick.FRunUpdate(); break;
            case STATE.S_PLAYING: cLive.FRunUpdate(); break;
            case STATE.S_RESULT: cRes.FRunUpdate(); break;
        }
    }

    public GameData.FIELD_POS FCalcNewSpot(GameData.FIELD_POS startSpot, GameData.POSSESSION hasBall, int dis)
    {
        GameData.FIELD_POS newSpot;
        newSpot.mSide = startSpot.mSide;

        if(startSpot.mSide == hasBall)
        {
            newSpot.mYardMark = startSpot.mYardMark + dis;
        }else{
            newSpot.mYardMark = startSpot.mYardMark - dis;
        }

        if(newSpot.mYardMark > 50){
            newSpot.mYardMark = 50 - (newSpot.mYardMark - 50);
            if(startSpot.mSide == GameData.POSSESSION.HOME){
                newSpot.mSide = GameData.POSSESSION.AWAY;
            }else{
                newSpot.mSide = GameData.POSSESSION.HOME;
            }
        }

        if(newSpot.mYardMark < 0){
            newSpot.mYardMark = 0;
        }

        return newSpot;
    }

    /**********************************************************************
    So if we pass in HOME 47, and AWAY 37, we should get a result of +16.

    Might have to return a FIELD_POS struct, with the mSide representing the 
    direction.
    **********************************************************************/
    public int FCalcDistance(GameData.FIELD_POS pos1, GameData.FIELD_POS pos2, GameData.POSSESSION hasBall)
    {
        int dis;
        if(pos2.mSide == pos1.mSide)
        {
            dis = pos2.mYardMark - pos1.mYardMark;
            if(hasBall != pos1.mSide){
                dis *= -1;
            }
        }else{
            dis = (50 - pos2.mYardMark) + (50 - pos1.mYardMark);
            if(pos1.mSide != hasBall){
                dis *= -1;
            }
        }

        return dis;
    }

    // Let's convert everything to HOME
    // If I'm HOME, I know I'm going to AWAY
    // And I'm on my OWN 45.
    // Therefore, I increase the number
    // But because OWN 55 is too far, that gets converted to 
    // AWAY 45
}
