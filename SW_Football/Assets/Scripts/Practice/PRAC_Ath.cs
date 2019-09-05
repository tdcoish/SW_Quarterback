/*************************************************************************************
Just stores the damn info for the play.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;


public class PRAC_Ath : MonoBehaviour
{
    public enum PRAC_ATH_STATE
    {
        SPRE_SNAP,
        SDOING_JOB,
        SPOST_PLAY
    }
    public PRAC_ATH_STATE           mState;

    private Rigidbody               cRigid;
    private PRAC_AI_Acc             cAcc;


    public DT_PlayerRole            mJob;

    void Awake()
    {
        cRigid = GetComponent<Rigidbody>();
        cAcc = GetComponent<PRAC_AI_Acc>();
        mState = PRAC_ATH_STATE.SPRE_SNAP;
    }

    void Update()
    {        
        switch(mState)
        {
            case PRAC_ATH_STATE.SPRE_SNAP: RUN_PreSnap(); break;
            case PRAC_ATH_STATE.SDOING_JOB: RUN_Job(); break;
            case PRAC_ATH_STATE.SPOST_PLAY: RUN_PostPlay(); break;
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

    // Again, nothing. Eventually some animations or something.
    protected virtual void RUN_PostPlay()
    {

    }

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
            // The direction from them to us.
            Vector3 vDirThemToUs = transform.position - collision.transform.position;
            vDirThemToUs = Vector3.Normalize(vDirThemToUs);

            // Now get the momentum they have.
            float mWgt = 100f;          // just giving everyone 100 lbs for now.

            Vector3 vTheirVelIntoUs = vDirThemToUs * Vector3.Dot(collision.rigidbody.velocity, vDirThemToUs);
            Vector3 vOurVelAwayFromThem = vDirThemToUs * Vector3.Dot(cRigid.velocity, vDirThemToUs);

            Vector3 vTheirRelativeVelIntoUs = vTheirVelIntoUs - vOurVelAwayFromThem;

            if(Vector3.Dot(vTheirRelativeVelIntoUs, vDirThemToUs) < 0f)
            {
                Debug.Log("Probably due to ordering, they are not moving into us");
                return;
            }

            // although we do have to give a little more or we might have elastic collisions.
            Vector3 vTheirForceIntoUs = vTheirRelativeVelIntoUs * mWgt;
            
            vTheirForceIntoUs /= mWgt;                  // simulating our weight dampening a blow.

            vTheirForceIntoUs *= 100f;           // debugging.

            cRigid.velocity += vTheirForceIntoUs;
        }
    }

    // We call this on the opposing athlete, as well.
    public void FChangeVelAfterCol(PRAC_Ath other)
    {
        
    }
}
