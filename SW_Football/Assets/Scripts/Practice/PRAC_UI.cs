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

    public Text                     mOffensivePlayName;
    public Text                     mDefensivePlayName;

    public PB_UI                    mPlaybookSCN;
    public PB_Def                   mDefPBSCN;

    // These are actually just called once, not every frame.
    public void FRUN_OffPlaybook()
    {
        mPlaybookSCN.gameObject.SetActive(true);

        // Set all the images.
        mPlaybookSCN.FSetUpPlaybookImagery();
        // mDefPBSCN.FSetUpPlaybookImagery();
    }

    public void FRUN_DefPlaybook()
    {
        mPlaybookSCN.gameObject.SetActive(false);
        mDefPBSCN.gameObject.SetActive(true);

        // Set all the images.
        mDefPBSCN.FSetUpPlaybookImagery();
    }

    public void FRUN_Presnap()
    {
        mPlaybookSCN.gameObject.SetActive(false);
        mDefPBSCN.gameObject.SetActive(false);
    }

    public void FRUN_Playing()
    {

    }

    public void FRUN_Postsnap()
    {

    }

}
