/*************************************************************************************
Runs the coin toss
*************************************************************************************/
using UnityEngine;

public class TDC_Component : MonoBehaviour
{
    public virtual void FRunUpdate(){}
    public virtual void FEnter(){}
    public virtual void FExit(){}
}

public class EX_CoinToss : TDC_Component
{
    private EX_Man                          cMan;
    private EX_Plays                        cPlayMan;

    public enum STATE{
        S_PICK_HEADS,
        S_FLIPPING,
        S_RESULT,
        S_CHOOSE_SIDE
    }
    public STATE                            mState;
    
    public UI_CoinToss                      mUI;

    private float                           mTime;
    private bool                            mPickedHeads;
    private bool                            mPickedCorrectly;

    
    void Start()
    {
        cMan = GetComponent<EX_Man>();
        cPlayMan = GetComponent<EX_Plays>();
        mUI.mTxtHeadOrTail.gameObject.SetActive(false);
        mUI.mReceive.gameObject.SetActive(false);
        mUI.mKick.gameObject.SetActive(false);
        mUI.gameObject.SetActive(false);
    }
    public override void FEnter()
    {
        cMan.mState = EX_Man.STATE.S_COIN_TOSS;
        mState = STATE.S_PICK_HEADS;

        mUI.gameObject.SetActive(true);

        cMan.mTxtState.text = "Coin Toss";
        cMan.mTxtInstructions.text = "Pick heads or tails";
    }
    public override void FExit()
    {
        mUI.gameObject.SetActive(false);
    }

    public override void FRunUpdate()
    {
        switch(mState){
            case STATE.S_PICK_HEADS: RUN_Pick(); break;
            case STATE.S_FLIPPING: RUN_Flipping(); break;
            case STATE.S_RESULT: RUN_Result(); break;
            case STATE.S_CHOOSE_SIDE: RUN_PickSides(); break;
        }
    }

    // probably play some audio stuffs?
    public void RUN_Pick()
    {

    }

    public void ENTER_Flipping()
    {
        mState = STATE.S_FLIPPING;
        mUI.mHeads.gameObject.SetActive(false);
        mUI.mTails.gameObject.SetActive(false);
        cMan.mTxtInstructions.text = "Wait for result";
        mTime = Time.time;
    }
    public void RUN_Flipping()
    {
        if(Time.time - mTime > 0.5f){
            ENTER_Result();
        }
    }
    public void ENTER_Result()
    {
        mState = STATE.S_RESULT;

        // calculate the result
        float rand = Random.value;
        Debug.Log("Num: " + rand);
        // See if they are correct. Pretend 0.5> is heads
        mPickedCorrectly = false;
        if(rand > 0.5f){
            mUI.mTxtHeadOrTail.text = "HEADS!";
            if(mPickedHeads){
                mPickedCorrectly = true;
            } 
        }else{
            mUI.mTxtHeadOrTail.text = "TAILS!";
            if(!mPickedHeads) mPickedCorrectly = true;
        }

        mTime = Time.time;
    }
    public void RUN_Result()
    {
        // chill for a sec, then figure out if they won.
        if(Time.time - mTime > 0.5f){
            if(mPickedCorrectly){
                mUI.mTxtHeadOrTail.text = "YOU PICKED CORRECTLY!";
            }else{
                mUI.mTxtHeadOrTail.text = "YOU PICKED WRONG!";
            }
        }

        ENTER_PickSides();
    }

    /****************************************************
    If they were correct, they can pick to receiver, or they can pick which way to go.
    For now, just pretend that we always won.
    ****************************************************/
    public void ENTER_PickSides()
    {
        mState = STATE.S_CHOOSE_SIDE;
        cMan.mTxtInstructions.text = "Pick Sides";
        mUI.mReceive.gameObject.SetActive(true);
        mUI.mKick.gameObject.SetActive(true);
    }
    public void RUN_PickSides()
    {

    }

    public void BT_Heads()
    {
        BT_HeadsOrTails(true);
    }
    public void BT_Tails()
    {
        BT_HeadsOrTails(false);
    }

    public void BT_HeadsOrTails(bool heads)
    {
        mUI.mTxtHeadOrTail.gameObject.SetActive(true);
        if(heads){
            mUI.mTxtHeadOrTail.text = "You Picked: Heads";
        }else{
            mUI.mTxtHeadOrTail.text = "You Picked: Tails";
        }
        mPickedHeads = heads;
        ENTER_Flipping();
    }

    public void BT_Kick()
    {
        cPlayMan.mGameData.mReceivedFirst = GameData.POSSESSION.AWAY;
        StartPlaying();
    }
    public void BT_Receive()
    {
        cPlayMan.mGameData.mReceivedFirst = GameData.POSSESSION.HOME;
        StartPlaying();
    }

    private void StartPlaying()
    {
        FExit();
        cMan.mState = EX_Man.STATE.S_PLAYING;
        cPlayMan.FEnter();
    }
}
