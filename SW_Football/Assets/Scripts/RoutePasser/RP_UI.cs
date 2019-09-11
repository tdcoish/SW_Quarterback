/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class RP_UI : MonoBehaviour
{

    public Canvas               rIntroCanvas;
    public Canvas               rPreSnapCanvas;
    public Canvas               rPlayLiveCanvas;
    public Canvas               rPostPlayCanvas;
    public Canvas               rOutroCanvas;

    public Canvas               rScoreCanvas;
    public Canvas               rPauseMenu;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void FSetScoreText(int score)
    {
        rScoreCanvas.GetComponentInChildren<Text>().text = "SCORE: " + score;
    }

    public void FSetPocketText(bool inPocket)
    {
        if(!inPocket)
        {
            rPlayLiveCanvas.GetComponent<UI_PlayLive>().tPocket.text = "WARNING: OUT OF POCKET";
        }else{
            rPlayLiveCanvas.GetComponent<UI_PlayLive>().tPocket.text = "IN POCKET";
        }
    }

    public void FSetTimerText(float tm, bool ballThrown)
    {
        if(!ballThrown){
            rPlayLiveCanvas.GetComponent<UI_PlayLive>().tTime.text = "Throw Time: " + System.Math.Round(tm, 1);
        }else{
            rPlayLiveCanvas.GetComponent<UI_PlayLive>().tTime.text = "Ball Thrown!";
        }
    }

    public void FSetCombosDoneText(int numDone, int numTotal)
    {
        rPlayLiveCanvas.GetComponent<UI_PlayLive>().tCombosDone.text = "Done: " + numDone + "/" + numTotal;
    }

    public void FSetPostPlayText(string msg)
    {
        rPostPlayCanvas.GetComponentInChildren<Text>().text = msg;
    }

    public void FSetOutroScoreText(int score)
    {
        rOutroCanvas.GetComponentInChildren<Text>().text = "SCORE: " + score;
    }
}
