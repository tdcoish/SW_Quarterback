﻿/*************************************************************************************
Logic for pass rush.

When engaged, for now they can only bull rush. They take pushes, and they receive pushes,
but we don't apply Newtons second to their pushes, it just happens.

Net acceleration is then provided by our leftover force added into our weight.

New system. We run our own rush, sure, but a player manager figures out who's pushing on whom.

Alright, I'm stealing the easier system from 

They need to rush "in their lanes" instead of straight at the QB. That means that they
are going to sort of make a type of circle. The way to do this is to give a player a "radius"
to rush. If they start x meters from the snap, they should rush in an arc that keeps them
that distance from the snap. That also lets the QB step up into the pocket. 
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class DEF_RushLog : MonoBehaviour
{
    private PRAC_Ath            cAth;
    private Rigidbody           cRigid;
    private ATH_Forces          cForces;

    public float                mEngageDis = 1f;
    public float                mMoveCooldown = 0.5f;
    public float                mLastMoveTime;
    public float                mMoveScore = 80f;

    private Vector3             rSnapSpot;

    // Maybe have this as a function that we call.
    void Start()
    {   
        cRigid = GetComponent<Rigidbody>();
        cAth = GetComponent<PRAC_Ath>();
        cForces = GetComponent<ATH_Forces>();

        rSnapSpot = FindObjectOfType<PLY_SnapSpot>().transform.position;
    }

    // We just straight up run to the quarterback.
    public void FRunRush()
    {

        // -------------------- We always need references to all the blockers, as well as the PC.
        PC_Controller rPC = FindObjectOfType<PC_Controller>();
        if(rPC == null){
            cRigid.velocity = Vector3.zero;
            return;
        }
        
        // ------------------ Tackle the QB. Shouldn't work if the ball has not been thrown yet.
        Vector3 vDis = rPC.transform.position - transform.position;
        // Debug.DrawLine(rPC.transform.position, transform.position, Color.green);
        if(vDis.magnitude < 1f){
            TDC_EventManager.FBroadcast(TDC_GE.GE_Sack);        
            return;
        }

        PRAC_Off_Ply[] athlets = FindObjectsOfType<PRAC_Off_Ply>();
        List<OFF_BlockLog> blockers = new List<OFF_BlockLog>();
        foreach(PRAC_Off_Ply a in athlets){
            if(a.mRole == "BLOCK"){
                blockers.Add(a.GetComponent<OFF_BlockLog>());
            }
        }
        if(blockers.Count == 0){
            RUN_FreeRun(rPC);
            return;
        }


        // ---------------- Okay, now we see if there is a guy blocking us.
        // For now just pick the closest one to us.
        OFF_BlockLog closestBlocker = FuncClosestBlocker(blockers, transform.position);
        float fDisToBlocker = Vector3.Distance(transform.position, closestBlocker.transform.position);
        bool blockerInFront = FuncBlockerInFront(transform.position, closestBlocker.transform.position, transform.forward);
        if(fDisToBlocker < mEngageDis && closestBlocker.mState != OFF_BlockLog.STATE.S_Stunned && blockerInFront){
            RUN_Engage(closestBlocker, rPC);
        }else if (blockerInFront){
            RUN_RushLane(rPC);
        }else{
            RUN_FreeRun(rPC);
        }
        
    }

    public void RUN_Engage(OFF_BlockLog rBlocker, PC_Controller rPC)
    {
        cRigid.velocity = Vector3.zero;
        if(Time.time - mLastMoveTime > mMoveCooldown)
        {
            // alright now we try to beat the blocker.
            mLastMoveTime = Time.time;
            
            if(FuncCalcMoveSuccess(mMoveScore, rBlocker.mMoveDefMin, rBlocker.mMoveDefMax)){
                rBlocker.FGetFinessed();
                Vector3 vNewPos = transform.position;
                // here we want to know if we should go to our left, or our right.
                if(FuncGoRight(transform.right, transform.position, rPC.transform.position)){
                    vNewPos += transform.right * 1f;
                }else{
                    vNewPos += transform.right * -1f;
                }
                transform.position = vNewPos;
            }
        }

        transform.forward = Vector3.Normalize(rBlocker.transform.position - transform.position);
    }

    // Now rushing in an arc.
    public void RUN_FreeRun(PC_Controller rPC)
    {
        transform.forward = FuncAngleToSpot(transform.position, rPC.transform.position);
        Vector3 vDis = rPC.transform.position - transform.position;
        vDis = Vector3.Normalize(vDis);
        cRigid.velocity = vDis;
    }

    private void RUN_RushLane(PC_Controller rPC)
    {
        Vector3 vDirToSnap = FuncAngleToSpot(transform.position, rSnapSpot); vDirToSnap.y = 0f;
        Vector3 vDirToQB = FuncAngleToSpot(transform.position, rPC.transform.position); vDirToQB.y = 0f;
        Vector3 vSnapQbCross = Vector3.Cross(vDirToSnap, vDirToQB);     // will be either straight up or straight down.
        Vector3 vRushDir = Vector3.Cross(vDirToSnap, vSnapQbCross) * -1f;
        // cRigid.velocity = vDirToSnap.normalized;
        cRigid.velocity = vRushDir.normalized * 2f;
        Debug.DrawLine(transform.position, (transform.position + vRushDir), Color.red);
    }

    private OFF_BlockLog FuncClosestBlocker(List<OFF_BlockLog> blockers, Vector3 ourPos)
    {
        int ixClose = 0;
        float fDis = Vector3.Distance(blockers[0].transform.position, ourPos);
        for(int i=1; i<blockers.Count; i++){
            float temp = Vector3.Distance(blockers[i].transform.position, ourPos);
            if(temp < fDis){
                fDis = temp;
                ixClose = i;
            }
        }

        return blockers[ixClose];
    }

    private bool FuncBlockerInFront(Vector3 ourPos, Vector3 blockerPos, Vector3 ourFacingDir, float minAngle = 0f)
    {
        Vector3 vDis = blockerPos - ourPos;
        vDis = Vector3.Normalize(vDis);
        ourFacingDir = Vector3.Normalize(ourFacingDir);
        if(Vector3.Dot(ourFacingDir, vDis) > minAngle){
            return true;
        }
        return false;
    }

    private Vector3 FuncAngleToSpot(Vector3 ourPos, Vector3 spot)
    {
        Vector3 vDis = spot - ourPos;
        vDis = Vector3.Normalize(vDis);
        return vDis;
    }

    private bool FuncCalcMoveSuccess(float moveScore, float moveDefMin, float moveDefMax)
    {
        float defScore = Random.Range(moveDefMin, moveDefMax);
        if(defScore > moveScore){
            return true;
        }else{
            return false;
        }
    }

    private bool FuncGoRight(Vector3 dOurRight, Vector3 ourPos, Vector3 qbPos)
    {
        Vector3 vDisToQB = qbPos - ourPos;
        vDisToQB = Vector3.Normalize(vDisToQB);
        float dot = Vector3.Dot(vDisToQB, dOurRight);
        if(dot > 0f){
            return true;
        }
        return false;
    }
}
