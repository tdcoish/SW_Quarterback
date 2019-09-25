/*************************************************************************************
New manager for the practice offense scene.

Alright, I can now pick the play.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class PRAC_Off_Man : MonoBehaviour
{
    private PRAC_Off_SetupPlayers               cPlayerSetup;

    public PRAC_STATE                           mState;
    public string                               mPlay = "Sail";

    public PLY_SnapSpot                         rSnapSpot;

    public GameObject                           UI_PauseScreen;
    public PRAC_PB_UI                           UI_PlayPicker;

    void Start()
    {
        IO_Settings.FLOAD_SETTINGS();

        cPlayerSetup = GetComponent<PRAC_Off_SetupPlayers>();    

        ENTER_PickPlay();
    }

    void Update()
    {
        switch(mState){
            case PRAC_STATE.SPICK_PLAY: RUN_PickPlay(); break;
            case PRAC_STATE.SPLAY_PICKED: RUN_PlayPicked(); break;
            case PRAC_STATE.SPRE_SNAP: RUN_PreSnap(); break;
            case PRAC_STATE.SPLAY_RUNNING: RUN_Live(); break;
            case PRAC_STATE.SPOST_PLAY: RUN_PostPlay(); break;
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

            //GE_PauseMenuOpened.Raise(null);
        }
    }

    void ENTER_PickPlay(){
        mState = PRAC_STATE.SPICK_PLAY;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UI_PlayPicker.gameObject.SetActive(true);
        UI_PlayPicker.FSetUpPlaybookImagery();
    }
    void RUN_PickPlay()
    {
        UI_PlayPicker.FRunUpdate();
    }
    void EXIT_PickPlay(){
        Cursor.lockState = CursorLockMode.Locked;
        UI_PlayPicker.gameObject.SetActive(false);
        Cursor.visible = false;
    }
    void ENTER_PlayPicked(){
        mState = PRAC_STATE.SPLAY_PICKED;
    }
    void RUN_PlayPicked(){
        ENTER_PreSnap();
    }

    void ENTER_PreSnap(){
        mState = PRAC_STATE.SPRE_SNAP;
        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            a.mState = PRAC_Ath.PRAC_ATH_STATE.SPRE_SNAP;
        }
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SPRE_SNAP;

    }
    // Fill in the high camera stuff later.
    void RUN_PreSnap()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EXIT_PreSnap();
            ENTER_Live();
        }
    }
    void EXIT_PreSnap(){

    }
    void ENTER_Live(){
        mState = PRAC_STATE.SPLAY_RUNNING;
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;
        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            a.mState = PRAC_Ath.PRAC_ATH_STATE.SDOING_JOB;
        }
    }
    private void RUN_Live()
    {
        // Debug.Log("Running");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Now we just repeat the whole shebang.
            ENTER_PostSnap();
            return;
        }
    }

    void EXIT_Live(){

    }
    void ENTER_PostSnap(){
        mState = PRAC_STATE.SPOST_PLAY;

        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            Destroy(a.gameObject);
        }
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SINACTIVE;

        ENTER_PickPlay();

    }
    void RUN_PostPlay(){}


    public void FOffPlayPicked(string name)
    {
        if(mState != PRAC_STATE.SPICK_PLAY){
            Debug.Log("ERROR. Picked play from wrong state");
            return;
        }
        mPlay = name;
        cPlayerSetup.FSetUpPlayers(mPlay, rSnapSpot);

        EXIT_PickPlay();
        ENTER_PlayPicked();
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
