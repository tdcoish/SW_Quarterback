/*************************************************************************************
Scoreboard that is displayed after the game is over.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PP_Scoreboard : MonoBehaviour
{
    public Text             mScoreTxt;
    public SO_Int           lScore;

    private void Awake()
    {
        mScoreTxt.text = "SCORE: " + lScore.Val;
    }

    // also, we're gonna have a camera that goes around in a circle, although maybe not in this script.

}
