/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class UI_Sim : MonoBehaviour
{
    public Text                             mTxtPossession;
    public Text                             mTxtTimeAndQuarter;
    public Text                             mTxtBallSpot;
    public Text                             mTxtDownAndDis;

    public void FSetTimeText(TDC_Time t, GameData.QUARTER q)
    {
        string time = t.mMin.ToString("D2") + ":" + t.mSec.ToString("D2");
        string quarter = "";
        switch(q){
            case GameData.QUARTER.FIRST: quarter = "1st"; break;
            case GameData.QUARTER.SECOND: quarter = "2nd"; break;
            case GameData.QUARTER.THIRD: quarter = "3rd"; break;
            case GameData.QUARTER.FOURTH: quarter = "4th"; break;
        }
        quarter += " quarter";

        mTxtTimeAndQuarter.text = time + " in " + quarter;
    }

    public void FSetBallText(GameData.FIELD_POS pos)
    {
        string sPos = "";
        if(pos.mSide == GameData.POSSESSION.HOME){
            sPos += "HOME";
        }else{
            sPos += "AWAY";
        }

        sPos += " " + pos.mYardMark;

        mTxtBallSpot.text = sPos;
    }

    public void FSetDownAndDisText(GameData.DOWN down, int disToFirstDown)
    {
        string downAndDis = "";
        switch(down)
        {
            case GameData.DOWN.FIRST: downAndDis+="First"; break;
            case GameData.DOWN.SECOND: downAndDis+="Second"; break;
            case GameData.DOWN.THIRD: downAndDis+="Third"; break;
            case GameData.DOWN.FOURTH: downAndDis+="Fourth"; break;
        }

        downAndDis += " and " + disToFirstDown;

        mTxtDownAndDis.text = downAndDis;
    }

    public void FSetPossessionText(GameData.POSSESSION pos)
    {
        mTxtPossession.text = "Possession: " + pos;
    }
}
