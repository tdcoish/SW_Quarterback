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

    void Start()
    {
        PlayRestart();
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
