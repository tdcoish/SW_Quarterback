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
            rPlayLiveCanvas.GetComponentInChildren<Text>().text = "WARNING: OUT OF POCKET";
        }else{
            rPlayLiveCanvas.GetComponentInChildren<Text>().text = "IN POCKET";
        }
    }

    public void FSetPostPlayText(string msg)
    {
        rPostPlayCanvas.GetComponentInChildren<Text>().text = msg;
    }
}
