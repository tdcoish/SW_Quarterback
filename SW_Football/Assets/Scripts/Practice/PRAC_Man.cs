/*************************************************************************************
Practice manager. This is the thing that does all the logic to run practice.

1) Load in a whole bunch of offensive players, and give them the roles here.
2) Snap ball
3) Have them do the thing.

Sigh. Starting to need to actually make some state machines now.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PRAC_STATE
{
    SPICK_PLAY,
    SPLAY_PICKED,           // kind of a hack so we get a frame after spawning players to not have to immediately get the references.
    SPRE_SNAP,
    SPLAY_RUNNING,
    SPOST_PLAY
}

public enum PICKPLAY_STATE
{
    SOFFENSE,
    SDEFENSE,
    SBOTH_PICKED
}

public enum PRESNAP_STATE
{
    SREADYTOSNAP,
    SHIGHCAM
}

[RequireComponent(typeof(PRAC_SetUpPlay))]
public class PRAC_Man : MonoBehaviour
{
    private PRAC_SetUpPlay      cPlaySetter;
    private PRAC_ShowDefense    cShowDefence;

    private PRAC_STATE          mState;
    private PRESNAP_STATE       mPreSnapState;
    private PICKPLAY_STATE      mPickPlayState;
    public string               mPlayName = "Default";

    public GameObject           MN_PauseScreen;
    public PRAC_UI              rPracUI;

    public PLY_SnapSpot         rSnapSpot;

    void Start()
    {
        cPlaySetter = GetComponent<PRAC_SetUpPlay>();
        cShowDefence = GetComponent<PRAC_ShowDefense>();

        mState = PRAC_STATE.SPOST_PLAY;
        mPreSnapState = PRESNAP_STATE.SREADYTOSNAP;
        mPickPlayState = PICKPLAY_STATE.SOFFENSE;
        IO_PlayList.FLOAD_PLAYS(); 
        IO_DefPlays.FLOAD_PLAYS();   
        IO_ZoneList.FLOAD_ZONES();

        IO_RouteList.FLOAD_ROUTES();

        PRS_AssignMan.FLOAD_PRIORITIES();

        // Won't affect the build, but will affect the editor.
        IO_Settings.FLOAD_SETTINGS();
        // IO_RouteList.FCONVERT_TO_TEXT_FILES();
        // IO_PlayList.FCONVERT_TO_TEXT_FILES();
        // IO_DefPlays.FCONVERT_TO_TEXT_FILES();
        // IO_ZoneList.FCONVERT_TO_TEXT_FILES();
        MN_PauseScreen.SetActive(false);
    }

    void Update()
    {
        // Setting the mouse stuff 
        if(mState == PRAC_STATE.SPICK_PLAY || mState == PRAC_STATE.SPOST_PLAY)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        switch(mState)
        {
            case PRAC_STATE.SPICK_PLAY: RUN_PickPlay(); break;
            case PRAC_STATE.SPRE_SNAP: RUN_PreSnap(); break;
            case PRAC_STATE.SPLAY_RUNNING: RUN_PlayRunning(); break;
            case PRAC_STATE.SPOST_PLAY: RUN_PostPlay(); break;
        }
    }

    // Okay, here we're going to actually load in all the plays, then display them in a dropdown menu.
    private void RUN_PickPlay()
    {

        switch(mPickPlayState)
        {
            case PICKPLAY_STATE.SOFFENSE: RUN_PickOffensivePlay(); break;
            case PICKPLAY_STATE.SDEFENSE: RUN_PickDefensivePlay(); break;
            case PICKPLAY_STATE.SBOTH_PICKED: RUN_FinishedPicking(); break;
        }
    }

    private void RUN_PickOffensivePlay()
    {
        rPracUI.mPlaybookSCN.FRunUpdate();
    }
    private void RUN_PickDefensivePlay()
    {
        rPracUI.mDefPBSCN.FRunUpdate();
    }
    private void RUN_FinishedPicking()
    {
        // I guess this is where I transfer state?
        mState = PRAC_STATE.SPRE_SNAP;
        mPreSnapState = PRESNAP_STATE.SREADYTOSNAP;

        rPracUI.FRUN_Presnap();
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SPRE_SNAP;
            
        // We wait until they click a play in the UI.
        cPlaySetter.FSetUpPlay(rPracUI.mOffensivePlayName.text, rPracUI.mDefensivePlayName.text, rSnapSpot);
    }

    public void FOffPlayPicked(string sOffPlayName)
    {
        mPickPlayState = PICKPLAY_STATE.SDEFENSE;
        rPracUI.FRUN_DefPlaybook();
    }

    public void FDefPlayPicked(string sOffPlayName)
    {
        mPickPlayState = PICKPLAY_STATE.SBOTH_PICKED;
    }

    public void FDefPlayPicked()
    {

    }

    private void RUN_PreSnap()
    {
        switch(mPreSnapState)
        {
            case PRESNAP_STATE.SREADYTOSNAP: RUN_SnapReady(); break;
            case PRESNAP_STATE.SHIGHCAM: RUN_HighCam(); break;
        }

    }

    // ----------------------------- PRESNAP STATES
    private void RUN_SnapReady()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;

            PRAC_Ath[] athletes = FindObjectsOfType<PRAC_Ath>();
            for(int i=0; i<athletes.Length; i++)
            {
                athletes[i].mState = PRAC_Ath.PRAC_ATH_STATE.SDOING_JOB;
            }

            // For now so it's not there when we run. Going to change this soon.
            cShowDefence.FStopShowingPlayArt();
            mState = PRAC_STATE.SPLAY_RUNNING;
        }

        // We also need the camera to go to the higher perspective.
        if(Input.GetKeyDown(KeyCode.T))
        {
            cShowDefence.FStopShowingPlayArt();
            cShowDefence.FShowAllPlayRoles(IO_PlayList.FLOAD_PLAY_BY_NAME(rPracUI.mOffensivePlayName.text), IO_DefPlays.FLOAD_PLAY_BY_NAME(rPracUI.mDefensivePlayName.text), rSnapSpot);
            FindObjectOfType<CAM_PlayShowing>().FActivate();
            mPreSnapState = PRESNAP_STATE.SHIGHCAM;
        }
    }
    private void RUN_HighCam()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            cShowDefence.FStopShowingPlayArt();
            FindObjectOfType<CAM_PlayShowing>().FDeactivate();
            mPreSnapState = PRESNAP_STATE.SREADYTOSNAP;
        }
    }

    // -----------------------------

    private void RUN_PlayRunning()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Now we just repeat the whole shebang.
            mState = PRAC_STATE.SPOST_PLAY;
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("SN_MN_Main");
        }
    }
    // For now, delete everything from the scene, and that's it.
    private void RUN_PostPlay()
    {
        PRAC_Ath[] athletes = FindObjectsOfType<PRAC_Ath>();
        for(int i=0; i<athletes.Length; i++)
        {
            Destroy(athletes[i].gameObject);
        }
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SINACTIVE;
        rPracUI.FRUN_OffPlaybook();
        mState = PRAC_STATE.SPICK_PLAY;
        mPickPlayState = PICKPLAY_STATE.SOFFENSE;
    }
}
