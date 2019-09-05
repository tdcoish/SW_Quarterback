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
}
