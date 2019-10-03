/*************************************************************************************
Figures out what the meaning of the last play is. 

Turnover means 1st and 10. New down marker. 
*************************************************************************************/
using UnityEngine;

public class EX_PL_Res : TDC_Component
{
    private EX_Over                             cOverMan;
    private EX_Plays                            cPlays;
    private EX_PL_Pick                          cPick;
    private EX_PL_Live                          cLive;
    
    public UI_PL_Res                            mUI;

    private float                               mTime;

    void Start()
    {
        cPlays = GetComponent<EX_Plays>();
        cOverMan = GetComponent<EX_Over>();
        cPick = GetComponent<EX_PL_Pick>();
        cLive = GetComponent<EX_PL_Live>();

        mUI.gameObject.SetActive(false);
    }

    public override void FEnter()
    {
        cPlays.mState = EX_Plays.STATE.S_RESULT;
        mUI.gameObject.SetActive(true);
        
        // ---------------------------------- calc time left
        cPlays.mGameData.mTimeInQuarter -= cLive.mResult.mTimeTaken;
        cPlays.mGameData.mTimeStruct = UT_MinSec.FSecsToMin((int)cPlays.mGameData.mTimeInQuarter);
        if(cPlays.mGameData.mTimeInQuarter <= 0)
        {
            cPlays.mGameData.mQuarter++;
            // now special logic for the halftime or game being over.
            if(cPlays.mGameData.mQuarter == GameData.QUARTER.THIRD){
                Debug.Log("It's halftime");
                if(cPlays.mGameData.mReceivedFirst == GameData.POSSESSION.HOME){
                    cPlays.mGameData.mBallLoc.mSide = GameData.POSSESSION.AWAY;
                    cPlays.mGameData.mPossession = GameData.POSSESSION.AWAY;
                }else{
                    cPlays.mGameData.mBallLoc.mSide = GameData.POSSESSION.HOME;
                    cPlays.mGameData.mPossession = GameData.POSSESSION.HOME;
                }
                cPlays.mGameData.mBallLoc.mYardMark = 25;
                cPlays.mGameData.mDown = GameData.DOWN.FIRST;
                cPlays.mGameData.mDownMark = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, 10);

                return;
            }
            if(cPlays.mGameData.mQuarter == GameData.QUARTER.OT)
            {
                Debug.Log("Unless it's tied, game should be over");
                cPlays.FExit();
                cOverMan.FEnter();
            }

            cPlays.mGameData.mTimeStruct.mMin = 15;
            cPlays.mGameData.mTimeStruct.mSec = 0;
            cPlays.mGameData.mTimeInQuarter = UT_MinSec.FMinToSecs(cPlays.mGameData.mTimeStruct);
        }

        // now figure out the remaining down and distance.
        cPlays.mGameData.mBallLoc = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, cLive.mResult.mDis);
        int disToFirstDown = cPlays.FCalcDistance(cPlays.mGameData.mBallLoc, cPlays.mGameData.mDownMark, cPlays.mGameData.mPossession);        

        cPlays.mGameData.mDown++;
        if(disToFirstDown <= 0){
            // they got a first down.
            cPlays.mGameData.mDownMark = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, 10);
            cPlays.mGameData.mDown = GameData.DOWN.FIRST;
            disToFirstDown = 10;
        }
        
        // ------------------------------------------------------------- Show the result of the play.
        string res = "";
        switch(cLive.mResult.mChoice)
        {
            case PLAY_CHOICE.C_RUN: res += "Ran the ball for "; break;
            case PLAY_CHOICE.C_PASS: res += "Passed the ball for "; break;
            case PLAY_CHOICE.C_PUNT: res += "Punted the ball for "; break;
        }

        res += cLive.mResult.mDis + " yards";
        // kind of a hack, turnovers overwrite everything we just did.
        if(cPlays.mGameData.mDown == GameData.DOWN.LENGTH || cLive.mResult.mTurnover){
            // Turnover on downs.
            Debug.Log("Turnover, either downs or ingame, handle later");
            res = "Turnover, either downs or ingame";
        }
        mUI.mTxtRes.text = "Result: " + res;
        

        // "Ran the ball for 7 yards. "
        // "Turnover!"
        // "Ran for Touchdown"

        // --------------------------------------------------- Handle turnovers
        if(cPlays.mGameData.mDown == GameData.DOWN.LENGTH || cLive.mResult.mTurnover){
            Debug.Log("Turnover, either downs or ingame, handle later");
            cPlays.mGameData.mDown = GameData.DOWN.FIRST;
            cPlays.mGameData.mDownMark = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, 10);
            disToFirstDown = 10;
            if(cPlays.mGameData.mPossession == GameData.POSSESSION.HOME){
                cPlays.mGameData.mPossession = GameData.POSSESSION.AWAY;
            }else{
                cPlays.mGameData.mPossession = GameData.POSSESSION.HOME;
            }
        }

        // ------------------------------------------------------ Handle touchdowns, or safeties.
        if(cPlays.mGameData.mBallLoc.mYardMark == 0)
        {
            // handle safeties later.
            if(cPlays.mGameData.mPossession == GameData.POSSESSION.HOME){
                cPlays.mGameData.mScores.mHome += 7;
                cPlays.mGameData.mPossession = GameData.POSSESSION.AWAY;
                cPlays.mGameData.mBallLoc.mSide = GameData.POSSESSION.AWAY;
                cPlays.mGameData.mBallLoc.mYardMark = 25;
                cPlays.mGameData.mDown = GameData.DOWN.FIRST;
            }else{
                cPlays.mGameData.mScores.mAway += 7;
                cPlays.mGameData.mPossession = GameData.POSSESSION.HOME;
                cPlays.mGameData.mBallLoc.mSide = GameData.POSSESSION.HOME;
                cPlays.mGameData.mBallLoc.mYardMark = 25;
                cPlays.mGameData.mDown = GameData.DOWN.FIRST;
            }
            mUI.mTxtRes.text = "TOUCHDOWN!";
        }

        cPlays.mUI.FSetBallText(cPlays.mGameData.mBallLoc);
        cPlays.mUI.FSetDownAndDisText(cPlays.mGameData.mDown, disToFirstDown);
        cPlays.mUI.FSetTimeText(cPlays.mGameData.mTimeStruct, cPlays.mGameData.mQuarter);
        cPlays.mUI.FSetPossessionText(cPlays.mGameData.mPossession);
        cPlays.mUI.FSetScoresText(cPlays.mGameData.mScores);

        mTime = Time.time;
    }

    public override void FExit()
    {
        mUI.gameObject.SetActive(false);
    }

    public override void FRunUpdate()
    {
        // Should probably let you click a button.
        if(Time.time - mTime > 2f)
        {

        }
    }

    // When they are tired of looking at the text, now we take them to the next play.
    public void BT_Done()
    {
        FExit();
        cPick.FEnter();
    }

}
