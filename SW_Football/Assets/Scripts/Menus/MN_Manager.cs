/*************************************************************************************
For now, it just plays some music, then you can select the one and only game mode.

Gonna experiment with an idea for "screens", just shoving a screen into a gameobject and 
seeing what happens when we switch them.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MN_Manager : MonoBehaviour
{

    public GameObject               mMainScreen;
    public GameObject               mPocketPasserScreen;
    public GameObject               mPP_LOAD_SCN;

    public AudioSource              mMusBackground;

    public void OnPressedPocketPasser()
    {
        //SceneManager.LoadScene("SN_PocketPasser");
        mMainScreen.SetActive(false);
        mPocketPasserScreen.SetActive(true);
    }

    public void OnPressedPlayPP()
    {
        mPocketPasserScreen.SetActive(false);
        mPP_LOAD_SCN.SetActive(true);
    }

    public void OnPressedMainMenu()
    {
        mMainScreen.SetActive(true);
        mPocketPasserScreen.SetActive(false);
    }

    public void OnPressedQuit()
    {
        Debug.Log("Tried to quit");
        Application.Quit();
    }
}
