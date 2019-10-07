/*************************************************************************************
Sort of represents the pocket. You have to be inside, or else the game just breaks.
*************************************************************************************/
using UnityEngine;

public class RP_ThrowSpot : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            TDC_EventManager.FBroadcast(TDC_GE.GE_InPocket);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            TDC_EventManager.FBroadcast(TDC_GE.GE_OutPocket);
        }
    }
}
