/*************************************************************************************
Practice manager. This is the thing that does all the logic to run practice.

1) Load in a whole bunch of offensive players, and give them the roles here.
2) Snap ball
3) Have them do the thing.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PRAC_STATE
{
    SPICK_PLAY,
    SPRE_SNAP,
    SPLAY_RUNNING,
    SPOST_PLAY
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
    public string               mPlayName = "Default";

    public PRAC_UI              rPracUI;

    public PLY_SnapSpot         rSnapSpot;

    void Start()
    {
        cPlaySetter = GetComponent<PRAC_SetUpPlay>();
        cShowDefence = GetComponent<PRAC_ShowDefense>();

        mState = PRAC_STATE.SPOST_PLAY;
        mPreSnapState = PRESNAP_STATE.SREADYTOSNAP;
        IO_PlayList.FLOAD_PLAYS(); 
        IO_DefPlays.FLOAD_PLAYS();   
        IO_ZoneList.FLOAD_ZONES();

        IO_RouteList.FLOAD_ROUTES();

        PRS_AssignMan.FLOAD_PRIORITIES();
        // IO_RouteList.FCONVERT_TO_TEXT_FILES();
        // IO_PlayList.FCONVERT_TO_TEXT_FILES();
        // IO_DefPlays.FCONVERT_TO_TEXT_FILES();
        // IO_ZoneList.FCONVERT_TO_TEXT_FILES();
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

        // basically don't do anything until they click a play.
        // UI has to run it's updating though.
        rPracUI.mPlaybookSCN.FRunUpdate();

    }

    public void FPlayPicked(string sOffPlayName)
    {
        
        // We wait until they click a play in the UI.
        // string sPlayName = rPracUI.mPlaybookSCN.DP_Plays.options[rPracUI.mPlaybookSCN.DP_Plays.value].text;
        // cPlaySetter.FSetUpPlay(sPlayName, "", rSnapSpot);
        cPlaySetter.FSetUpPlay(sOffPlayName, "", rSnapSpot);

        mState = PRAC_STATE.SPRE_SNAP;
        mPreSnapState = PRESNAP_STATE.SREADYTOSNAP;

        rPracUI.FRUN_Presnap();
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SPRE_SNAP;
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
        rPracUI.FRUN_Playbook();
        mState = PRAC_STATE.SPICK_PLAY;
    }
}
