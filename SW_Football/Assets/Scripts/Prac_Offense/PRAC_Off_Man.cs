/*************************************************************************************
New manager for the practice offense scene.
*************************************************************************************/
using UnityEngine;

public class PRAC_Off_Man : MonoBehaviour
{
    private PRAC_Off_SetupPlayers               cPlayerSetup;

    public PRAC_STATE                           mState;
    public string                               mPlay = "Sail";

    public PLY_SnapSpot                         rSnapSpot;

    void Start()
    {
        IO_Settings.FLOAD_SETTINGS();

        cPlayerSetup = GetComponent<PRAC_Off_SetupPlayers>();    
        cPlayerSetup.FSetUpPlayers(mPlay, rSnapSpot);

        ENTER_PreSnap();
    }

    void Update()
    {
        switch(mState){
            case PRAC_STATE.SPICK_PLAY: RUN_PickPlay(); break;
            case PRAC_STATE.SPRE_SNAP: RUN_PreSnap(); break;
            case PRAC_STATE.SPLAY_RUNNING: RUN_Live(); break;
            case PRAC_STATE.SPOST_PLAY: RUN_PostPlay(); break;
        }
    }

    void ENTER_PickPlay(){
        mState = PRAC_STATE.SPICK_PLAY;
        cPlayerSetup.FSetUpPlayers(mPlay, rSnapSpot);
    }
    void RUN_PickPlay()
    {
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
}
