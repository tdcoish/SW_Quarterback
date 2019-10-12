/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PROFST_Pick : PROFST_St
{
    public PRAC_PB_UI                               mUI;

    private PRAC_Off_SetupPlayers               cPlayerSetup;
    private PRAC_Def_SetupPlayers               cDefPlayerSetup;

    public PP_Turret                            PF_Turret;

    private bool                                mFrameWasted = false;               // have to waste a frame to let the game objects we want to get references to actually spawn.

    public override void Start()
    {
        base.Start();
        cPlayerSetup = GetComponent<PRAC_Off_SetupPlayers>();   
        cDefPlayerSetup = GetComponent<PRAC_Def_SetupPlayers>();
    }

    public override void FEnter(){
        cMan.mState = PRAC_STATE.SPICK_PLAY;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mUI.gameObject.SetActive(true);
        mUI.FSetUpPlaybookImagery();
        mUI.FSetLineEnabledText(cMan.mLineExists);
        mUI.FSetDefEnabledText(cMan.mDefenseExists);
        mFrameWasted = false;

        cMan.rTurrets.Clear();
    }
    public override void FRun()
    {
        mUI.FRunUpdate();

        if(mFrameWasted)
        {
            cPre.FEnter();
        }
    }
    public override void FExit(){
        Cursor.lockState = CursorLockMode.Locked;
        mUI.gameObject.SetActive(false);
        Cursor.visible = false;

        if(!cMan.mLineExists || !cMan.mOlineExists){
            PRAC_Off_Ply[] aths = FindObjectsOfType<PRAC_Off_Ply>();
            foreach(PRAC_Off_Ply a in aths){
                if(a.mRole == "BLOCK"){
                    Destroy(a.gameObject);
                }
            }
        }
        // now defense as well.
        if(cMan.mDefenseExists){
            if(!cMan.mLineExists){
                PRAC_Def_Ply[] aths = FindObjectsOfType<PRAC_Def_Ply>();
                foreach(PRAC_Def_Ply p in aths)
                {
                    if(p.mJob.mRole == "Pass Rush"){
                        Destroy(p.gameObject);
                    }
                }
            }else{
                PRAC_Def_Ply[] aths = FindObjectsOfType<PRAC_Def_Ply>();
                foreach(PRAC_Def_Ply a in aths){
                    if(a.mJob.mRole == "Pass Rush"){
                        // Instantiate a turret, keep track.
                        PP_Turret t = Instantiate(PF_Turret, a.transform.position, a.transform.rotation);
                        cMan.rTurrets.Add(t);
                        Destroy(a.gameObject);
                    }
                }
            }
        }
        // Get rid of the QB
        {
            PRAC_Off_Ply[] aths = FindObjectsOfType<PRAC_Off_Ply>();
            foreach(PRAC_Off_Ply a in aths){
                if(a.mRole == "QB"){
                    Destroy(a.gameObject);
                }
            }
        }

        mFrameWasted = true;
    }

    public void FOffPlayPicked(string name)
    {
        if(cMan.mState != PRAC_STATE.SPICK_PLAY){
            Debug.Log("ERROR. Picked play from wrong state");
            return;
        }
        cMan.mPlay = name;
        cPlayerSetup.FSetUpPlayers(cMan.mPlay, cMan.rSnapSpot);

        if(cMan.mDefenseExists){
            // set up defense here
            // TODO:
            cDefPlayerSetup.FSetUpPlayers("Cover 2", cMan.rSnapSpot);
        }

        FExit();
    }

    public void BT_ToggleLineEnabled()
    {
        cMan.mLineExists = !cMan.mLineExists;
        mUI.FSetLineEnabledText(cMan.mLineExists);
    }
    public void BT_ToggleOLineEnabled()
    {
        cMan.mOlineExists = !cMan.mOlineExists;
        mUI.FSetOLineEnabledText(cMan.mOlineExists);
    }
    public void BT_ToggleDefEnabled()
    {
        cMan.mDefenseExists = !cMan.mDefenseExists;
        mUI.FSetDefEnabledText(cMan.mDefenseExists);
    }
    public void BT_ToggleCamFollow()
    {
        cLive.mMakeCamFollowBall = !cLive.mMakeCamFollowBall;
        mUI.FSetCamFollowText(cLive.mMakeCamFollowBall);
    }
}
