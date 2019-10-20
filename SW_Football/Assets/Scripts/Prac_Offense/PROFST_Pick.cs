/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class PROFST_Pick : PROFST_St
{
    public PRAC_Pick_UI                           mUI;

    private PRAC_Off_SetupPlayers               cOffPlayerSetup;
    private PRAC_Def_SetupPlayers               cDefPlayerSetup;

    private bool                                mFrameWasted = false;               // have to waste a frame to let the game objects we want to get references to actually spawn.

    public override void Start()
    {
        base.Start();
        cOffPlayerSetup = GetComponent<PRAC_Off_SetupPlayers>();   
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

        // Get references to all the players in the scene.
        PRAC_Ath[] athletes = FindObjectsOfType<PRAC_Ath>();
        Debug.Log("Num aths: " + athletes.Length);
        cMan.rAths.Clear();
        foreach(PRAC_Ath a in athletes){
            cMan.rAths.Add(a);
        }

        if(!cMan.mLineExists || !cMan.mOlineExists){
            for(int i=0; i<cMan.rAths.Count; i++){
                PRAC_Off_Ply p = cMan.rAths[i].GetComponent<PRAC_Off_Ply>();
                if(p != null){
                    if(p.mRole == "BLOCK"){
                        Destroy(p.gameObject);
                        cMan.rAths.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        Debug.Log("Count: " + cMan.rAths.Count);
        // now defense as well.
        if(cMan.mDefenseExists){
            if(!cMan.mLineExists){
                for(int i=0; i<cMan.rAths.Count; i++){
                    PRAC_Def_Ply p = cMan.rAths[i].GetComponent<PRAC_Def_Ply>();
                    if(p != null){
                        if(p.mJob.mRole == "Pass Rush"){
                            Destroy(p.gameObject);
                            cMan.rAths.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }else{          // have to shove them all back 1 yard.
                for(int i=0; i<cMan.rAths.Count; i++){
                    PRAC_Def_Ply p = cMan.rAths[i].GetComponent<PRAC_Def_Ply>();
                    if(p != null){
                        if(p.mJob.mRole == "Pass Rush"){
                            Vector3 vPos = p.transform.position;
                            vPos.z += 2f;
                            p.transform.position = vPos;
                        }
                    }
                }
            }
        }
        Debug.Log("Count: " + cMan.rAths.Count);

        // Get rid of the QB
        {
            for(int i=0; i<cMan.rAths.Count; i++){
                PRAC_Off_Ply p = cMan.rAths[i].GetComponent<PRAC_Off_Ply>();
                if(p != null){
                    if(p.mRole == "QB"){
                        Destroy(p.gameObject);
                        cMan.rAths.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        Debug.Log("Count: " + cMan.rAths.Count);

        mFrameWasted = true;
    }

    public void FOffPlayPicked(string name)
    {
        if(cMan.mState != PRAC_STATE.SPICK_PLAY){
            Debug.Log("ERROR. Picked play from wrong state");
            return;
        }
        cMan.mPlay = name;
        cOffPlayerSetup.FSetUpPlayers(cMan.mPlay, cMan.rSnapSpot);

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
