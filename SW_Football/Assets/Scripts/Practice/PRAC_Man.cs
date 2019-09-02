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

[RequireComponent(typeof(PRAC_SetUpPlay))]
public class PRAC_Man : MonoBehaviour
{
    private PRAC_SetUpPlay      cPlaySetter;

    private PRAC_STATE          mState;
    public string               mPlayName = "Default";

    public PRAC_UI              rPracUI;

    public PLY_SnapSpot         rSnapSpot;

    void Start()
    {
        cPlaySetter = GetComponent<PRAC_SetUpPlay>();

        mState = PRAC_STATE.SPOST_PLAY;
        IO_PlayList.FLOAD_PLAYS(); 
        IO_DefPlays.FLOAD_PLAYS();   
        IO_ZoneList.FLOAD_ZONES();

        IO_RouteList.FLOAD_ROUTES();
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

    }

    public void FPlayPicked()
    {
        
        // We wait until they click a play in the UI.
        string sPlayName = rPracUI.mPlaybookSCN.DP_Plays.options[rPracUI.mPlaybookSCN.DP_Plays.value].text;
        cPlaySetter.FSetUpPlay(sPlayName, "", rSnapSpot);

        mState = PRAC_STATE.SPRE_SNAP;

        rPracUI.FRUN_Presnap();
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;
    }

    public void FDefPlayPicked()
    {

    }

    private void RUN_PreSnap()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PRAC_Ath[] athletes = FindObjectsOfType<PRAC_Ath>();
            for(int i=0; i<athletes.Length; i++)
            {
                athletes[i].mState = PRAC_Ath.PRAC_ATH_STATE.SDOING_JOB;
            }

            mState = PRAC_STATE.SPLAY_RUNNING;
        }
    }
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
