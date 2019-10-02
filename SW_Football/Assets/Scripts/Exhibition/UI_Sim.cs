/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class UI_Sim : MonoBehaviour
{
    public Text                             mTxtTimeAndQuarter;
    public Text                             mTxtBallSpot;
    public Text                             mTxtDownAndDis;

    public void FSetTimeText(TDC_Time t, GameData.QUARTER q)
    {
        string time = t.mMin + ":" + t.mSec;
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
}
