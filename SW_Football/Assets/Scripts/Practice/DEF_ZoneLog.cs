﻿/*************************************************************************************
Logic for zone.
Figure out how to code breaking on the ball.
*************************************************************************************/
using UnityEngine;

[RequireComponent(typeof(AI_Acc))]
[RequireComponent(typeof(PRAC_Ath))]
public class DEF_ZoneLog : MonoBehaviour
{
    private Rigidbody           cRigid;
    private PRAC_Ath            cAth;
    private PRAC_AI_Acc         cAcc;
    private RP_CatchLog         cCatchLog;

    public enum STATE{
        S_GETTING_TO_SPOT,
        S_READING_QB,
        S_CATCHING_BALL_TRY,
        S_MAN_UP
    }
    public STATE                mState;

    public Vector3              mZoneSpot;

    // Maybe have this as a function that we call.
    void Start()
    {   
        cRigid = GetComponent<Rigidbody>();
        cAth = GetComponent<PRAC_Ath>();
        cAcc = GetComponent<PRAC_AI_Acc>();
        cCatchLog = GetComponent<RP_CatchLog>();

        if(cAth.mJob.mRole == "Zone")
        {
            mZoneSpot = IO_ZoneList.FLOAD_ZONE_BY_NAME(cAth.mJob.mDetail).mSpot;
            mZoneSpot.z = mZoneSpot.y;
            mZoneSpot.y = 0f;
            PLY_SnapSpot snap = FindObjectOfType<PLY_SnapSpot>();
            mZoneSpot += snap.transform.position;
        }

        mState = STATE.S_GETTING_TO_SPOT;
    }

    public void FRunZone()
    {
        switch(mState)
        {
            case STATE.S_GETTING_TO_SPOT: RUN_GetToSpot(); break;
            case STATE.S_READING_QB: RUN_ReadQBEyes(); break;
            case STATE.S_MAN_UP: RUN_ManUp(); break;
            case STATE.S_CATCHING_BALL_TRY: RUN_TryCatchBall(); break;
        }
    }

    // At some point, if they get close, then have them try to cheat 
    // to the QB.
    private void RUN_GetToSpot()
    {
        // make it run to it's zone spot every time.
        Vector3 dis = mZoneSpot - transform.position;
        dis.y = 0f;
        Vector3 disNorm = Vector3.Normalize(dis);

        Vector3 vAcc = cAcc.FCalcAccFunc(disNorm, cAcc.mSpd);
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > cAcc.mSpd){
            cRigid.velocity *= cAcc.mSpd/cRigid.velocity.magnitude;
        }
        transform.forward = cRigid.velocity.normalized;
        
        if(cAth.FCheckIfBallThrown()){
            ENTER_TryCatchBall();
        }

        if(dis.magnitude < 10f){
            ENTER_ReadQBEyes();
        }
    }

    private void ENTER_ReadQBEyes(){
        mState = STATE.S_READING_QB;
    }
    private void RUN_ReadQBEyes()
    {
        PC_Controller rPC = FindObjectOfType<PC_Controller>();
        Vector3 vPlayerDir = rPC.GetComponentInChildren<PC_Camera>().transform.forward;

        // now we stay relatively close to our zone spot, but we "cheat" a little towards where the player is looking.
        // I can just assume that the depth should stay the same, for now.
        // also, if they aren't even looking close to us, don't do anything.
        Vector3 vDisPlayerToZone = mZoneSpot - rPC.transform.position;
        float fDot = Vector3.Dot(Vector3.Normalize(vDisPlayerToZone), vPlayerDir);

        if(fDot < 0.8f){
            Vector3 dis = mZoneSpot - transform.position;
            dis.y = 0f;
            Vector3 disNorm = Vector3.Normalize(dis);

            Vector3 vAcc = cAcc.FCalcAccFunc(disNorm, cAcc.mSpd);
            cRigid = cAth.FApplyAccelerationToRigidbody(cRigid, vAcc, cAcc.mSpd);
            transform.forward = cRigid.velocity.normalized;
        }else{
            Vector3 vCheatSpot = rPC.transform.position + vDisPlayerToZone.magnitude * vPlayerDir;  
            // can't cheat too much here.
            float fCheatDis = (vDisPlayerToZone.magnitude / 10f) * 2f;      // every 10 yards, you can cheat x more yards.
            if(Vector3.Distance(vCheatSpot, mZoneSpot) > fCheatDis)
            {
                Vector3 vDisToCheatSpot = vCheatSpot - mZoneSpot;
                vCheatSpot = mZoneSpot + vDisToCheatSpot.normalized * 2f;
            }
            Vector3 dis = vCheatSpot - transform.position; dis.y = 0f;
            Vector3 disNorm = Vector3.Normalize(dis);

            // ------- need to, just like catching, prematurely slow down if going too fast.
            // ------- Turns out, we need to know how quickly we can decelerate, or we can't calc this.
            // ------- Update. Just pretend that they need to be there in 0.2 seconds.
            Vector3 vAcc = new Vector3();
            float fDotVel = Vector3.Dot(cRigid.velocity, dis);
            if(fDotVel > 0f){       // otherwise time is infinite.
                float fTime = 0.2f;
                Vector3 vIdealVel = dis / fTime;
                Vector3 vAccDir = vIdealVel - cRigid.velocity;
                vAccDir = Vector3.Normalize(vAccDir);
                vAcc = cAcc.FCalcAccFunc(vAccDir, cAcc.mSpd/2f);
            }else{
                vAcc = cAcc.FCalcAccFunc(disNorm, cAcc.mSpd/2f);
            }
            // pretend that strafing speed is a lot lower.
            cRigid = cAth.FApplyAccelerationToRigidbody(cRigid, vAcc, cAcc.mSpd/2f);
            transform.forward = vDisPlayerToZone.normalized;
        }

        float fDepth = vDisPlayerToZone.magnitude;

        if(cAth.FCheckIfBallThrown()){
            ENTER_TryCatchBall();
        }

    }
    private void RUN_ManUp(){}
    private void RUN_TryCatchBall(){
        Vector3 vSpotToGetTo = cCatchLog.FCalcInterceptSpot(0.5f);
        Vector3 dis = vSpotToGetTo - transform.position;
        dis.y = 0f;
        float airTime = cCatchLog.FCalcInterceptTime(0.5f);
        // since I know how far I need to go, I also know the exact velocity I should be using.
        Vector3 vVel = dis / airTime;

        // Now subtract our current velocity, and we have the direction to accelerate in.
        Vector3 vAccDir = vVel - cRigid.velocity;
        vAccDir = Vector3.Normalize(vAccDir);

        Vector3 vAcc = cAcc.FCalcAccFunc(vAccDir, cAcc.mSpd);
        cRigid.velocity += vAcc;
        if(cRigid.velocity.magnitude > cAcc.mSpd){
            cRigid.velocity *= cAcc.mSpd/cRigid.velocity.magnitude;
        }
        transform.forward = cRigid.velocity.normalized;
    }

    private void ENTER_TryCatchBall(){
        mState = STATE.S_CATCHING_BALL_TRY;
    }

}
