/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PP_UI : MonoBehaviour
{
    public Text             TXT_Instr;

    public Image            mBar;
    public Image            mMaxLine;
    public Text             mScoreTxt;

    [SerializeField]
    private DT_Player           PlayerData;         // used for max throw power at a minimum
    [SerializeField]
    private SO_Float            CurrentThrowMaxCharge;      // they could hit ctrl to take something off of it.
    [SerializeField]
    private SO_Float            CurThrowPwr;

    private bool            mIsWindingUp = false;

    // Start is called before the first frame update
    void Start()
    {
        mBar.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // render balls along trajectory
        if(mIsWindingUp){

            ShowThrowBar();

        }

    }

    public void ShowThrowBar()
    {
        mBar.fillAmount = CurThrowPwr.Val / PlayerData._ThrowSpd;
        mMaxLine.fillAmount = CurrentThrowMaxCharge.Val / PlayerData._ThrowSpd;
    }

    public void QB_Charging(){
        mIsWindingUp = true;
    }

    public void QB_ThrewBall(){
        mIsWindingUp = false;
        mBar.fillAmount = 0f;
        mMaxLine.fillAmount = 1f;
    }

}
