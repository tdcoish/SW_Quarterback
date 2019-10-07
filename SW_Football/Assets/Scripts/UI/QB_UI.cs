/********************************************************************************************
This should be the basics of our player UI, across many game modes. We are going to need to add
state of course.

Some cleanup to do.

BTW, we can enable and disable things UI elements simply using .enabled = false.
eg.        mBar.enabled = false;

Could probably have multi-layered state. Visible/Invisible, then different state for the specifics
of what's going on.

Yep, we're bringing back the maximum throw power. This is because 

Could just store like the last 10 frames of looking around, and then use that to sort of show looking around 
inaccuracy?
******************************************************************************************* */

using UnityEngine;
using UnityEngine.UI;

public class ForwardVecStorer
{
    private int                 mNumVecs;
    public int                  mSize;
    private int                 mInd;
    public Vector3[]            mForwardVecsOverTime;           // basically this is where the camera is facing over time.

    public ForwardVecStorer(int size)
    {
        mSize = size;
        mNumVecs = 0;
        mInd = 0;
        mForwardVecsOverTime = new Vector3[size];
    }

    public void FStoreDirection(Vector3 vForward)
    {
        mForwardVecsOverTime[mInd++] = vForward;
        if(mInd >= mSize){
            mInd = 0;
        }
        mNumVecs++;
    }

    public float FGetDotOldFrames()
    {
        if(mNumVecs < mSize){
            return 1.0f;
        }else{
            // mInd is always on the most out of date vector.
            int curInd = mInd - 1;
            if(curInd < 0){
                curInd = mSize-1;
            }
            Vector3 vCur = mForwardVecsOverTime[curInd];
            Vector3 vOld = mForwardVecsOverTime[mInd];
            return Vector3.Dot(vCur, vOld);
        }
    }
}

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
    public Image                mAccuracy;
    public Image                mStaticInaccuraccy;         // even when we're not throwing, show the player sort of how accuracy is affected.

    private ForwardVecStorer    mLookStorer;

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

        mLookStorer = new ForwardVecStorer(50);

        TDC_EventManager.FAddHandler(TDC_GE.GE_QB_StopThrow, E_StopCharging);
        TDC_EventManager.FAddHandler(TDC_GE.GE_QB_ReleaseBall, E_StopCharging);
        TDC_EventManager.FAddHandler(TDC_GE.GE_QB_StartWindup, E_StartCharging);
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
        ShowThrowBar();
        // Always show the maximum throw power bar.
        mThrowMaxBar.fillAmount = GB_ThrowMaxCharge.Val / IO_Settings.mSet.lPlayerData.mThrowSpd;

        // How much should inaccuracy scale the image? Let's say that an inaccuracy of 1 degree is the norm, so we scale proportionally after that.
        float fCrossScale = GB_ThrowInaccuracy.Val;
        if(fCrossScale == 0f){
            fCrossScale = 0.01f;
        }
        mAccuracy.transform.localScale = new Vector3(fCrossScale, fCrossScale, fCrossScale);

        // ----------------------------------------- Look accuracy stuffs.
        mLookStorer.FStoreDirection(FindObjectOfType<PC_Controller>().GetComponentInChildren<PC_Camera>().transform.forward);
        float fLookDot = 1f - mLookStorer.FGetDotOldFrames();
        fLookDot *= IO_Settings.mSet.lLookPenalty / 5f;
        float fSpdPct = FindObjectOfType<PC_Controller>().GetComponent<Rigidbody>().velocity.magnitude/IO_Settings.mSet.lPlayerData.mMoveSpd;
        float fMoveInacc = fSpdPct * IO_Settings.mSet.lMovementPenalty;
        float fTotalInacc = fLookDot + fMoveInacc;
        mStaticInaccuraccy.transform.localScale = new Vector3(fTotalInacc, fTotalInacc, fTotalInacc);
    }

    public void ShowThrowBar()
    {
        mBar.fillAmount = GB_ThrowCharge.Val;
    }

    public void E_StartCharging(){
        mState = QB_UI_STATE.SCHARGING;
    }

    public void E_StopCharging(){
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
