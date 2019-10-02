/*************************************************************************************
For now just simulates the play. Take off a certain amount of time, etcetera. At the 
end of each play, we should know who has the ball, where they are, down and distance, 
and time left.
*************************************************************************************/
using UnityEngine;



public class EX_PL_Live : TDC_Component
{
    private EX_Plays                            cPlays;
    private EX_PL_Res                           cResult;

    public PLAY_RESULT                          mResult;

    void Start()
    {
        cPlays = GetComponent<EX_Plays>();   
        cResult = GetComponent<EX_PL_Res>();  
    }

    public override void FEnter()
    {
        cPlays.mState = EX_Plays.STATE.S_PLAYING;
    }

    // I guess this is more like, just running the play and seeing what happens?
    public override void FRunUpdate()
    {
        switch(cPlays.mChoice)
        {
            case PLAY_CHOICE.C_PASS: PlayPass(); break;
            case PLAY_CHOICE.C_RUN: PlayRun(); break;
            case PLAY_CHOICE.C_PUNT: PlayPunt(); break;
        }
    }

    // Average run, 4 yards. Make it biased towards being around there.
    // Would also need to bias towards really long runs. Do that later.
    private void PlayRun()
    {

        float rand = Random.Range(-1f, 1f);
        rand = Mathf.Pow(rand, 3);

        float avg = 4f;
        float range = 10f;
        float runDis = avg + (range*rand);

        mResult.mDis = (int)runDis;
        mResult.mChoice = PLAY_CHOICE.C_RUN;
        mResult.mTurnover = false;

        cResult.FEnter();
    }
    private void PlayPass()
    {

    }
    private void PlayPunt()
    {

    }
}
