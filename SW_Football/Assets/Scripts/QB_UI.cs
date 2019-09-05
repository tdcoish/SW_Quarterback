/********************************************************************************************
This should be the basics of our player UI, across many game modes. We are going to need to add
state of course.

Some cleanup to do.

BTW, we can enable and disable things UI elements simply using .enabled = false.
eg.        mBar.enabled = false;

Could probably have multi-layered state. Visible/Invisible, then different state for the specifics
of what's going on.

Yep, we're bringing back the maximum throw power. This is because 
******************************************************************************************* */

using UnityEngine;
using UnityEngine.UI;

// Want to draw little bar representing the maximum "point" of the current throw.
public class QB_UI : MonoBehaviour
{

    public enum QB_UI_STATE
    {
        SINVISIBLE,
        SCHARGING,
        SNOTCHARGING
    }
    public QB_UI_STATE          mState;

    public Image                mBar;
    public Image                mThrowMaxBar;
    public Image                mCrosshairs;

    [SerializeField]
    private SO_Float            GB_ThrowCharge;
    [SerializeField]
    private SO_Float            GB_ThrowMaxCharge;

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
        if(mState == QB_UI_STATE.SINVISIBLE)
        {
            TransitionToInvisible();
            return;
        }
        TransitionToVisible();

        // render balls along trajectory
        if(mState == QB_UI_STATE.SCHARGING){
            ShowThrowBar();
        }
        // Always show the maximum throw power bar.
        mThrowMaxBar.fillAmount = GB_ThrowMaxCharge.Val / IO_Settings.mSet.lPlayerData.mThrowSpd;
        mThrowMaxBar.fillAmount = 1f;

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

    // QB_Start_Charging
    public void QB_Charging(){
        mState = QB_UI_STATE.SCHARGING;
    }

    public void QB_ThrewBall(){
        mState = QB_UI_STATE.SNOTCHARGING;
        mBar.fillAmount = 0f;
    }

    private void TransitionToVisible()
    {
        mBar.enabled = true;
        mCrosshairs.enabled = true;
    }
    private void TransitionToInvisible()
    {
        mBar.enabled = false;
        mCrosshairs.enabled = false;
    }
    
}
