/*************************************************************************************
When the game is over, for now just display the results screen.
*************************************************************************************/
using UnityEngine;

public class EX_Over : TDC_Component
{
    private EX_Man                          cMan;
    private EX_Plays                        cPlayMan;
    


    public UI_Over                          mUI;

    void Start()
    {
        cMan = GetComponent<EX_Man>();
        cPlayMan = GetComponent<EX_Plays>();

        mUI.gameObject.SetActive(false);
    }

    public override void FEnter()
    {
        cMan.mState = EX_Man.STATE.S_END;
        cMan.mTxtInstructions.text = "Look at stats and quit";
        mUI.gameObject.SetActive(true);

        mUI.mTxtAwayScore.text = "Away: " + cPlayMan.mGameData.mScores.mAway;
        mUI.mTxtHomeScore.text = "Home: " + cPlayMan.mGameData.mScores.mHome;

        if(cPlayMan.mGameData.mScores.mHome == cPlayMan.mGameData.mScores.mAway)
        {
            mUI.mTxtWhoWon.text = "TIE!";
        }else if(cPlayMan.mGameData.mScores.mHome > cPlayMan.mGameData.mScores.mAway){
            mUI.mTxtWhoWon.text = "HOME TEAM WINS!";
        }else{
            mUI.mTxtWhoWon.text = "AWAY TEAM WINS!";
        }
    }

    public override void FRunUpdate()
    {

    }

    public void BT_Quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SN_MN_Main");
    }
}
