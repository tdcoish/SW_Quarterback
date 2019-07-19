/*************************************************************************************
Game Management controls everything about the play on the field, minus the actual AI. 
So, place the ball, bring up the play selection, snap the ball, figure out when the play
is over, and do it again. Halftime, end of game, etcetera. Basically the metagame.

For now, just figure out when the play is over and whether it was successful or not.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class GM_Manager : MonoBehaviour
{
    public bool                 mPlayOngoing = true;

    [SerializeField]
    private Text                rPlayRes;

    private bool                mBallSnapped = false;
    public GE_Event             GE_BALL_SNAP;

    void Start()
    {
        PlayRestart();
    }

    void Update()
    {
        if(!mBallSnapped){
            if(Input.GetKeyDown(KeyCode.Space))
            {
                GE_BALL_SNAP.Raise(null);
                mBallSnapped = true;
            }

        }
    }

    public void OnIncompletion()
    {
        if(mPlayOngoing){
            rPlayRes.text = "Play Res: Incompletion";
            PlayOver();
        }
    }

    public void OnIntercept()
    {
        if(mPlayOngoing){
            rPlayRes.text = "Play Res: Interception";
            PlayOver();
        }
    }

    public void OnCatch()
    {
        if(mPlayOngoing){
            rPlayRes.text = "Play Res: Catch";
            PlayOver();
        }
    }

    private void PlayOver()
    {
        mPlayOngoing = false;
    }

    public void PlayRestart()
    {
        mPlayOngoing = true;
        rPlayRes.text = "Play Res: Ongoing";
    }
}
