﻿/*************************************************************************************
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

    public MARK_EndZone                         rAway;
    public MARK_EndZone                         rHome;
    public MARK_Center                          rCenter;
    public MARK_Ball                            rBall;
    public MARK_DownStart                       rDownStart;
    public MARK_FirstDown                       rFirstDown;
    
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
        

        // ---------------------------------------- figure out the remaining down and distance. Assuming no turnovers or halftime/over.
        cPlays.mGameData.mBallLoc = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, cLive.mResult.mDis);
        int disToFirstDown = cPlays.FCalcDistance(cPlays.mGameData.mBallLoc, cPlays.mGameData.mDownMark, cPlays.mGameData.mPossession);        

        cPlays.mGameData.mDown++;
        if(disToFirstDown <= 0){
            // they got a first down.
            cPlays.mGameData.mDownMark = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, 10);
            cPlays.mGameData.mDown = GameData.DOWN.FIRST;
            disToFirstDown = 10;

            FGFX_PlaceFirstDownMarkers(cPlays.mGameData.mBallLoc, cPlays.mGameData.mDownMark);
        }

        // --------------------------------------------------- Handle turnovers. Need to handle touchbacks here.
        if(cPlays.mGameData.mDown == GameData.DOWN.LENGTH || cLive.mResult.mTurnover){
            cPlays.mGameData.mDown = GameData.DOWN.FIRST;

            // handle touchback
            if(cPlays.mGameData.mBallLoc.mYardMark <= 0){
                cPlays.mGameData.mBallLoc.mYardMark = 20;
                Debug.Log("Touchback");
            }
            disToFirstDown = 10;
            if(cPlays.mGameData.mPossession == GameData.POSSESSION.HOME){
                cPlays.mGameData.mPossession = GameData.POSSESSION.AWAY;
            }else{
                cPlays.mGameData.mPossession = GameData.POSSESSION.HOME;
            }
            cPlays.mGameData.mDownMark = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, 10);

            FGFX_PlaceFirstDownMarkers(cPlays.mGameData.mBallLoc, cPlays.mGameData.mDownMark);
        }

        // ------------------------------------------------------ Handle touchdowns, or safeties.
        bool touchdown = false;
        if(cPlays.mGameData.mBallLoc.mYardMark == 0)
        {
            touchdown = true;
            cPlays.mGameData.mDown = GameData.DOWN.FIRST;
            cPlays.mGameData.mBallLoc.mYardMark = 25;
            // handle safeties later.
            if(cPlays.mGameData.mPossession == GameData.POSSESSION.HOME){
                cPlays.mGameData.mScores.mHome += 7;
                cPlays.mGameData.mPossession = GameData.POSSESSION.AWAY;
                cPlays.mGameData.mBallLoc.mSide = GameData.POSSESSION.AWAY;
            }else{
                cPlays.mGameData.mScores.mAway += 7;
                cPlays.mGameData.mPossession = GameData.POSSESSION.HOME;
                cPlays.mGameData.mBallLoc.mSide = GameData.POSSESSION.HOME;
            }

            cPlays.mGameData.mDownMark = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, 10);

            FGFX_PlaceFirstDownMarkers(cPlays.mGameData.mBallLoc, cPlays.mGameData.mDownMark);
        }

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

        // ------------------------------------------------------------- Show the result of the play.
        mUI.FSetResultText(cPlays.mChoice, cLive.mResult.mTurnover, cLive.mResult.mDis, touchdown);
        mUI.mPlayInfo.text = cLive.mResult.mInfo;

        cPlays.mUI.FSetBallText(cPlays.mGameData.mBallLoc);
        cPlays.mUI.FSetDownAndDisText(cPlays.mGameData.mDown, disToFirstDown);
        cPlays.mUI.FSetTimeText(cPlays.mGameData.mTimeStruct, cPlays.mGameData.mQuarter);
        cPlays.mUI.FSetPossessionText(cPlays.mGameData.mPossession);
        cPlays.mUI.FSetScoresText(cPlays.mGameData.mScores);

        // ------------------------------------------ Graphically move the ball forwards. 
        rBall.transform.position = GetPositionOnFieldInWorldCoordinates(cPlays.mGameData.mBallLoc);

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

    private Vector3 GetPositionOnFieldInWorldCoordinates(GameData.FIELD_POS fPos)
    {
        if(fPos.mSide == GameData.POSSESSION.HOME){
            Vector3 pos = rHome.transform.position;
            float percent = fPos.mYardMark*2f / 100f;
            pos = Vector3.Lerp(rHome.transform.position, rCenter.transform.position, percent);
            return pos;
        }else{
            Vector3 pos = rAway.transform.position;
            float percent = fPos.mYardMark*2f / 100f;
            pos = Vector3.Lerp(rAway.transform.position, rCenter.transform.position, percent);
            return pos;
        }
    }

    public void FGFX_PlaceFirstDownMarkers(GameData.FIELD_POS start, GameData.FIELD_POS firstDown)
    {
        Vector3 pos = GetPositionOnFieldInWorldCoordinates(start);
        pos.x = 0;
        rDownStart.transform.position = pos;
        pos = GetPositionOnFieldInWorldCoordinates(firstDown);
        pos.x = 0;
        rFirstDown.transform.position = pos;
    }

}