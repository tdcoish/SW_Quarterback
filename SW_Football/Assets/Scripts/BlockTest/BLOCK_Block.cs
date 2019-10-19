/*************************************************************************************
Okay this guy just chills, but then he has to defend against rusher moves.
If he's not stunned, then he always blocks perfectly.
*************************************************************************************/
using UnityEngine;

public class BLOCK_Block : MonoBehaviour
{
    public enum STATE{
        S_Chill,
        S_Engaged,
        S_Stunned,
        S_Exploded
    }
    public STATE                            mState;
    private float                           mStunMoment;
    public float                            mStunnedDuration = 1f;

    public float                            mEngageDistance = 3f;

    public BLOCK_Rush                       rRusher;

    public ParticleSystem                   mParticles;

    public float                            mMoveDefMin = 50f;
    public float                            mMoveDefMax = 100f;

    void Start()
    {
        mParticles.gameObject.SetActive(false);
        mState = STATE.S_Chill;
    }

    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        switch(mState){
            case STATE.S_Chill: RUN_Chill(); break;
            case STATE.S_Engaged: RUN_Engaged(); break;
            case STATE.S_Stunned: RUN_Stunned(); break;
            case STATE.S_Exploded: RUN_Exploded(); break;
        }
    }
    private void RUN_Chill(){
        if(Vector3.Distance(transform.position, rRusher.transform.position) < mEngageDistance){
            mState = STATE.S_Engaged;
        }
    }
    private void RUN_Engaged(){
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
    public void FGetBullRushed()
    {
        
    }
}
