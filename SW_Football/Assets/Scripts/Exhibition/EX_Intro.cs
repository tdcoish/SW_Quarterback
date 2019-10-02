/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class EX_Intro : TDC_Component
{
    private EX_Man                              cMan;
    private EX_CoinToss                         cCoinMan;

    void Awake()
    {
        cMan = GetComponent<EX_Man>();
        cCoinMan = GetComponent<EX_CoinToss>();
    }

    public override void FEnter()
    {
        cMan.mState = EX_Man.STATE.S_INTRO;
        cMan.mTxtState.text = "Intro State";
        cMan.mTxtInstructions.text = "Press Any Key to Continue...";
    }
    public override void FRunUpdate()
    {
        if(Input.anyKeyDown){
            cCoinMan.FEnter();
        }
    }
}
