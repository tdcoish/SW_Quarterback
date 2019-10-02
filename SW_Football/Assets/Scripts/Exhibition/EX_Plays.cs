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
    C_PUNT
}

public struct PLAY_RESULT{
    public PLAY_CHOICE                  mChoice;
    public int                          mDis;
    public bool                         mTurnover;
}

public struct GameData{
    public TDC_Time                     mTimeStruct;
    public float                        mTimeInQuarter;
    public enum QUARTER{
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        OT
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

    // So AWAY 37, or HOME 43, or whatever.
    public struct FIELD_POS{
        public POSSESSION               mSide;
        public int                      mYardMark;
    }
    public FIELD_POS                    mFieldLoc;

    public int                          mBallLoc;       // int might be the wrong datatype
    public int                          mMarkerDown;
}

public class EX_Plays : TDC_Component
{
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
        mGameData.mBallLoc = 20;
        mGameData.mFieldLoc.mSide = GameData.POSSESSION.HOME;
        mGameData.mFieldLoc.mYardMark = 20;
        mGameData.mMarkerDown = 30;      
        mGameData.mQuarter = GameData.QUARTER.FIRST;
        mGameData.mTimeStruct.mMin = 15;
        mGameData.mTimeStruct.mSec = 0;
        mGameData.mTimeInQuarter = UT_MinutesSeconds.FMinToSecs(mGameData.mTimeStruct);
        mGameData.mPossession = GameData.POSSESSION.HOME;
        mUI.FSetTimeText(mGameData.mTimeStruct, mGameData.mQuarter);
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
}
