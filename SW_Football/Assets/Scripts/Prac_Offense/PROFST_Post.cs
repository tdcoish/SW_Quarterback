/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PROFST_Post : PROFST_St
{
    public override void Start()
    {
        base.Start();
        mUI.gameObject.SetActive(false);
    }

    private float                               mTime;
    public UI_PostPlay                          mUI;

    public override void FEnter(){
        cMan.mState = PRAC_STATE.SPOST_PLAY;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mUI.gameObject.SetActive(true);
        cMan.GetComponentInChildren<AD_Prac>().FPlayOver(cMan.mRes.mBallCaught, cMan.mRes.mInt);
        mUI.FSetPostPlayText(cMan.mRes);
        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            a.mState = PRAC_Ath.PRAC_ATH_STATE.SPOST_PLAY;
        }

        mTime = Time.time;
    }
    public override void FRun(){
        if(Time.time - mTime > 3f){
            FExit();
            cPick.FEnter();
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            FExit();
            cPick.FEnter();
        }
    }
    public override void FExit(){
        mUI.gameObject.SetActive(false);
        PRAC_Ath[] aths = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath a in aths){
            Destroy(a.gameObject);
        }
        FindObjectOfType<PC_Controller>().mState = PC_Controller.PC_STATE.SINACTIVE;

        PROJ_Football[] footballs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in footballs){
            Destroy(f.gameObject);
        }

        cMan.mRes.mBallCaught = false;
        cMan.mRes.mInt = false;
    }

    public void BT_NextPlay()
    {
        FExit();
        cPick.FEnter();
    }

}
