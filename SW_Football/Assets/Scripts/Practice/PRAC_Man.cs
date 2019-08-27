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

public class PRAC_Man : MonoBehaviour
{
    private PRAC_STATE          mState;
    public string               mPlayName = "Default";

    public PRAC_UI              rPracUI;

    public PLY_SnapSpot         rSnapSpot;
    public PRAC_Ath             PF_PlayerObj;

    public GameObject           PF_RouteNode;

    void Start()
    {
        mState = PRAC_STATE.SPOST_PLAY;
        IO_PlayList.FLOAD_PLAYS();    
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
        DATA_Play playToRun = IO_PlayList.FLOAD_PLAY_BY_NAME(sPlayName);

        if(playToRun == null){
            Debug.Log("No play of that name");
        }

        // Now we just pretend there were no issues, so we put our players in their spots.
        for(int i=0; i<playToRun.mPlayerRoles.Length; i++)
        {
            Vector3 vPlayerSpot = new Vector3();
            vPlayerSpot.x = playToRun.mPlayerRoles[i].mStart.x;
            vPlayerSpot.z = playToRun.mPlayerRoles[i].mStart.y;
            vPlayerSpot += rSnapSpot.transform.position;
            var clone = Instantiate(PF_PlayerObj, vPlayerSpot, transform.rotation);
            clone.mJob.mTag = playToRun.mPlayerRoles[i].mTag;
            clone.mJob.mRole = playToRun.mPlayerRoles[i].mRole;
            clone.mJob.mDetail = playToRun.mPlayerRoles[i].mDetail;
        }

        // now, for all of the players, if their roles are route running, then load in their routes.
        IO_RouteList.FLOAD_ROUTES();
        PRAC_Ath[] athletes = FindObjectsOfType<PRAC_Ath>();
        for(int i=0; i<athletes.Length; i++)
        {
            athletes[i].mState = PRAC_Ath.PRAC_ATH_STATE.SPRE_SNAP;
            if(athletes[i].mJob.mRole == "Route")
            {
                DATA_Route rt = IO_RouteList.FLOAD_ROUTE_BY_NAME(athletes[i].mJob.mDetail);
                foreach (Vector2 routeSpot in rt.mSpots)
                {
                    Vector3 vSpot = athletes[i].transform.position;
                    vSpot.x += routeSpot.x; vSpot.z += routeSpot.y;
                    // Also, shove the spot into the receiver, just for now.
                    athletes[i].mRouteSpots.Add(vSpot);
                    Instantiate(PF_RouteNode, vSpot, transform.rotation);
                }
            }
        }

        // now we also have to shove the PC_Controller into the spot the QB is, and remove the QB athlete.
        for(int i=0; i<athletes.Length; i++)
        {
            if(athletes[i].mJob.mTag == "QB")
            {
                Vector3 vSpot = athletes[i].transform.position;
                vSpot.y = 1f;
                FindObjectOfType<PC_Controller>().transform.position = vSpot;
            }
        }

        mState = PRAC_STATE.SPRE_SNAP;

        rPracUI.FRUN_Presnap();
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SACTIVE;
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
