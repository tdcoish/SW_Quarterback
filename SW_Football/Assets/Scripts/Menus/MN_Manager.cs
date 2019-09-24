/*************************************************************************************
For now, it just plays some music, then you can select the one and only game mode.

Gonna experiment with an idea for "screens", just shoving a screen into a gameobject and 
seeing what happens when we switch them.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MN_Manager : MonoBehaviour
{
    // Unity cannot serialize dictionaries. Shame.
    public MN_Screen[]              mScreens;

    public MN_Transition            PF_Transition;

    public AD_Manager               refAudioManager;

    void Start()
    {
        IO_Settings.FLOAD_SETTINGS();
        
        refAudioManager.OnBackToNormal();

        // true == include inactive.
        mScreens = GetComponentsInChildren<MN_Screen>(true);
    }

    private void ScreenTransition(string sScreen)
    {
        MN_Transition transition = Instantiate(PF_Transition);
        transition.transform.SetParent(transform);

        for(int i=0; i<mScreens.Length; i++)
        {
            if(mScreens[i].mName == sScreen){
                mScreens[i].gameObject.SetActive(true);
            }else{
                mScreens[i].gameObject.SetActive(false);
            }
        }

    }

    public void OnPressedEnterMainMenu()
    {
        ScreenTransition("Main");
    }

    public void BT_Play()
    {
        ScreenTransition("Play_LOAD");
    }

    public void OnPressedPocketPasser()
    {
        ScreenTransition("PP_Dif");
    }

    public void BT_RoutePasser()
    {
        ScreenTransition("RP_Dif");
    }

    // Called by that script, not us.
    public void BT_RoutePasserReady()
    {
        Debug.Log("Difficulty: " + IO_RP_Dif.mDifSelected);
        ScreenTransition("RP_LOAD");
    }

    public void BT_PocketPasserReady()
    {
        Debug.Log("Difficulty: " + IO_PP_Dif.mDif);
        ScreenTransition("PP_Load");
    }

    public void OnPressedMainMenu()
    {
        ScreenTransition("Main");
    }

    public void OnPressedQuit()
    {
        ScreenTransition("Quit");
    }

    public void OnPressedSettings()
    {
        ScreenTransition("Settings");
    }

    public void BT_Practice()
    {
        SceneManager.LoadScene("SN_Practice");
    }

    public void BT_DefensivePlays()
    {
        SceneManager.LoadScene("ED_DefPlayCreate");
    }

    public void OnPlayEditorPressed()
    {
        SceneManager.LoadScene("ED_Plays2");
    }

    public void BT_TrophyRoom()
    {
        SceneManager.LoadScene("SN_TrophyRoom");
    }

    public void BT_Formation(){
        SceneManager.LoadScene("ED_Formation");
    }
    public void BT_OffensivePlays(){
        SceneManager.LoadScene("ED_OffPlay");
    }
    public void BT_OffensePractice(){
        SceneManager.LoadScene("SN_PracOffense");
    }
}
