/*************************************************************************************
This is the logic for physically catching the ball.
*************************************************************************************/
using UnityEngine;

[RequireComponent(typeof(PRAC_Ath))]
public class AI_CatchHandling : MonoBehaviour
{
    private PRAC_Ath                    cAth;

    public enum STATE{
        S_NOCATCH,
        S_CONTROLLING,
        S_CONTROLLED
    }
    public STATE                        mState = STATE.S_NOCATCH;

    private float                       mCatchSecureTime = 0.5f;
    private float                       mBallHitHandsTime;

    void Start()
    {
        cAth = GetComponent<PRAC_Ath>();
    }

    void Update()
    {
        switch(mState){
            case STATE.S_CONTROLLING: RUN_Controlling(); break;
        }
    }

    public void FENTER_Controlling()
    {
        mBallHitHandsTime = Time.time;
        mState = STATE.S_CONTROLLING;

        cAth.mHasBall = true;
        TDC_EventManager.FBroadcast(TDC_GE.GE_BallHitFingers);
    }
    private void RUN_Controlling()
    {
        if(Time.time - mBallHitHandsTime > mCatchSecureTime){
            mState = STATE.S_CONTROLLED;
            cAth.FCaughtBall();
        }
    }
}
