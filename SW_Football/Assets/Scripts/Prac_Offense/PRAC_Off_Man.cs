/*************************************************************************************
New manager for the practice offense scene.

Alright, I can now pick the play.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public struct PRAC_PLAY_RES
{
    public bool                         mBallCaught;
    public bool                         mInt;
}

public class PRAC_Off_Man : MonoBehaviour
{
    private PROFST_Live                         cLive;
    private PROFST_Pick                         cPick;
    private PROFST_Pre                          cPre;
    private PROFST_Post                         cPost;

    [HideInInspector]
    public AD_Prac                              cAud;

    public PRAC_STATE                           mState;
    public string                               mPlay = "Sail";

    public PLY_SnapSpot                         rSnapSpot;

    public GameObject                           UI_PauseScreen;

    public PRAC_PLAY_RES                        mRes;

    public bool                                 mLineExists = false;
    public bool                                 mDefenseExists = true;

    void Awake()
    {
        IO_Settings.FLOAD_SETTINGS();
        IO_DefPlays.FLOAD_PLAYS();
        IO_ZoneList.FLOAD_ZONES();
    }
    void Start()
    {
        cPick = GetComponent<PROFST_Pick>();
        cPost = GetComponent<PROFST_Post>();
        cLive = GetComponent<PROFST_Live>();
        cPre = GetComponent<PROFST_Pre>();

        cAud = GetComponentInChildren<AD_Prac>();

        cPick.FEnter();
    }

    void Update()
    {
        switch(mState){
            case PRAC_STATE.SPICK_PLAY: cPick.FRun(); break;
            case PRAC_STATE.SPRE_SNAP: cPre.FRun(); break;
            case PRAC_STATE.SPLAY_RUNNING: cLive.FRun(); break;
            case PRAC_STATE.SPOST_PLAY: cPost.FRun(); break;
        }

        // if the user presses m, then they bring up the pause menu.
        if(Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 0f;
            UI_PauseScreen.SetActive(true);

            // have to show the mouse, as well as disable the player camera.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SINACTIVE;
        }
    }

    public void OnResumePressed()
    {
        UI_PauseScreen.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;

        // GE_PauseMenuClosed.Raise(null);
    }
    // Since we're displaying the scoreboard screen, this is still fine
    public void OnQuitPressed()
    {
        // refUI.gameObject.SetActive(false);
        // MN_PauseScreen.SetActive(false);
        // refScoreboardUI.SetActive(false);
        // refQuitUI.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("SN_MN_Main");        
    }
}
