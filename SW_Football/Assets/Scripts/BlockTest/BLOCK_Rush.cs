/*************************************************************************************
Rusher now has some decisions to make. Engage blocker, or just run around? When should they
be running straight at the QB?
*************************************************************************************/
using UnityEngine;

public class BLOCK_Rush : MonoBehaviour
{
    public BLOCK_Block                      rBlocker;
    public PC_Controller                    rPC;

    private Rigidbody                       cRigid;

    public float                            mEngageDistance = 1f;
    public float                            mMoveCooldown = 2f;
    public float                            mLastMoveTime;
    public float                            mMoveScore = 80f;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {

        float fDisToBlocker = Vector3.Distance(transform.position, rBlocker.transform.position); 
        bool blockerInFront = FuncBlockerInFront(transform.position, rBlocker.transform.position, transform.forward);
        if(fDisToBlocker < mEngageDistance && rBlocker.mState != BLOCK_Block.STATE.S_Stunned && blockerInFront)
        {
            RUN_Engage();
        }else{
            RUN_FreeRun();
        }
    }

    public void RUN_Engage()
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
    public void RUN_FreeRun()
    {
        transform.forward = FuncAngleToPlayer(transform.position, rPC.transform.position);
        Vector3 vDis = rPC.transform.position - transform.position;
        vDis = Vector3.Normalize(vDis);
        cRigid.velocity = vDis;
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

    private Vector3 FuncAngleToPlayer(Vector3 ourPos, Vector3 playerPos)
    {
        Vector3 vDis = playerPos - ourPos;
        vDis = Vector3.Normalize(vDis);
        return vDis;
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
