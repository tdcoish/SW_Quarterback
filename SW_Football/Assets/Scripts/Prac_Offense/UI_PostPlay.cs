/*************************************************************************************
Just for the offensive practice.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class UI_PostPlay : MonoBehaviour
{
    public Text                                     mTxtResult;

    public void FSetPostPlayText(PRAC_PlayInfo info)
    {
        if(info.mWasCatch){
            if(info.mWasTackled){
                mTxtResult.text = "Catch for " + System.Math.Round(info.mYardsGained, 0) + " yards";
            }else{
                mTxtResult.text = "Catch, and he's still running";
            }
        }else if(info.mWasIncompletion){
            mTxtResult.text = "Incompletion";
        }else if(info.mWasInterception){
            mTxtResult.text = "INTERCEPTION!";
        }
    }
}
