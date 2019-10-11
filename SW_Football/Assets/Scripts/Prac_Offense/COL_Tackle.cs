/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class COL_Tackle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PRAC_Off_Ply p = UT_FindComponent.FindComponent<PRAC_Off_Ply>(other.gameObject);
        if(p != null)
        {
            if(p.mState == PRAC_Ath.PRAC_ATH_STATE.SRUN_WITH_BALL){
                Debug.Log("Tackle the offensive player");
                p.FENTER_Tackled();
                TDC_EventManager.FBroadcast(TDC_GE.GE_Tackled);
            }
        }
    }
}
