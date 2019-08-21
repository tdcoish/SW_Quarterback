/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PP_UI : MonoBehaviour
{
    public Text             TXT_Instr;

    public Image            mBar;

    public Text             mScoreTxt;
    public Text             mSackedTxt;
    public Text             mPocketWarningTxt;
    public Text             mTimeLeftTxt;
    public Text             mInnacuracyTxt;
    public Text             mThrowInaccuracyTxt;
    public Text             mLookInaccuracyTxt;
    public Text             mSackImmunityTxt;
    public Text             mStreakTxt;

    [SerializeField]
    private DT_Player           PlayerData;         // used for max throw power at a minimum
    [SerializeField]
    private SO_Float            GB_ThrowCharge;

    private bool            mIsWindingUp = false;

    // innaccuracy is dependent on our movement alone. Throw innaccuracy is the cumulative innaccuracy of the current throw.
    public SO_Float             GB_Innacuracy;
    public SO_Float             GB_ThrowInaccuracy;
    public SO_Float             GB_ThrowLookInaccuracy;

    // Start is called before the first frame update
    void Start()
    {
        mBar.fillAmount = 0f;
        mPocketWarningTxt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // render balls along trajectory
        if(mIsWindingUp){

            ShowThrowBar();

        }

        Color col = mSackedTxt.color;
        col.a -= Time.deltaTime;           // Sure, why not?
        mSackedTxt.color = col;

        // Set accuracy text.
        mInnacuracyTxt.text = "Innacuracy: " + GB_Innacuracy.Val;
        mThrowInaccuracyTxt.text = "Throw Inaccuracy: " + GB_ThrowInaccuracy.Val;
        mLookInaccuracyTxt.text = "Look Inaccuracy: " + GB_ThrowLookInaccuracy.Val;
    }

    public void ShowThrowBar()
    {
        mBar.fillAmount = GB_ThrowCharge.Val;
    }

    public void QB_Charging(){
        mIsWindingUp = true;
    }

    public void QB_ThrewBall(){
        mIsWindingUp = false;
        mBar.fillAmount = 0f;
    }

    public void OnPlayerLeftPocket()
    {
        mPocketWarningTxt.gameObject.SetActive(true);
    }

    public void OnPlayerBackInPocket()
    {
        mPocketWarningTxt.gameObject.SetActive(false);
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
