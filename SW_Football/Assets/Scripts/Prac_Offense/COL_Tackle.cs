/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class COL_Tackle : MonoBehaviour
{
    private PRAC_Ath                    cAth;
    void Start()
    {
        cAth = GetComponentInParent<PRAC_Ath>();
    }

    void OnTriggerEnter(Collider other)
    {
        PRAC_Off_Ply p = UT_FindComponent.FindComponent<PRAC_Off_Ply>(other.gameObject);
        if(p != null)
        {
            if(p.mState != PRAC_Ath.PRAC_ATH_STATE.STACKLED){
                Debug.Log(p.mState);
                if(p == cAth.rMan.FGetBallCarrier()){
                    Debug.Log("Tackle the offensive player");
                    p.FENTER_Tackled();
                    TDC_EventManager.FBroadcast(TDC_GE.GE_Tackled);
                }else{
                    Debug.Log("Wasn't the ball carrier");
                }
            }
        }
    }
}
