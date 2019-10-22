/*************************************************************************************
Figures out what the meaning of the last play is. 

Turnover means 1st and 10. New down marker. 
*************************************************************************************/
using UnityEngine;

public class EX_PL_Res : TDC_Component
{
    private AD_Exhibition                       cAud;
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
    // public MARK_PlaySpot                        PF_PlaySpot;

    public GFX_YardGainLoss                     PF_YardGainLoss;

    public UI_PL_Res                            mUI;

    private int                                 mPlayNumForDrive;
    private float                               mTime;

    void Start()
    {
        cAud = GetComponentInChildren<AD_Exhibition>();
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
            cAud.FFirstDown();

            FGFX_PlaceFirstDownMarkers(cPlays.mGameData.mBallLoc, cPlays.mGameData.mDownMark);
        }

        // ------------------------------------------------------ Handle Field Goals
        if(cLive.mResult.mChoice == PLAY_CHOICE.C_KICK)
        {       
            if(cLive.mResult.mSuccessfulFieldGoal){
                if(cPlays.mGameData.mPossession == GameData.POSSESSION.HOME){
                    cPlays.mGameData.mScores.mHome += 3;
                }else{
                    cPlays.mGameData.mScores.mAway += 3;
                }
                cPlays.mGameData.mBallLoc.mYardMark = 25;
            }
            cAud.FFieldGoal(cLive.mResult.mSuccessfulFieldGoal);

            Debug.Log("Changing possession");
            if(cPlays.mGameData.mPossession == GameData.POSSESSION.HOME){
                cPlays.mGameData.mPossession = GameData.POSSESSION.AWAY;
            }else{
                cPlays.mGameData.mPossession = GameData.POSSESSION.HOME;
            }

            cPlays.mGameData.mDown = GameData.DOWN.FIRST;

            cPlays.mGameData.mDownMark = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, 10);
            FGFX_PlaceFirstDownMarkers(cPlays.mGameData.mBallLoc, cPlays.mGameData.mDownMark);
        }

        // --------------------------------------------------- Handle turnovers. Need to handle touchbacks here.
        if(cPlays.mGameData.mDown == GameData.DOWN.LENGTH || cLive.mResult.mTurnover){
            mPlayNumForDrive = 0;
            MARK_PlaySpot[] spots = FindObjectsOfType<MARK_PlaySpot>();
            foreach(MARK_PlaySpot s in spots){
                Destroy(s.gameObject);
            }

            cPlays.mGameData.mDown = GameData.DOWN.FIRST;

            // handle touchback
            bool touchBack = false;
            if(cPlays.mGameData.mBallLoc.mYardMark <= 0){
                cPlays.mGameData.mBallLoc.mYardMark = 20;
                touchBack = true;
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
            if(!touchBack){
                cAud.FTurnover();
            }
        }

        // ------------------------------------------------------ Handle touchdowns, or safeties.
        bool touchdown = false;
        if(cPlays.mGameData.mBallLoc.mYardMark == 0)
        {
            mPlayNumForDrive = 0;
            MARK_PlaySpot[] spots = FindObjectsOfType<MARK_PlaySpot>();
            foreach(MARK_PlaySpot s in spots){
                Destroy(s.gameObject);
            }

            touchdown = true;
            cAud.FTouchDown();
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

            if(cPlays.mGameData.mQuarter == GameData.QUARTER.OT){
                Debug.Log("Team won in overtime");
                cPlays.FExit();
                cOverMan.FEnter();
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

                
            }else if(cPlays.mGameData.mQuarter == GameData.QUARTER.OT)
            {
                if(cPlays.mGameData.mScores.mAway != cPlays.mGameData.mScores.mHome){
                    cPlays.FExit();
                    cOverMan.FEnter();
                    cPlays.mGameData.mDown++;
                }else{
                    Debug.Log("It's Overtime");
                    cAud.FOverTime();
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
            }else if(cPlays.mGameData.mQuarter == GameData.QUARTER.LENGTH){
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
        rBall.transform.position = FGetPositionOnFieldInWorldCoordinates(cPlays.mGameData.mBallLoc);

        // ------------------------------------------ Graphically render the result of the play. Show where they came from, to where they are.
        mPlayNumForDrive++;
        Vector3 curPos = FGetPositionOnFieldInWorldCoordinates(cPlays.mGameData.mBallLoc);
        curPos.x = mPlayNumForDrive;
        GameData.FIELD_POS fLastPos = cPlays.FCalcNewSpot(cPlays.mGameData.mBallLoc, cPlays.mGameData.mPossession, cLive.mResult.mDis*-1);
        Vector3 lastPos = FGetPositionOnFieldInWorldCoordinates(fLastPos);
        lastPos.x = mPlayNumForDrive;
        int yards = FGetRawYardDistance(cPlays.mGameData.mBallLoc, fLastPos);


        // ----------------------------- New way, just put down a quad in the right spot.
        Vector3 vRot = transform.rotation.eulerAngles; vRot.x = 90;
        Vector3 vPos = Vector3.Lerp(lastPos, curPos, 0.5f);
        GFX_YardGainLoss g = Instantiate(PF_YardGainLoss, vPos, Quaternion.Euler(vRot));
        g.transform.localScale = new Vector3(1f, Mathf.Abs(lastPos.z - curPos.z), 1f);
        if(cLive.mResult.mDis >= 0){
            g.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }else{
            g.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }

        // // now just spawn the little nodes for each yard between them?
        // for(int i=0; i<yards; i++){
        //     Vector3 iterPos = Vector3.Lerp(lastPos, curPos, (float)i/(float)yards);
        //     Instantiate(PF_PlaySpot, iterPos, transform.rotation);
        // }
        // ------------------------------------------
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

    public Vector3 FGetPositionOnFieldInWorldCoordinates(GameData.FIELD_POS fPos)
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

    public GameData.FIELD_POS FGetFieldPosFromWorldCoordinates(float z)
    {
        float disToAwayEnd = Mathf.Abs(z - rAway.transform.position.z);
        float disToHomeEnd = Mathf.Abs(z - rHome.transform.position.z);
        float disToCenter = Mathf.Abs(z - rCenter.transform.position.z);

        GameData.FIELD_POS pos;
        if(disToAwayEnd < disToHomeEnd){
            pos.mSide = GameData.POSSESSION.AWAY;
            float perc = disToAwayEnd / (disToCenter + disToAwayEnd);
            pos.mYardMark = (int)(50 * perc);
        }else{
            pos.mSide = GameData.POSSESSION.HOME;
            float perc = disToHomeEnd / (disToCenter + disToHomeEnd);
            pos.mYardMark = (int)(50 * perc);
        }

        return pos;
    }

    // Just returns the distance in yardage between two things. No sign at all.
    public int FGetRawYardDistance(GameData.FIELD_POS pos1, GameData.FIELD_POS pos2)
    {
        if(pos1.mSide == pos2.mSide){
            return System.Math.Abs(pos2.mYardMark - pos1.mYardMark);
        }else{
            return (50 - pos1.mYardMark) + (50 - pos2.mYardMark);
        }
    }

    public void FGFX_PlaceFirstDownMarkers(GameData.FIELD_POS start, GameData.FIELD_POS firstDown)
    {
        Vector3 pos = FGetPositionOnFieldInWorldCoordinates(start);
        pos.x = 0;
        rDownStart.transform.position = pos;
        pos = FGetPositionOnFieldInWorldCoordinates(firstDown);
        pos.x = 0;
        rFirstDown.transform.position = pos;
    }

}
