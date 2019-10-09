/*************************************************************************************
Just for the offensive practice.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class UI_PostPlay : MonoBehaviour
{
    public Text                                     mTxtResult;

    public void FSetPostPlayText(PRAC_PLAY_RES res)
    {
        if(res.mInt){
            mTxtResult.text = "INTERCEPTION!";
            return;
        }
        if(res.mBallCaught){
            mTxtResult.text = "Caught the ball";
        }else{
            mTxtResult.text = "No catch";
        }
    }
}
