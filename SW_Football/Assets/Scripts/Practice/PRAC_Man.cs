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

    public PLY_SnapSpot         rSnapSpot;
    public PRAC_Ath             PF_PlayerObj;

    public GameObject           PF_RouteNode;

    void Start()
    {
        mState = PRAC_STATE.SPICK_PLAY;
        IO_PlayList.FLOAD_PLAYS();    
    }

    void Update()
    {
        switch(mState)
        {
            case PRAC_STATE.SPICK_PLAY: RUN_PickPlay(); break;
            case PRAC_STATE.SPRE_SNAP: RUN_PreSnap(); break;
            case PRAC_STATE.SPLAY_RUNNING: RUN_PlayRunning(); break;
            case PRAC_STATE.SPOST_PLAY: RUN_PostPlay(); break;
        }
    }

    private void RUN_PickPlay()
    {
        // basically just skip over this, and pretend they selected "default".
        DATA_Play playToRun = IO_PlayList.FLOAD_PLAY_BY_NAME(mPlayName);

        if(playToRun == null){
            Debug.Log("No play of that name");
        }else{
            Debug.Log("Yes, we can run that play");
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

        mState = PRAC_STATE.SPRE_SNAP;
    }
    private void RUN_PreSnap()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PRAC_Ath[] athletes = FindObjectsOfType<PRAC_Ath>();
            for(int i=0; i<athletes.Length; i++)
            {
                athletes[i].mActive = true;
            }

            mState = PRAC_STATE.SPLAY_RUNNING;
        }
    }
    private void RUN_PlayRunning()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("SN_MN_Main");
        }
    }
    private void RUN_PostPlay()
    {

    }
}
