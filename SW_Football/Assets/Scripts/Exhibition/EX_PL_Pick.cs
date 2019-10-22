/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class EX_PL_Pick : TDC_Component
{
    private EX_Plays                            cPlays;
    private EX_PL_Live                          cLive;

    public GameObject                           mUI;

    void Start()
    {
        cPlays = GetComponent<EX_Plays>();
        cLive = GetComponent<EX_PL_Live>();
        mUI.SetActive(false);    
    }

    public override void FEnter()
    {
        mUI.SetActive(true);
    }
    public override void FExit()
    {
        mUI.SetActive(false);
    }

    // should have time limit and stuff like that.
    public override void FRunUpdate()
    {
        // pretend time ticks down for delay of game or whatever.
    }

    // They pick the style of play they want, then it happens.
    public void BT_Run()
    {
        cPlays.mChoice = PLAY_CHOICE.C_RUN;
        FExit();
        cLive.FEnter();
    }
    public void BT_Pass()
    {
        cPlays.mChoice = PLAY_CHOICE.C_PASS;
        FExit();
        cLive.FEnter();
    }
    public void BT_Punt()
    {
        cPlays.mChoice = PLAY_CHOICE.C_PUNT;
        FExit();
        cLive.FEnter();
    }
    public void BT_Kick()
    {
        cPlays.mChoice = PLAY_CHOICE.C_KICK;
        FExit();
        cLive.FEnter();
    }
    public void BT_Kneeldown()
    {
        cPlays.mChoice = PLAY_CHOICE.C_KNEELDOWN;
        FExit();
        cLive.FEnter();
    }

}
