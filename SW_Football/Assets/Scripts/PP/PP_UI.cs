/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PP_UI : MonoBehaviour
{
    public Text             TXT_Instr;

    public Text             mScoreTxt;
    public Text             mPocketWarningTxt;
    public Text             mTimeLeftTxt;
    public Text             mSackImmunityTxt;
    public Text             mStreakTxt;
    public Text             mTrophyTxt;

    private float           mReactTxtSetTime;
    public Text             mReactionText;

    private float           mCountdownTextSetTime;
    public Text             mCountdownText;

    // Start is called before the first frame update
    void Start()
    {
        TDC_EventManager.FAddHandler(TDC_GE.GE_OutPocket, E_PlayerLeftPocket);
        TDC_EventManager.FAddHandler(TDC_GE.GE_InPocket, E_PlayerBackInPocket);
        mPocketWarningTxt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // -------------------- The reaction text
        float timeSinceReact = Time.time - mReactTxtSetTime;
        if(timeSinceReact < 0.2f){
            float percentThere = timeSinceReact / 0.2f;
            Vector3 vSize = new Vector3(percentThere, percentThere, percentThere);
            mReactionText.transform.localScale = vSize;
        }else{
            Color col2 = mReactionText.color;
            col2.a -= Time.deltaTime;
            mReactionText.color = col2;
        }

        // -------------------- The Countdown Text
        timeSinceReact = Time.time - mCountdownTextSetTime;
        if(timeSinceReact < 0.1f){
            float percentThere = timeSinceReact / 0.1f;
            Vector3 vSize = new Vector3(percentThere, percentThere, percentThere);
            mCountdownText.transform.localScale = vSize;
        }else{
            Color col2 = mCountdownText.color;
            col2.a -= Time.deltaTime * 2;
            mCountdownText.color = col2;
        }
    }

    public void E_PlayerLeftPocket()
    {
        mPocketWarningTxt.gameObject.SetActive(true);
    }

    public void E_PlayerBackInPocket()
    {
        mPocketWarningTxt.gameObject.SetActive(false);
    }

    public void FSetTrophyText(string trophyWon)
    {
        mTrophyTxt.text = "TROPHY: " + trophyWon;
    }

    public void FSetTimeText(float tm)
    {
        tm = (float)System.Math.Round((double)tm, 0);
        mTimeLeftTxt.text = "Time: " + tm;
    }

    public void FSetStreakText(int streakBonus)
    {
        mStreakTxt.text = "Streak Multiplier: x" + streakBonus;
    }

    public void FSetReactionText(string blurb)
    {
        Color col = mReactionText.color; col.a = 1f; mReactionText.color = col;
        mReactionText.text = blurb;
        mReactTxtSetTime = Time.time;
    }

    public void FSetCountDownText(float tmLeft)
    {
        Color col = mCountdownText.color; col.a = 1f; mCountdownText.color = col;
        int iTime = (int)tmLeft + 1;
        string num = iTime.ToString("D1") + "!";
        mCountdownText.text = num;
        mCountdownTextSetTime = Time.time;
    }
}
