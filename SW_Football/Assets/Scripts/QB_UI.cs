/********************************************************************************************
This should be the basics of our player UI, across many game modes. We are going to need to add
state of course.

Some cleanup to do.
******************************************************************************************* */

using UnityEngine;
using UnityEngine.UI;

// Want to draw little bar representing the maximum "point" of the current throw.
public class QB_UI : MonoBehaviour
{

    public enum QB_UI_STATE
    {
        SCHARGING,
        SNOTCHARGING
    }
    public QB_UI_STATE          mState;

    public Image                mBar;
    public Image                mCrosshairs;

    [SerializeField]
    private DT_Player           PlayerData;         // used for max throw power at a minimum
    [SerializeField]
    private SO_Float            GB_ThrowCharge;

    // innaccuracy is dependent on our movement alone. Throw innaccuracy is the cumulative innaccuracy of the current throw.
    public SO_Float             GB_MoveInaccuracy;
    public SO_Float             GB_ThrowInaccuracy;
    public SO_Float             GB_ThrowLookInaccuracy;

    // Start is called before the first frame update
    void Start()
    {
        mState = QB_UI_STATE.SNOTCHARGING;
        mBar.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // render balls along trajectory
        if(mState == QB_UI_STATE.SCHARGING){
            ShowThrowBar();
        }

        // How much should inaccuracy scale the image? Let's say that an inaccuracy of 1 degree is the norm, so we scale proportionally after that.
        float fCrossScale = GB_ThrowInaccuracy.Val;
        if(fCrossScale < 2f){
            fCrossScale = 2f;
        }
        mCrosshairs.transform.localScale = new Vector3(fCrossScale,fCrossScale,fCrossScale);
    }

    public void ShowThrowBar()
    {
        mBar.fillAmount = GB_ThrowCharge.Val;
    }

    public void QB_Charging(){
        mState = QB_UI_STATE.SCHARGING;
    }

    public void QB_ThrewBall(){
        mState = QB_UI_STATE.SNOTCHARGING;
        mBar.fillAmount = 0f;
    }
    
}
