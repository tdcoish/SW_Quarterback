﻿/*************************************************************************************
Exhibition manager. For now takes us through the game.

BTW, abstract keyword might help me create state machine.

The components can have side effects. Eg. when we press any key in intro, the intro 
component changes our state for us.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class EX_Man : MonoBehaviour
{
    private EX_Intro                        cIntroMan;
    private EX_CoinToss                     cCoinMan;
    private EX_Plays                        cPlayMan;

    public enum STATE{
        S_INTRO,
        S_COIN_TOSS,
        S_PLAYING,
        S_END
    }
    public STATE                            mState;

    public Text                             mTxtState;
    public Text                             mTxtInstructions;

    void Start()
    {
        cIntroMan = GetComponent<EX_Intro>();
        cCoinMan = GetComponent<EX_CoinToss>();
        cPlayMan = GetComponent<EX_Plays>();

        cIntroMan.FEnter();
    }

    void Update()
    {
        switch(mState)
        {
            case STATE.S_INTRO: cIntroMan.FRunUpdate(); break;
            case STATE.S_COIN_TOSS: cCoinMan.FRunUpdate(); break;
            case STATE.S_PLAYING: cPlayMan.FRunUpdate(); break;
        }    
    }

}
