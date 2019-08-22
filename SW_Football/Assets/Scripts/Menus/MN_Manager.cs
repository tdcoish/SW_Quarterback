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

    public GameObject               mMainScreen;
    public GameObject               mPocketPasserScreen;
    public GameObject               mPP_LOAD_SCN;
    public GameObject               mSettingsScreen;
    public GameObject               mQuitScreen;

    public AD_Manager               refAudioManager;

    void Start()
    {
        refAudioManager.OnBackToNormal();

        // true == include inactive.
        mScreens = GetComponentsInChildren<MN_Screen>(true);
    }

    private void ScreenTransition(string sScreen)
    {
        for(int i=0; i<mScreens.Length; i++)
        {
            if(mScreens[i].mName == sScreen){
                Debug.Log("Right scren");
                mScreens[i].gameObject.SetActive(true);
            }else{
                mScreens[i].gameObject.SetActive(false);
            }
        }

    }

    public void OnPressedPocketPasser()
    {
        ScreenTransition("PP");
    }

    public void OnPressedPlayPP()
    {
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
}
