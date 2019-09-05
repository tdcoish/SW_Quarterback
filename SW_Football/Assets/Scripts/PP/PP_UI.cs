/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PP_UI : MonoBehaviour
{
    public Text             TXT_Instr;

    public Text             mScoreTxt;
    public Text             mSackedTxt;
    public Text             mPocketWarningTxt;
    public Text             mTimeLeftTxt;
    public Text             mSackImmunityTxt;
    public Text             mStreakTxt;
    public Text             mTrophyTxt;

    // Start is called before the first frame update
    void Start()
    {
        mPocketWarningTxt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        Color col = mSackedTxt.color;
        col.a -= Time.deltaTime;           // Sure, why not?
        mSackedTxt.color = col;
    }

    public void OnPlayerLeftPocket()
    {
        mPocketWarningTxt.gameObject.SetActive(true);
    }

    public void OnPlayerBackInPocket()
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
}
