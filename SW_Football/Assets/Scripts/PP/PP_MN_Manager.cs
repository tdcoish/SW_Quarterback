/*************************************************************************************
Score screen after pocket passer.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PP_MN_Manager : MonoBehaviour
{
    public Text                     TXT_Quit;
    public Text                     TXT_Score;
    public SO_Int                   mScoreGlobal;

    public float                    mTimeLeft = 4f;

    void Start()
    {
        TXT_Score.text = "SCORE: " + mScoreGlobal.Val;
    }

    void Update()
    {
        mTimeLeft -= Time.deltaTime;

        if(mTimeLeft < 3f)
        {
            TXT_Quit.text = "QUIT " + (int)(mTimeLeft + 1f);
        }

        if(mTimeLeft <= 0f)
        {
            Application.Quit();
        }
    }

}
