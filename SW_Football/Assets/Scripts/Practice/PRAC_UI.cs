/*************************************************************************************
Alrighty.

You know what? Maybe we should just have the PRAC_Manager just directly telling this what's going 
on each frame.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PRAC_UI : MonoBehaviour
{
    public enum PRAC_UI_STATE
    {
        PLAY_PICKED,
        WHATEVER
    }

    public Text                     mDefensivePlayName;

    public PB_UI                    mPlaybookSCN;

    // These are actually just called once, not every frame.
    public void FRUN_Playbook()
    {
        mPlaybookSCN.gameObject.SetActive(true);

        mPlaybookSCN.DP_Plays.options.Clear();
        // Plays already loaded in.
        foreach(DATA_Play ply in IO_PlayList.mPlays)
        {
            // add something to the playbook.
            mPlaybookSCN.DP_Plays.options.Add(new Dropdown.OptionData(ply.mName));
        }

        // Set all the images.
        mPlaybookSCN.FSetUpPlaybookImagery();
    }

    public void FRUN_Presnap()
    {
        mPlaybookSCN.gameObject.SetActive(false);
    }

    public void FRUN_Playing()
    {

    }

    public void FRUN_Postsnap()
    {

    }

}
