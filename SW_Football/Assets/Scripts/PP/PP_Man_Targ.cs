/*************************************************************************************
Manages the targets.
*************************************************************************************/
using UnityEngine;

public class PP_Man_Targ : MonoBehaviour
{
    private PP_Manager          cPPMan;
    private PP_Man_Arr          cArrMan;

    public PP_Target[]          refTargets;
    private float               mLastReceiverSwitch;  
    public int                  mActiveTarget;
    public float                mWaitToMakeRecHot = 2f;
    public float                mExtraTimeForDistance;

    void Start()
    {
        cPPMan = GetComponent<PP_Manager>();
        cArrMan = GetComponent<PP_Man_Arr>();

    }

    public void FHandleSwitchingReceiverIfTimeRunsOut()
    {
        // now we focus on switching the receiver or not.
        if(Time.time - mLastReceiverSwitch > cPPMan.lDifData.mTimeBetweenTargetChanges+mExtraTimeForDistance && cPPMan.mState == PP_State.GAME_ACTIVE)
        {
            // There's no active target for a sec.
            mLastReceiverSwitch = Time.time;        // this will get overwritten, but it solves a bug
            FDeactivateReceiver();
        }
    }

    public void FDeactivateReceiver()
    {
        mActiveTarget = -1;

        PP_Arrow[] arrows = FindObjectsOfType<PP_Arrow>();
        for(int i=0; i<arrows.Length; i++)
        {
            Destroy(arrows[i].gameObject);
        }

        Invoke("SetNewActiveTarget", mWaitToMakeRecHot);

        cPPMan.mGameState = PP_GAME_STATE.CHILLING;
    }

    private void SetNewActiveTarget()
    {
        if(cPPMan.mState != PP_State.GAME_ACTIVE){
            Debug.Log("Wrong game state to spawn receiver");
            return;
        }

        if(mActiveTarget != -1)
        {
            Debug.Log("Already active receiver.");
            return;
        }
        // now we switch which receiver is active.
        // later, make it so it can't be the smae receiver.
        int ind = mActiveTarget;
        while(ind == mActiveTarget)
        {
            ind = (int)Random.Range(0, refTargets.Length);
        }
        mActiveTarget = ind;
        Vector3 vPos = refTargets[mActiveTarget].transform.position;
        vPos.y += 7f;

        cArrMan.FSpawnArrow(vPos, refTargets[ind].transform.rotation);

        // Now we need to set the extra time that this guy gets due to him being further away.
        float fDis = Vector3.Distance(FindObjectOfType<PP_Pocket>().transform.position, refTargets[mActiveTarget].transform.position);
        mExtraTimeForDistance = fDis/10f - 1f;
        mExtraTimeForDistance = (float)System.Math.Round((double)mExtraTimeForDistance, 0);

        mLastReceiverSwitch = Time.time;
    }

}
