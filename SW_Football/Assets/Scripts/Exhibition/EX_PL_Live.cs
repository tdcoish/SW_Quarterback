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
        mResult.mChoice = PLAY_CHOICE.C_RUN;

        float rand = Random.Range(-1f, 1f);
        rand = Mathf.Pow(rand, 3);

        float avg = 4f;
        float range = 10f;
        float runDis = avg + (range*rand);

        mResult.mDis = (int)runDis;
        mResult.mTurnover = false;
        mResult.mTimeTaken = 40f;

        mResult.mInfo = "Ran for " + mResult.mDis + " yards";

        cResult.FEnter();
    }

    // yeah passing is a little bit more complicated. Let's say 30% chance incompletion. 8% chance sack. 60% chance completion. 2% chance interception
    private void PlayPass()
    {
        mResult.mChoice = PLAY_CHOICE.C_PASS;

        float rand = Random.Range(0f, 1f);
        if(rand <= 0.08f){
            //sack.
            rand = Random.Range(0, 1f);
            float avg = -5f;
            mResult.mDis = (int) (rand*avg);
            mResult.mTimeTaken = 35f;
            mResult.mTurnover = false;
            mResult.mInfo = "Sacked for " + mResult.mDis + " yards";
        }else if(rand <= 0.1){
            // interception.
            mResult.mTimeTaken = 10f;
            float avg = 0f;
            rand = Random.Range(-1f, 1f);
            float range = 20f;
            float intDis = avg * (rand*range);
            mResult.mDis = 10;
            mResult.mTurnover = true;
            mResult.mInfo = "Intercepted for " + mResult.mDis + " yards";
        }
        else if(rand <= 0.4f){
            // incomplete.
            mResult.mTimeTaken = 5f;
            mResult.mDis = 0;
            mResult.mTurnover = false;
            mResult.mInfo = "Incomplete pass";
        }else{
            // completion.
            mResult.mTimeTaken = 35f;
            float avg = 10f;
            float range = 30f;
            rand = Random.Range(-0.5f, 1f);
            mResult.mDis = (int) (avg + rand*range);
            mResult.mTurnover = false;
            mResult.mInfo = "Completed pass for " + mResult.mDis + " yards";
        }
        
        cResult.FEnter();
    }
    private void PlayPunt()
    {
        mResult.mChoice = PLAY_CHOICE.C_PUNT;

        float avg = 35f;
        float range = 40f;
        float rand = Random.Range(-1f, 0.5f);
        mResult.mDis = (int) (avg + (rand*range));
        mResult.mTimeTaken = 15f;
        mResult.mTurnover = true;
        mResult.mInfo = "Punted for " + mResult.mDis + " net yards";

        cResult.FEnter();
    }
}
