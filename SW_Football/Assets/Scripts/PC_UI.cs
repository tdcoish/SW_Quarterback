using UnityEngine;
using UnityEngine.UI;

// Want to draw little bar representing the maximum "point" of the current throw.
public class PC_UI : MonoBehaviour
{

    public Image            mBar;
    public Image            mMaxLine;
    public Text             mScoreTxt;

    // if the pc_controller is winding up.
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
    private SO_Float            CurrentThrowMaxCharge;      // they could hit ctrl to take something off of it.
    [SerializeField]
    private SO_Float            CurThrowPwr;

    private float               score = 0f;

    // Start is called before the first frame update
    void Start()
    {
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

    public void REC_TargetHit(){
        score += 100f;
        mScoreTxt.text = "Score: " + score;
    }
    
}
