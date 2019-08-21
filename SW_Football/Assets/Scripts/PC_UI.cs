using UnityEngine;
using UnityEngine.UI;

// Want to draw little bar representing the maximum "point" of the current throw.
public class PC_UI : MonoBehaviour
{

    public Image            mBar;
    public Text             mScoreTxt;

    private bool            mIsWindingUp = false;

    [SerializeField]
    private GameObject          RefFootballPathNode;

    [SerializeField]
    private SO_Transform        RefPlayerPos;
    [SerializeField]
    private SO_Transform        RefPlayerCamera;

    [SerializeField]
    private DT_Player           PlayerData;         // used for max throw power at a minimum
    [SerializeField]
    private SO_Float            GB_CurThrowChrg;

    private float               score = 0f;

    void Update()
    {
        // render balls along trajectory
        if(mIsWindingUp){
            ShowThrowBar();
        }

    }

    public void ShowThrowBar()
    {
        mBar.fillAmount = GB_CurThrowChrg.Val;
    }

    public void QB_Charging(){
        mIsWindingUp = true;
    }

    public void QB_ThrewBall(){
        mIsWindingUp = false;
        mBar.fillAmount = 0f;
    }

    public void REC_TargetHit(){
        score += 100f;
        mScoreTxt.text = "Score: " + score;
    }
    
}
