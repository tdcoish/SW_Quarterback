/*************************************************************************************
This is the game mode where you just throw to receivers. Eventually through hoops.
*************************************************************************************/
using UnityEngine;

public class RP_Manager : MonoBehaviour
{

    private enum STATE{
        S_INTRO_TEXT,
        S_PRESNAP,
        S_LIVE,
        S_POST_PLAY
    }
    private STATE               mState;

    public Canvas               rIntroCanvas;
    private PC_Controller       rPC;
    private RP_Receiver[]       rRecs;

    void Awake()
    {
        IO_Settings.FLOAD_SETTINGS();
        IO_RouteList.FLOAD_ROUTES();
    }

    void Start()
    {
        mState = STATE.S_INTRO_TEXT;
        rPC = FindObjectOfType<PC_Controller>();
        rRecs = FindObjectsOfType<RP_Receiver>();
    }

    void Update()
    {
        switch(mState)
        {
            case STATE.S_INTRO_TEXT: RUN_INTRO(); break;
            case STATE.S_PRESNAP: RUN_PRESNAP(); break;
            case STATE.S_LIVE: RUN_LIVE(); break;
            case STATE.S_POST_PLAY: RUN_POST_PLAY(); break;
        }
    }

    private void ENTER_INTRO()
    {
        mState = STATE.S_INTRO_TEXT;

        rIntroCanvas.gameObject.SetActive(true);
        rPC.mState = PC_Controller.PC_STATE.SINACTIVE;
        foreach(RP_Receiver rec in rRecs)
        {
            rec.mState = RP_Receiver.STATE.SPRE_SNAP;
        }
    }
    
    private void ENTER_PRESNAP()
    {
        mState = STATE.S_PRESNAP;
        
        rPC.mState = PC_Controller.PC_STATE.SPRE_SNAP;
        foreach(RP_Receiver rec in rRecs)
        {
            rec.mState = RP_Receiver.STATE.SPRE_SNAP;
        }
    }

    private void ENTER_LIVE()
    {
        mState = STATE.S_LIVE;

        rPC.mState = PC_Controller.PC_STATE.SACTIVE;
        foreach(RP_Receiver rec in rRecs)
        {
            rec.mState = RP_Receiver.STATE.SDOING_JOB;
        }
    }

    private void ENTER_POST_SNAP()
    {

    }

    private void EXIT_INTRO()
    {
        rIntroCanvas.gameObject.SetActive(false);
        rPC.mState = PC_Controller.PC_STATE.SACTIVE;
    }
    private void EXIT_PRESNAP()
    {

    }
    private void EXIT_LIVE()
    {

    }
    private void EXIT_POST_SNAP()
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EXIT_PRESNAP();
            ENTER_LIVE();
        }
    }

    private void RUN_LIVE()
    {

    }

    private void RUN_POST_PLAY()
    {

    }
}
