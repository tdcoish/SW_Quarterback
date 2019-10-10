/*************************************************************************************
Just stores the damn info for the play.

Actual way collisions need to be done, is the next frame. Oh but wait, we need to store their
velocity and weight (and probably like 10 other things before we're done).

Okay, they have a new role called "run with ball" wherein they just straight up run to
the endzone.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

// Hitter is the thing that hit us last frame. We need to store it's weight and velocity.
[System.Serializable]
public struct DATA_Hitter
{
    public float               mWgt;
    public Vector3             mVel;
    public Vector3             mPos;
}

public class PRAC_Ath : MonoBehaviour
{
    public enum PRAC_ATH_STATE
    {
        SPRE_SNAP,
        SDOING_JOB,
        SRUN_WITH_BALL,
        SPOST_PLAY
    }
    public PRAC_ATH_STATE           mState;

    private Rigidbody               cRigid;
    private PRAC_AI_Acc             cAcc;
    protected AD_Athletes           cAud;

    public bool                     mHitLastFrame = false;
    public DATA_Hitter              dThingThatHitUs;

    public DT_PlayerRole            mJob;

    void Awake()
    {
        cRigid = GetComponent<Rigidbody>();
        cAcc = GetComponent<PRAC_AI_Acc>();
        cAud = GetComponentInChildren<AD_Athletes>();
        mState = PRAC_ATH_STATE.SPRE_SNAP;
    }

    void Update()
    {        
        switch(mState)
        {
            case PRAC_ATH_STATE.SPRE_SNAP: RUN_PreSnap(); break;
            case PRAC_ATH_STATE.SDOING_JOB: RUN_Job(); break;
            case PRAC_ATH_STATE.SRUN_WITH_BALL: RUN_RunWithBall(); break;
            case PRAC_ATH_STATE.SPOST_PLAY: RUN_PostPlay(); break;
        }

        if(mHitLastFrame)
        {
            HandleGettingBumped();
            mHitLastFrame = false;
        }

    }

    protected virtual void RUN_PreSnap()
    {
        // Actually we just don't really do anything here, for now.
        cRigid.velocity = Vector3.zero;
    }

    protected virtual void RUN_Job()
    {
        
    }

    // Again, nothing. Maybe they can slowly walk back to the snap?
    protected virtual void RUN_PostPlay()
    {
        PLY_SnapSpot s = FindObjectOfType<PLY_SnapSpot>();
        if(s != null){
            Vector3 vDis = transform.position - s.transform.position;
            vDis = Vector3.Normalize(vDis);
            cRigid.velocity = vDis * 0.1f;
        }else{
            cRigid.velocity = Vector3.zero;
        }
    }

    protected virtual void RUN_RunWithBall(){}

    public virtual void FCaughtBall(){}

    /******************************************************************
    You hit me with your weight*differenceInVelocity. If I am going 5m/s north,
    and you are going 4m/s north, you hit me at 1m/s * your weight.

    Component of their velocity into us, minus the component of our velocity, in that 
    direction.

    Due to ordering effects, we should in fact change their velocity for them. Yes, this does
    have some bugs (they get effected twice per frame), but it makes the collisions
    "fair", as in, it doesn't matter who gets collided with first.
    ******************************************************************/
    void OnCollisionStay(Collision collision)
    {
        if(collision.transform.GetComponent<PRAC_Ath>() != null)
        {
            dThingThatHitUs.mPos = collision.transform.position;
            dThingThatHitUs.mVel = collision.rigidbody.velocity;
            dThingThatHitUs.mWgt = 100f;            // cause fuck it.
            mHitLastFrame = true;
        }
    }

    private void HandleGettingBumped()
    {
        // The direction from them to us.
        Vector3 vDirThemToUs = transform.position - dThingThatHitUs.mPos;
        vDirThemToUs = Vector3.Normalize(vDirThemToUs);

        // Now get the momentum they have.
        float mWgt = 100f;          // just giving everyone 100 lbs for now.

        Vector3 vTheirVelIntoUs = vDirThemToUs * Vector3.Dot(dThingThatHitUs.mVel, vDirThemToUs);
        Vector3 vOurVelAwayFromThem = vDirThemToUs * Vector3.Dot(cRigid.velocity, vDirThemToUs);

        Vector3 vTheirRelativeVelIntoUs = vTheirVelIntoUs - vOurVelAwayFromThem;

        if(Vector3.Dot(vTheirRelativeVelIntoUs, vDirThemToUs) < 0f)
        {
            return;
        }

        // although we do have to give a little more or we might have elastic collisions.
        Vector3 vTheirForceIntoUs = vTheirRelativeVelIntoUs * mWgt;
        
        vTheirForceIntoUs /= mWgt;                  // simulating our weight dampening a blow.

        // vTheirForceIntoUs *= 100f;           // debugging.

        cRigid.velocity += vTheirForceIntoUs;
    }

    public Rigidbody FApplyAccelerationToRigidbody(Rigidbody rigid, Vector3 vAcc, float fMaxSpd)
    {
        rigid.velocity += vAcc;
        if(rigid.velocity.magnitude > fMaxSpd){
            rigid.velocity *= fMaxSpd/rigid.velocity.magnitude;
        }

        return rigid;
    }

    public bool FCheckIfBallThrown()
    {
        PROJ_Football f = FindObjectOfType<PROJ_Football>();
        if(f!=null){
            return true;
        }

        return false;
    }

}
