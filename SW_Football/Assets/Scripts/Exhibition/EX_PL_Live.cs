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

    public MARK_EndZone                         rHomeEnd;
    public MARK_EndZone                         rAwayEnd;

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
        if(cPlays.mChoice == PLAY_CHOICE.C_PASS){
            mResult = Pass();
        }else if(cPlays.mChoice == PLAY_CHOICE.C_RUN){
            mResult = Run();
        }else if(cPlays.mChoice == PLAY_CHOICE.C_PUNT){
            mResult = Punt();
        }else if(cPlays.mChoice == PLAY_CHOICE.C_KICK){
            mResult = KickFieldGoal();
        }
        
        cResult.FEnter();
    }

    /*****************************************************************************
    Ultimately all these plays need to know where the ball is, since things are harder
    in the redzone, on both sides.
    *****************************************************************************/
    private PLAY_RESULT Run()
    {
        PLAY_RESULT res = new PLAY_RESULT();

        float rand1 = Random.Range(0f, 1f);
        if(rand1 <= 0.02f){
            // breakaway run here.
            float avg = 30f;
            float range = 10f;
            float rangeRand = Random.Range(-1f, 5f);        // bias towards really long runs.      
            float runDis = avg + rangeRand * range;

            res.mDis = (int)runDis;
            res.mTurnover = false;
            res.mTimeTaken = 15f; 
            res.mInfo = "Big run for: " + res.mDis + " yards";

        }else if (rand1 < 0.04f){
            res.mTurnover = true;
            res.mDis = 5;
            res.mTimeTaken = 10f;
            res.mInfo = "Fumbled the ball!";
        } else{
            float rand = Random.Range(-1f, 1f);
            rand = Mathf.Pow(rand, 3);

            float avg = 4f;
            float range = 10f;
            float runDis = avg + (range*rand);

            res.mDis = (int)runDis;
            res.mTimeTaken = 30f;

            res.mTurnover = false;
            res.mInfo = "Standard run for " + res.mDis + " yards";
        }

        return res;
    }

    private PLAY_RESULT Pass()
    {
        PLAY_RESULT res = new PLAY_RESULT();
        
        res.mChoice = PLAY_CHOICE.C_PASS;

        float rand = Random.Range(0f, 1f);
        if(rand <= 0.08f){
            //sack.
            rand = Random.Range(0, 1f);
            float avg = -5f;
            res.mDis = (int) (rand*avg);
            res.mTimeTaken = 35f;
            res.mTurnover = false;
            res.mInfo = "Sacked for " + res.mDis + " yards";
        }else if(rand <= 0.1){
            // interception.
            res.mTimeTaken = 10f;
            float avg = 0f;
            rand = Random.Range(-1f, 1f);
            float range = 20f;
            float intDis = avg * (rand*range);
            res.mDis = 10;
            res.mTurnover = true;
            res.mInfo = "Intercepted for " + res.mDis + " yards";
        }
        else if(rand <= 0.4f){
            // incomplete.
            res.mTimeTaken = 5f;
            res.mDis = 0;
            res.mTurnover = false;
            res.mInfo = "Incomplete pass";
        }else{
            // completion.
            res.mTimeTaken = 35f;
            float avg = 10f;
            float range = 30f;
            rand = Random.Range(-0.5f, 1f);
            res.mDis = (int) (avg + rand*range);
            res.mTurnover = false;
            res.mInfo = "Completed pass for " + res.mDis + " yards";
        }

        return res;
    }

    /********************************************************************************
    Previously I had failed to accurately simulate big plays. Most punts are going to 
    be ~45 yards, give or take 20 yards. However, some smaller percentage are going to 
    be fumbles, blocks, or big returns for touchdowns.
    ********************************************************************************/
    private PLAY_RESULT Punt()
    {
        PLAY_RESULT res = new PLAY_RESULT();
        res.mChoice = PLAY_CHOICE.C_PUNT;

        float rand1 = Random.Range(0f, 1f);
        if(rand1 < 0.02f){
            // fumbled, not worrying about blocked now.
            float avg = 40f;
            float range = 10f;
            float rand = Random.Range(-1f, 1f);
            res.mDis = (int) (avg + (rand*range));
            res.mTimeTaken = 15f;
            res.mTurnover = false;
            res.mInfo = "Muffed Punt, Punting team gets the ball back.";
        }else if(rand1 < 0.1f){
            // big return
            float avg = -10f;
            float range = 40f;
            float rand = Random.Range(-1f, 0.5f);
            res.mDis = (int) (avg + (rand*range));
            res.mTimeTaken = 20f;
            res.mTurnover = true;
            res.mInfo = "Big Return for: " + res.mDis + " net yards";
        }else{
            // normal.
            float avg = 35f;
            float range = 20f;
            float rand = Random.Range(-1f, 1f);
            res.mDis = (int) (avg + (rand*range));
            res.mTimeTaken = 5f;
            res.mTurnover = true;
            res.mInfo = "Standard Punt for: " + res.mDis + " net yards";
        }

        return res;
    }

    private PLAY_RESULT KickFieldGoal()
    {
        PLAY_RESULT res = new PLAY_RESULT();
        
        res.mChoice = PLAY_CHOICE.C_KICK;

        // calc dis of the field goal.
        int dis;
        GameData.FIELD_POS uprightPos;
        if(cPlays.mGameData.mPossession == GameData.POSSESSION.HOME){
            uprightPos = cResult.FGetFieldPosFromWorldCoordinates(rAwayEnd.transform.position.z);
            Debug.Log("upright spot: " + uprightPos.mYardMark);
            dis = cResult.FGetRawYardDistance(uprightPos, cPlays.mGameData.mBallLoc) + 15;
        } else{
            uprightPos = cResult.FGetFieldPosFromWorldCoordinates(rHomeEnd.transform.position.z);
            Debug.Log("upright spot: " + uprightPos.mYardMark);
            dis = cResult.FGetRawYardDistance(uprightPos, cPlays.mGameData.mBallLoc) + 15;
        }

        Debug.Log("Field goal distance: " + dis);

        // you always score when kicking less than 20.
        float gimmieDis = 20f;
        int adjDis = dis;
        adjDis -= (int)gimmieDis;
        // always miss when kicking further than 65.
        float missProb = (float)adjDis / (65f-gimmieDis);
        missProb = Mathf.Pow(missProb, 3);
        Debug.Log("Miss Probability: " + missProb);

        float chance = Random.Range(0f, 1f);
        if(chance < missProb)
        {
            res.mSuccessfulFieldGoal = false;
            res.mInfo = "Missed field goal from: " + dis + " yards";
        }else{
            res.mSuccessfulFieldGoal = true;
            res.mInfo = "Hit field goal from: " + dis + " yards";
        }

        res.mTimeTaken = 5f;
        res.mTurnover = false;                  // just manually handling the field goal as its own thing
        res.mDis = 0;

        return res;
    }
}
