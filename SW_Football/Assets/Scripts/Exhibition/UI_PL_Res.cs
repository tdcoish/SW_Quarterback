/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class UI_PL_Res : MonoBehaviour
{
    public Text                                             mTxtRes;
    public Text                                             mPlayInfo;  

    public void FSetResultText(PLAY_CHOICE choice, bool turnover, int dis, bool touchdown)
    {
        string res = "";
        switch(choice)
        {
            case PLAY_CHOICE.C_RUN: res += "Ran the ball for "; break;
            case PLAY_CHOICE.C_PASS: res += "Passed the ball for "; break;
            case PLAY_CHOICE.C_PUNT: res += "Punted the ball for "; break;
        }

        res += dis + " yards";
        // kind of a hack, turnovers overwrite everything we just did.
        if(turnover){
            // Turnover on downs.
            Debug.Log("Turnover, either downs or ingame, handle later");
            res = "Turnover, either downs or ingame";
        }
        if(touchdown){
            res = "TOUCHDOWN!";
        }
        mTxtRes.text = res;
    }
}
