/*************************************************************************************
Figures out what the meaning of the last play is. 
*************************************************************************************/
using UnityEngine;

public class EX_PL_Res : TDC_Component
{
    
    private EX_Plays                            cPlays;
    private EX_PL_Pick                          cPick;
    private EX_PL_Live                          cLive;
    
    public UI_PL_Res                            mUI;

    private float                               mTime;

    void Start()
    {
        cPlays = GetComponent<EX_Plays>();
        cPick = GetComponent<EX_PL_Pick>();
        cLive = GetComponent<EX_PL_Live>();

        mUI.gameObject.SetActive(false);
    }

    // Need to handle if they went for a first down.
    public override void FEnter()
    {
        cPlays.mState = EX_Plays.STATE.S_RESULT;
        mUI.gameObject.SetActive(true);
        
        // here we handle the result of the last play.

        string res = "";
        switch(cLive.mResult.mChoice)
        {
            case PLAY_CHOICE.C_RUN: res += "Ran the ball for "; break;
            case PLAY_CHOICE.C_PASS: res += "Passed the ball for "; break;
            case PLAY_CHOICE.C_PUNT: res += "Punted the ball for "; break;
        }

        res += cLive.mResult.mDis + " yards";
        // now figure out the remaining down and distance.
        cPlays.mGameData.mBallLoc += cLive.mResult.mDis;
        int disToFirstDown = cPlays.mGameData.mMarkerDown - cPlays.mGameData.mBallLoc;
        cPlays.mGameData.mDown++;
        if(disToFirstDown <= 0){
            // they got a first down.
            cPlays.mGameData.mMarkerDown = cPlays.mGameData.mBallLoc + 10;
            cPlays.mGameData.mDown = GameData.DOWN.FIRST;
            disToFirstDown = 10;
        }else if(cPlays.mGameData.mDown == GameData.DOWN.LENGTH){
            // Turnover on downs.
            Debug.Log("Turnover on downs, handle later");
        }
        string downAndDis = "";
        switch(cPlays.mGameData.mDown)
        {
            case GameData.DOWN.FIRST: downAndDis+="First"; break;
            case GameData.DOWN.SECOND: downAndDis+="Second"; break;
            case GameData.DOWN.THIRD: downAndDis+="Third"; break;
            case GameData.DOWN.FOURTH: downAndDis+="Fourth"; break;
        }

        downAndDis += " and " + disToFirstDown;
        res += downAndDis;
        mUI.mTxtRes.text = "Result: " + res;

        // "Ran the ball for 7 yards. "
        // "Turnover!"
        // "Ran for Touchdown"

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
