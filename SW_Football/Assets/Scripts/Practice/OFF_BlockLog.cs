/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class OFF_BlockLog : MonoBehaviour
{
    private Rigidbody           cRigid;

    public enum STATE{
        S_Chill,
        S_Engaged,
        S_Stunned,
        S_Exploded
    }
    public STATE                            mState;

    public ParticleSystem                   mParticles;

    private float                           mStunMoment;
    public float                            mStunnedDuration = 1f;
    public float                            mMoveDefMin = 50f;
    public float                            mMoveDefMax = 100f;
    public float                            mEngageDistance = 1f;

    private void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        mParticles.gameObject.SetActive(false);
        mState = STATE.S_Chill;
    }

    // Call this when the play is actually running.
    public void FRunBlocking()
    {
        switch(mState){
            case STATE.S_Chill: RUN_Chill(); break;
            case STATE.S_Engaged: RUN_Engaged(); break;
            case STATE.S_Stunned: RUN_Stunned(); break;
            case STATE.S_Exploded: RUN_Exploded(); break;
        }
    }

    // Slowly move back to the quarterback.
    private void RUN_Chill(){
        DEF_RushLog rRusher = FuncGetClosestRusher(FindObjectsOfType<PRAC_Def_Ply>(), transform.position);
        if(Vector3.Distance(transform.position, rRusher.transform.position) < mEngageDistance){
            mState = STATE.S_Engaged;
        }

        Vector3 vDir = Vector3.Normalize(FindObjectOfType<PC_Controller>().transform.position - transform.position);
        cRigid.velocity = vDir * 0.2f;
        transform.forward = vDir * -1f;
    }
    private void RUN_Engaged(){
        DEF_RushLog rRusher = FuncGetClosestRusher(FindObjectsOfType<PRAC_Def_Ply>(), transform.position);
        Vector3 vDis = rRusher.transform.position - transform.position;
        transform.forward = vDis.normalized;
    }   
    private void RUN_Stunned(){
        if(Time.time - mStunMoment > mStunnedDuration){
            mState = STATE.S_Chill;
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
