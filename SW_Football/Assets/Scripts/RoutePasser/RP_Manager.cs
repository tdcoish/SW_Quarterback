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

    private PC_Controller           rPC;
    private List<RP_Receiver>       rRecs;
    private RP_ThrowSpot            rPocket;
    private List<RP_Hoop>           rHoops;

    // -------------------------------- 
    public string               sRingHit;           // set this to "NA" before every frame.
    public bool                 mBallCaught;
    public int                  mScore = 0;
    private bool                mInPocket;
    public bool                 mBallThrown;
    public float                mTimer;             // for now, always start at 5 seconds or something.

    // ---------------------------------
    public DATA_RP_Set          mActiveSet;
    public List<string>         mCompletions;

    // --------------------------------
    public RP_Hoop              PF_Ring;
    public RP_Receiver          PF_Receiver;

    void Awake()
    {
        IO_Settings.FLOAD_SETTINGS();
        IO_RouteList.FLOAD_ROUTES();
    }

    void Start()
    {
        mUI = GetComponent<RP_UI>();
        cRouteDrawer = GetComponent<RP_DrawRoutes>();

        // IO_RP_Diff.FLoadSet("EASY1");
        IO_RP_Dif.FSaveSet(mActiveSet);
        mActiveSet = IO_RP_Dif.FLoadSet("EASY4");
        // IO_RP_Dif.FSaveSet(mActiveSet);

        
        // Unfortunately, the destroyed receivers and hoops are still around, so we can't get references this frame.
        // Instead, do this on exitIntro.
        rPC = FindObjectOfType<PC_Controller>();
        rPocket = FindObjectOfType<RP_ThrowSpot>();

        mCompletions = new List<string>();
        ENTER_INTRO();
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

        // Destroy all receivers and rings who are still in the scene.
        RP_Receiver[] recs = FindObjectsOfType<RP_Receiver>();
        RP_Hoop[] hoops = FindObjectsOfType<RP_Hoop>();
        foreach(RP_Receiver rec in recs)
        {
            Destroy(rec.gameObject);
        }
        foreach(RP_Hoop h in hoops){
            Destroy(h.gameObject);
        }

        // ------------------- And spawn in our receivers and hoops.
        rRecs = new List<RP_Receiver>();
        rHoops = new List<RP_Hoop>();
        foreach(DATA_RP_Receiver r in mActiveSet.mReceiverData){
            RP_Receiver clone = Instantiate(PF_Receiver, r.mStartPos, transform.rotation);
            clone.mTag = r.mTag;
            clone.mRoute = r.mRoute;
            rRecs.Add(clone);
        }
        foreach(DATA_RP_Ring r in mActiveSet.mRingData){
            RP_Hoop clone = Instantiate(PF_Ring, r.mStartPos, Quaternion.Euler(r.mDir));
            clone.mWRTag = r.mTag;
            clone.transform.localScale = r.mScale;
            clone.mStartRot = r.mDir;
            rHoops.Add(clone);
        }
        Debug.Log("Receivers Instantiated: " + rRecs.Count);
        Debug.Log("Hoops Instantiated: " + rHoops.Count);
    }

    private void ENTER_PRESNAP()
    {
        mState = STATE.S_PRESNAP;
        mPreState = PRESNAP_STATE.SREADYTOSNAP;

        mUI.rPreSnapCanvas.gameObject.SetActive(true);
        rPC.mState = PC_Controller.PC_STATE.SPRE_SNAP;
        rPC.transform.position = mActiveSet.mPCSpot;
        foreach(RP_Receiver rec in rRecs)
        {
            rec.transform.position = FGetRecStartingSpot(rec.mTag);
            rec.FENTER_PRE_SNAP();
        }
        foreach(RP_Hoop hoop in rHoops)
        {
            hoop.transform.position = FGetRingStartingSpot(hoop.mWRTag);
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

        sRingHit = "NA";
        mBallCaught = false;

        mTimer = mActiveSet.mTimeToThrow;

        mUI.FMakeQBUIVisible();
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

        mUI.FMakeQBUIInvisible();
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
        mUI.FSetCombosDoneText(mCompletions.Count, rRecs.Count);
    }

    private void RUN_POST_PLAY()
    {
        if(mCompletions.Count == rRecs.Count){
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
    public void OnThroughRing(string sTag)
    {
        Debug.Log("Hit ring");
        sRingHit = sTag;
        Debug.Log(sRingHit);
    }
    // This will always happen second if things went well.
    public void OnBallCaught(string tag)
    {
        if(sRingHit == "NA"){
            HandlePlayResult("Need to throw through the ring first. FAILURE.", false);
            return;
        }

        if(tag != sRingHit){
            HandlePlayResult("Right Ring, Wrong Receiver. FAILURE.", false);
            return;
        }

        mBallCaught = true;
        if(mCompletions.Contains(tag))
        {
            HandlePlayResult("You already threw that combo. FAILURE.", false);
            return;
        }
        mCompletions.Add(tag);
        HandlePlayResult("Hit both the ring and the target. SUCCESS!", true);
        return;
    }
    public void OnBallHitGround()
    {
        if(sRingHit != "NA"){
            HandlePlayResult("Missed the player. FAILURE", false);
            return;
        }
        HandlePlayResult("Missed the ring. FAILURE.", false);
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


    public Vector3 FGetRecStartingSpot(string wrTag)
    {
        foreach(DATA_RP_Receiver r in mActiveSet.mReceiverData){

            if(r.mTag == wrTag)
            {
                return r.mStartPos;
            }
        }

        Debug.Log("No spot with that tag found");
        return Vector3.zero;
    }

    public Vector3 FGetRingStartingSpot(string ringTag)
    {
        foreach(DATA_RP_Ring r in mActiveSet.mRingData){
            if(r.mTag == ringTag){
                return r.mStartPos;
            }
        }
        
        Debug.Log("No ring spot with that tag found");
        return Vector3.zero;
    }

}
