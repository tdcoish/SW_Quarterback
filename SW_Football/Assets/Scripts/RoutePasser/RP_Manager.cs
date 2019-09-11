/*************************************************************************************
This is the game mode where you just throw to receivers. Eventually through hoops.

The actual game itself gives you six tries. After all six tries, they load up a new field
with six more tires, and do this again. You get bonus points for getting all the hoops done
early.

Need to figure out what happened after the play stops being live.

1) Receiver catches it after it goes through the hoop. Success.
2) Receiver catches it but it doesn't go through the hoop, Failure.
3) Goes through the hoop, but the receiver doesn't catch it. Failure.
4) Doesn't go through the hoop AND the receiver doesn't catch it. Failure.
5) Throw from outside the pocket. Failure.

Points need to be allocated, and the UI text needs to be updated.

Now for resetting the receivers. We need to store receivers starting positions and routes.
We also need to store which rings correspond to which receivers. Let's start with the hacky version.

Just make a list of the valid completions. When we have all of them, then we are done.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class RP_Manager : MonoBehaviour
{
    private RP_UI               mUI;
    private RP_DrawRoutes       cRouteDrawer;

    private enum STATE{
        S_INTRO_TEXT,
        S_PRESNAP,
        S_LIVE,
        S_POST_PLAY,
        S_OUTRO
    }
    private STATE               mState;

    public enum PRESNAP_STATE
    {
        SREADYTOSNAP,
        SHIGHCAM
    }
    private PRESNAP_STATE       mPreState;

    private PC_Controller       rPC;
    private RP_Receiver[]       rRecs;
    private RP_ThrowSpot        rPocket;
    private RP_Hoop[]           rHoops;

    // -------------------------------- 
    public bool                 mHitRing;
    public bool                 mBallCaught;
    public int                  mScore = 0;
    private bool                mInPocket;
    public bool                 mBallThrown;
    public float                mTimer;             // for now, always start at 5 seconds or something.

    // ---------------------------------
    public RP_ReceiverList      rSet;
    public List<string>         mCompletions;

    void Awake()
    {
        IO_Settings.FLOAD_SETTINGS();
        IO_RouteList.FLOAD_ROUTES();
    }

    void Start()
    {
        mUI = GetComponent<RP_UI>();
        cRouteDrawer = GetComponent<RP_DrawRoutes>();
        
        mState = STATE.S_INTRO_TEXT;
        rPC = FindObjectOfType<PC_Controller>();
        rRecs = FindObjectsOfType<RP_Receiver>();
        rPocket = FindObjectOfType<RP_ThrowSpot>();
        rHoops = FindObjectsOfType<RP_Hoop>();

        mCompletions = new List<string>();

        rSet.FStoreSet();
    }

    void Update()
    {
        switch(mState)
        {
            case STATE.S_INTRO_TEXT: RUN_INTRO(); break;
            case STATE.S_PRESNAP: RUN_PRESNAP(); break;
            case STATE.S_LIVE: RUN_LIVE(); break;
            case STATE.S_POST_PLAY: RUN_POST_PLAY(); break;
            case STATE.S_OUTRO: RUN_OUTRO(); break;
        }

        // We can always pause or not.
    }

    private void ENTER_INTRO()
    {
        mState = STATE.S_INTRO_TEXT;

        mUI.rIntroCanvas.gameObject.SetActive(true);
        rPC.mState = PC_Controller.PC_STATE.SINACTIVE;
        foreach(RP_Receiver rec in rRecs)
        {
            rec.mState = RP_Receiver.STATE.SPRE_SNAP;
        }
    }
    
    private void ENTER_PRESNAP()
    {
        mState = STATE.S_PRESNAP;
        mPreState = PRESNAP_STATE.SREADYTOSNAP;

        mUI.rPreSnapCanvas.gameObject.SetActive(true);
        rPC.mState = PC_Controller.PC_STATE.SPRE_SNAP;
        rPC.transform.position = rSet.mPCSpot;
        foreach(RP_Receiver rec in rRecs)
        {
            rec.transform.position = rSet.FGetRecSpot(rec.mTag);
            rec.FENTER_PRE_SNAP();
        }
        foreach(RP_Hoop hoop in rHoops)
        {
            hoop.transform.position = rSet.FGetRingSpot(hoop.mWRTag);
            // hoop.transform.LookAt(rPocket.transform.position);
        }
    }

    private void ENTER_LIVE()
    {
        mState = STATE.S_LIVE;
        mUI.rPlayLiveCanvas.gameObject.SetActive(true);

        rPC.mState = PC_Controller.PC_STATE.SACTIVE;
        foreach(RP_Receiver rec in rRecs)
        {
            rec.mState = RP_Receiver.STATE.SDOING_JOB;
        }

        mHitRing = false;
        mBallCaught = false;
        mTimer = 10f;

        // EXIT_LIVE();
        // ENTER_OUTRO();
    }

    private void ENTER_POST_SNAP()
    {
        mState = STATE.S_POST_PLAY;
        mUI.rPostPlayCanvas.gameObject.SetActive(true);

        rPC.mState = PC_Controller.PC_STATE.SINACTIVE;
        foreach(RP_Receiver rec in rRecs)
        {
            rec.mState = RP_Receiver.STATE.SPOST_PLAY;
        }
    }

    private void ENTER_OUTRO()
    {
        mState = STATE.S_OUTRO;

        mUI.rOutroCanvas.gameObject.SetActive(true);
        rPC.GetComponentInChildren<Camera>().enabled = false;
        rPC.GetComponentInChildren<AudioListener>().enabled = false;
        rPC.mState = PC_Controller.PC_STATE.SINACTIVE;
        FindObjectOfType<CAM_Outro>().FActivate();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        FindObjectOfType<QB_UI>().gameObject.SetActive(false);
    }

    private void EXIT_INTRO()
    {
        mUI.rIntroCanvas.gameObject.SetActive(false);
    }
    private void EXIT_PRESNAP()
    {
        mUI.rPreSnapCanvas.gameObject.SetActive(false);
    }
    private void EXIT_LIVE()
    {
        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football fb in footballs){
            Destroy(fb.gameObject);
        }
        mUI.rPlayLiveCanvas.gameObject.SetActive(false);
        mInPocket = false;

        // Now here's where we reset the receiver.
        mBallThrown = false;
    }
    private void EXIT_POST_SNAP()
    {
        mUI.rPostPlayCanvas.gameObject.SetActive(false);
    }
    private void EXIT_OUTRO()
    {

    }

    private void RUN_INTRO()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EXIT_INTRO();
            ENTER_PRESNAP();
        }
    }

    private void RUN_PRESNAP()
    {
        if(mPreState == PRESNAP_STATE.SREADYTOSNAP){
            if(Input.GetKeyDown(KeyCode.Space))
            {
                EXIT_PRESNAP();
                ENTER_LIVE();
            }
            // Now here's where we show the play art.
            if(Input.GetKeyDown(KeyCode.T))
            {
                // Go to high snap state.
                cRouteDrawer.FShowRouteGraphics();
                FindObjectOfType<CAM_PlayShowing>().FActivate();
                mPreState = PRESNAP_STATE.SHIGHCAM;
            }
        }else if(mPreState == PRESNAP_STATE.SHIGHCAM){
            if(Input.GetKeyDown(KeyCode.T)){
                cRouteDrawer.FStopShowingRoutes();
                FindObjectOfType<CAM_PlayShowing>().FDeactivate();
                mPreState = PRESNAP_STATE.SREADYTOSNAP;
            }
        }
    }

    private void RUN_LIVE()
    {
        mTimer -= Time.deltaTime;
        if(!mBallThrown && mTimer < 0f){
            HandlePlayResult("Ran out of time, sacked.", false);
        }
        mUI.FSetTimerText(mTimer, mBallThrown);
        mUI.FSetPocketText(mInPocket);
        mUI.FSetCombosDoneText(mCompletions.Count, rRecs.Length);
    }

    private void RUN_POST_PLAY()
    {
        if(mCompletions.Count == rRecs.Length){
            Debug.Log("You've won this set already");
            EXIT_POST_SNAP();
            ENTER_OUTRO();
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            EXIT_POST_SNAP();
            ENTER_PRESNAP();
        }
    }

    private void RUN_OUTRO()
    {
        // Maybe load the main menu back or something.
        mUI.FSetOutroScoreText(mScore);
    }

    // ------------------------------ Things that happen in the world can trigger these.
    public void OnThroughRing()
    {
        Debug.Log("Hit ring");
        mHitRing = true;
    }
    // This will always happen second if things went well.
    public void OnBallCaught(string tag)
    {
        mBallCaught = true;
        if(mHitRing)
        {
            if(mCompletions.Contains(tag))
            {
                HandlePlayResult("You already threw that combo. FAILURE.", false);
                return;
            }
            mCompletions.Add(tag);
            HandlePlayResult("Hit both the ring and the target. SUCCESS!", true);
            return;
        }
        HandlePlayResult("Missed Ring. FAILURE.", false);
    }
    public void OnBallHitGround()
    {
        if(!mHitRing){
            HandlePlayResult("Missed the ring. FAILURE.", false);
            return;
        }
        HandlePlayResult("Missed the player. FAILURE", false);
    }

    public void OnEnteredPocket()
    {
        mInPocket = true;
    }
    public void OnExitPocket()
    {
        mInPocket = false;
    }
    
    public void OnBallThrown()
    {
        mBallThrown = true;
        if(!mInPocket)
        {
            HandlePlayResult("Threw from outside pocket. FAILURE.", false);
        }
    }

    private void HandlePlayResult(string msg, bool success)
    {
        if(success)
        {
            mScore += 50;
        }
        mUI.FSetPostPlayText(msg);

        EXIT_LIVE();
        ENTER_POST_SNAP();
    }

}
