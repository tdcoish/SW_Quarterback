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
    public float                mReceiverSwitchInterval = 5f;  
    public int                  mActiveTarget;
    public float                mWaitToMakeRecHot = 2f;

    void Start()
    {
        cPPMan = GetComponent<PP_Manager>();
        cArrMan = GetComponent<PP_Man_Arr>();

        refTargets = FindObjectsOfType<PP_Target>();
    }

    public void FHandleSwitchingReceiverIfTimeRunsOut()
    {
        // now we focus on switching the receiver or not.
        if(Time.time - mLastReceiverSwitch > mReceiverSwitchInterval && cPPMan.mState == PP_State.GAME_ACTIVE)
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

        mLastReceiverSwitch = Time.time;
    }

}
