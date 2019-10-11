/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class COL_Tackle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(UT_FindComponent.FindComponent<PRAC_Off_Ply>(other.gameObject) != null)
        {
            Debug.Log("Tackle the offensive player");
            TDC_EventManager.FBroadcast(TDC_GE.GE_Tackled);
        }
    }
}
