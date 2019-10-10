/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PROFST_Pre : PROFST_St
{

    private PRAC_Off_ShowGFX                    cShowPreSnapGFX;

    public PRAC_STATE                           mState;
    public PRESNAP_STATE                        mPreSnapState;

    public override void Start()
    {
        base.Start();
        cShowPreSnapGFX = GetComponent<PRAC_Off_ShowGFX>(); 
    }

    public override void FEnter(){
        cMan.mState = PRAC_STATE.SPRE_SNAP;

        cMan.GetComponentInChildren<AD_Prac>().FPlayWhistle();

        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            a.mState = PRAC_Ath.PRAC_ATH_STATE.SPRE_SNAP;
        }
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SPRE_SNAP;

        cShowPreSnapGFX.FShowOffensivePlay(IO_OffensivePlays.FLoadPlay(cMan.mPlay), cMan.rSnapSpot);
    }
    // Fill in the high camera stuff later.
    public override void FRun()
    {
        switch(mPreSnapState)
        {
            case PRESNAP_STATE.SREADYTOSNAP: RUN_SnapReady(); break;
            case PRESNAP_STATE.SHIGHCAM: RUN_HighCam(); break;
        }
    }
    public override void FExit(){
        cShowPreSnapGFX.FStopShowingPlayArt();
    }


    void RUN_SnapReady(){
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FExit();
            cLive.FEnter();
        }

        // We also need the camera to go to the higher perspective.
        if(Input.GetKeyDown(KeyCode.T))
        {
            cShowPreSnapGFX.FStopShowingPlayArt();
            cShowPreSnapGFX.FShowOffensivePlay(IO_OffensivePlays.FLoadPlay(cMan.mPlay), cMan.rSnapSpot);
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

}
