/*************************************************************************************
Kind of a gamble because you can always use this. I might have to integrate this more with
a manager.

Might have to store the state the PC was in when we paused.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_PauseMenu : MonoBehaviour
{
    public Canvas                   mPauseSCN;

    void Start()
    {
        mPauseSCN = GetComponentInChildren<Canvas>();
        mPauseSCN.gameObject.SetActive(false);
    }

    void Update()
    {
        // if the user presses m, then they bring up the pause menu.
        if(Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0f;
            mPauseSCN.gameObject.SetActive(true);

            // have to show the mouse, as well as disable the player camera.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SINACTIVE;

            // GE_PauseMenuOpened.Raise(null);
        }
    }

    public void BT_Resume()
    {
        mPauseSCN.gameObject.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;
    }

    // Huh. This might be hard to implement.
    public void BT_Quit()
    {
        SceneManager.LoadScene("SN_MN_Main");
    }

    //     // Since we're displaying the scoreboard screen, this is still fine
    // public void OnQuitPressed()
    // {
    //     refUI.gameObject.SetActive(false);
    //     MN_PauseScreen.SetActive(false);
    //     refScoreboardUI.SetActive(false);
    //     refQuitUI.SetActive(true);
    // }
    // public void OnResumePressed()
    // {
    //     MN_PauseScreen.SetActive(false);
    //     Time.timeScale = 1f;

    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;

    //     FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;

    //     GE_PauseMenuClosed.Raise(null);
    // }
}
