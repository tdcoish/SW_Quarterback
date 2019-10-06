/*************************************************************************************
New manager for the practice offense scene.

Alright, I can now pick the play.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public struct PRAC_PLAY_RES
{
    public bool                         mBallCaught;
}

public class PRAC_Off_Man : MonoBehaviour
{
    private PRAC_Off_SetupPlayers               cPlayerSetup;
    private PRAC_Off_ShowGFX                    cShowPreSnapGFX;

    public PRAC_STATE                           mState;
    public PRESNAP_STATE                        mPreSnapState;
    public string                               mPlay = "Sail";

    public PLY_SnapSpot                         rSnapSpot;

    public GameObject                           UI_PauseScreen;
    public PRAC_PB_UI                           UI_PlayPicker;
    public UI_PostPlay                          UI_PostPlay;

    public PRAC_PLAY_RES                        mRes;

    private float                               mTime;
    private bool                                mLineExists = true;

    void Start()
    {
        IO_Settings.FLOAD_SETTINGS();

        cPlayerSetup = GetComponent<PRAC_Off_SetupPlayers>();   
        cShowPreSnapGFX = GetComponent<PRAC_Off_ShowGFX>(); 

        TDC_EventManager.FAddHandler(TDC_GE.GE_BallCaught, E_ReceiverCatchesBall);
        TDC_EventManager.FAddHandler(TDC_GE.GE_BallHitGround, E_BallHitsGround);

        UI_PostPlay.gameObject.SetActive(false);

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
        }
    }

    void ENTER_PickPlay(){
        mState = PRAC_STATE.SPICK_PLAY;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UI_PlayPicker.gameObject.SetActive(true);
        UI_PlayPicker.FSetUpPlaybookImagery();
        UI_PlayPicker.FSetLineEnabledText(mLineExists);
    }
    void RUN_PickPlay()
    {
        UI_PlayPicker.FRunUpdate();
    }
    void EXIT_PickPlay(){
        Cursor.lockState = CursorLockMode.Locked;
        UI_PlayPicker.gameObject.SetActive(false);
        Cursor.visible = false;

        if(!mLineExists){
            Debug.Log("Here");
            PRAC_Off_Ply[] aths = FindObjectsOfType<PRAC_Off_Ply>();
            foreach(PRAC_Off_Ply a in aths){
                if(a.mRole == "BLOCK"){
                    Debug.Log("Here");
                    Destroy(a.gameObject);
                }else{
                    Debug.Log(a.mJob.mRole);
                }
            }
        }
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

        cShowPreSnapGFX.FShowOffensivePlay(IO_OffensivePlays.FLoadPlay(mPlay), rSnapSpot);
    }
    // Fill in the high camera stuff later.
    void RUN_PreSnap()
    {
        switch(mPreSnapState)
        {
            case PRESNAP_STATE.SREADYTOSNAP: RUN_SnapReady(); break;
            case PRESNAP_STATE.SHIGHCAM: RUN_HighCam(); break;
        }
    }
    void RUN_SnapReady(){
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EXIT_PreSnap();
            ENTER_Live();
        }

        // We also need the camera to go to the higher perspective.
        if(Input.GetKeyDown(KeyCode.T))
        {
            cShowPreSnapGFX.FStopShowingPlayArt();
            cShowPreSnapGFX.FShowOffensivePlay(IO_OffensivePlays.FLoadPlay(mPlay), rSnapSpot);
            FindObjectOfType<CAM_PlayShowing>().FActivate();
            mPreSnapState = PRESNAP_STATE.SHIGHCAM;
        }
    }
    void RUN_HighCam(){
        if(Input.GetKeyDown(KeyCode.T))
        {
            cShowPreSnapGFX.FStopShowingPlayArt();
            FindObjectOfType<CAM_PlayShowing>().FDeactivate();
            mPreSnapState = PRESNAP_STATE.SREADYTOSNAP;
        }
    }
    void EXIT_PreSnap(){
        cShowPreSnapGFX.FStopShowingPlayArt();
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
            ENTER_PostPlay();
            return;
        }
    }

    void EXIT_Live(){

    }
    void ENTER_PostPlay(){
        mState = PRAC_STATE.SPOST_PLAY;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UI_PostPlay.gameObject.SetActive(true);
        if(mRes.mBallCaught){
            UI_PostPlay.mTxtResult.text = "Caught the ball";
        }else{
            UI_PostPlay.mTxtResult.text = "No catch";
        }
        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            a.mState = PRAC_Ath.PRAC_ATH_STATE.SPOST_PLAY;
        }

        mTime = Time.time;
    }
    void RUN_PostPlay(){
        if(Time.time - mTime > 3f){
            EXIT_PostPlay();
            ENTER_PickPlay();
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            EXIT_PostPlay();
            ENTER_PickPlay();
        }
    }
    void EXIT_PostPlay(){
        UI_PostPlay.gameObject.SetActive(false);
        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            Destroy(a.gameObject);
        }
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SINACTIVE;

        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }

        mRes.mBallCaught = false;
    }

    public void BT_NextPlay()
    {
        EXIT_PostPlay();
        ENTER_PickPlay();
    }

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

    public void BT_ToggleLineEnabled()
    {
        mLineExists = !mLineExists;
        UI_PlayPicker.FSetLineEnabledText(mLineExists);
    }

    public void E_BallHitsGround()
    {
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }

        if(mState == PRAC_STATE.SPLAY_RUNNING){
            ENTER_PostPlay();
        }
    }

    public void E_ReceiverCatchesBall()
    {
        Debug.Log("Receiver Caught ball!");
        mRes.mBallCaught = true;
        ENTER_PostPlay();
    }
}
