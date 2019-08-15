/*************************************************************************************
Start simple.

Just rip from madden for now. So the receivers come open randomly, stay open for x seconds.

If you throw to a receiver who isn't open, subtract points. If you miss, subtract points.
If you step out of the pocket, subtract points.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PP_Manager : MonoBehaviour
{
    
    public int                  mScore;

    public PP_UI                refUI;

    private bool                mIsOutOfPocket = false;
    private float               mLastTimeInPocket;

    // Need a list of the "receivers"
    public PP_Target[]          mTargets;
    private float               mLastReceiverSwitch;  
    public float                mReceiverSwitchInterval = 5f;  
    private float               mReceiverCatchableCountdown;
    public int                  mActiveTarget;

    public GameObject           PF_Arrow;

    private void Update()
    {
        if(mIsOutOfPocket)
        {
            if(Time.time - mLastTimeInPocket >= 1f)
            {
                // this could be confusing, mLastTimeInPocket isn't actually the last time in pocket after a while.
                mLastTimeInPocket = Time.time;
                mScore -= 25;
            }
        }

        refUI.mScoreTxt.text = "Score: " + mScore;


        // now we focus on switching the receiver or not.
        if(Time.time - mLastReceiverSwitch > mReceiverSwitchInterval)
        {
            // now we switch which receiver is active.
            // later, make it so it can't be the smae receiver.
            int ind = mActiveTarget;
            while(ind == mActiveTarget)
            {
                ind = (int)Random.Range(0, mTargets.Length);
            }
            mActiveTarget = ind;
            Vector3 vPos = mTargets[mActiveTarget].transform.position;
            vPos.y += 2f;

            PP_Arrow[] arrows = FindObjectsOfType<PP_Arrow>();
            for(int i=0; i<arrows.Length; i++)
            {
                Destroy(arrows[i].gameObject);
            }

            Instantiate(PF_Arrow, vPos, mTargets[ind].transform.rotation);

            mLastReceiverSwitch = Time.time;
        }
    }

    public void OnTargetHit()
    {
        refUI.TXT_Instr.text = "Congrats";
        mScore += 100;

        if(Time.time - mTargets[mActiveTarget].mLastTimeHit < 0.1f)
        {
            Debug.Log("Hit the right target");
        }
    }

    public void OnStepOutOfPocket()
    {
        mIsOutOfPocket = true;
        mLastTimeInPocket = Time.time;
    }

    public void OnStepIntoPocket()
    {
        mIsOutOfPocket = false;
    }
}
