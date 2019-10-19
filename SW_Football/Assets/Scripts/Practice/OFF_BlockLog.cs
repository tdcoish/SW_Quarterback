/*************************************************************************************
Set refers to when they are getting the initial distance/positioning.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class OFF_BlockLog : MonoBehaviour
{
    private Rigidbody           cRigid;

    public enum STATE{
        S_Chill,
        S_Set,
        S_Engaged,
        S_Stunned,
        S_Exploded
    }
    public STATE                            mState;

    public ParticleSystem                   mParticles;

    private float                           mPercentKickOut;            // for tackles this is very high, for centers it's 0.

    private float                           mStunMoment;
    public float                            mStunnedDuration = 1f;
    public float                            mMoveDefMin = 50f;
    public float                            mMoveDefMax = 100f;
    public float                            mEngageDistance = 1f;

    private Vector3                         rSnapSpot;
    private Vector3                         rRunAroundSpot;             // because they arc around a spot about 3 yards back.
    private float                           mKickStartTime;
    private float                           mKickTimeLength;

    private void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        mParticles.gameObject.SetActive(false);
        mState = STATE.S_Chill;

        rSnapSpot = FindObjectOfType<PLY_SnapSpot>().transform.position;
        rRunAroundSpot = rSnapSpot;
        rRunAroundSpot.z -= 2f;
    }

    // Call this when the play is actually running.
    public void FRunBlocking()
    {
        switch(mState){
            case STATE.S_Chill: RUN_Chill(); break;
            case STATE.S_Set: RUN_Set(); break;
            case STATE.S_Engaged: RUN_Engaged(); break;
            case STATE.S_Stunned: RUN_Stunned(); break;
            case STATE.S_Exploded: RUN_Exploded(); break;
        }
    }

    private void RUN_Chill(){
        ENTER_Set();
    }
    private void ENTER_Set()
    {
        mState = STATE.S_Set;
        float fDis = Vector3.Distance(rSnapSpot, transform.position);
        mPercentKickOut = fDis/4f;
        if(mPercentKickOut > 1f){
            Debug.Log("Perc was too high");
            mPercentKickOut = 1f;
        } 
        mKickStartTime = Time.time;
        mKickTimeLength = 1f + mPercentKickOut * 1.5f;
    }

    // pass blockers also try to maintain distance to snap. However, we want to bias to kicking outside a little bit.
    // Solution is just have them do this for a second or two, then they move straight back to the QB.
    // Should depend on how far they start from the snap.
    // Calculate path straight to QB, calc path in arc. Take percentage.
    private void RUN_Set(){
        DEF_RushLog rRusher = FuncGetClosestRusher(FindObjectsOfType<PRAC_Def_Ply>(), transform.position);
        if(Vector3.Distance(transform.position, rRusher.transform.position) < mEngageDistance){
            mState = STATE.S_Engaged;
        }

        // ---------------------- If they've gone far enough, then just have them stop so they don't crowd the QB.
        if(Time.time - mKickStartTime > mKickTimeLength){
            cRigid.velocity = Vector3.zero;
        }else{
            // ---------------------- arc path.
            Vector3 vDirToQB = Vector3.Normalize(FindObjectOfType<PC_Controller>().transform.position - transform.position);
            Vector3 vDirToSnap = Vector3.Normalize(rRunAroundSpot - transform.position);
            Vector3 vSnapQBCross = Vector3.Cross(vDirToSnap, vDirToQB);
            Vector3 vBlockDir = Vector3.Cross(vSnapQBCross, vDirToSnap);

            // --------------------- percentage path/blend between
            Vector3 vActualPath = vBlockDir * mPercentKickOut + vDirToQB * (1f-mPercentKickOut);
            float fSpd = 0.5f + mPercentKickOut*2f;
            cRigid.velocity = vActualPath * fSpd;
        }


        // but we want them looking at the rusher.
        Vector3 vDirToRusher = Vector3.Normalize(rRusher.transform.position - transform.position);
        // transform.forward = vDirToRusher;
    }
    private void RUN_Engaged(){
        DEF_RushLog rRusher = FuncGetClosestRusher(FindObjectsOfType<PRAC_Def_Ply>(), transform.position);
        Vector3 vDis = rRusher.transform.position - transform.position;
        transform.forward = vDis.normalized;
    }   
    private void RUN_Stunned(){
        if(Time.time - mStunMoment > mStunnedDuration){
            mState = STATE.S_Set;
            mParticles.gameObject.SetActive(false);
        }
    }
    private void RUN_Exploded(){}

    public void FGetFinessed()
    {
        mParticles.gameObject.SetActive(true);
        mStunMoment = Time.time;
        mState = STATE.S_Stunned;
    }

    private DEF_RushLog FuncGetClosestRusher(PRAC_Def_Ply[] defenders, Vector3 ourPos)
    {
        List<DEF_RushLog> rushers = new List<DEF_RushLog>();
        foreach(PRAC_Def_Ply d in defenders)
        {
            if(d.mJob.mRole == "Pass Rush"){
                rushers.Add(d.GetComponent<DEF_RushLog>());
            }
        }

        if(rushers.Count == 0){
            return null;
        }

        float fDis = Vector3.Distance(rushers[0].transform.position, ourPos);
        int ixClose = 0;
        for(int i=1; i<rushers.Count; i++){
            float temp = Vector3.Distance(rushers[1].transform.position, ourPos);
            if(temp < fDis){
                fDis = temp;
                ixClose = i;
            }
        }

        return rushers[ixClose];
    }
}
