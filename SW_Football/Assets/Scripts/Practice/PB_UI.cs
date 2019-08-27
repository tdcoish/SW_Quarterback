/*************************************************************************************
User interface of the play selection screen.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PB_UI : MonoBehaviour
{
    public Dropdown             DP_Plays;

    public void BT_PlaySelected()
    {
        // Do something when they select a play.
        Debug.Log("Play selected: " + DP_Plays.options[DP_Plays.value].text);
    }
}
