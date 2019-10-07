/*************************************************************************************
Basically we just call some events when the player leaves the pocket, and comes back in.
*************************************************************************************/
using UnityEngine;

public class PP_Pocket : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            TDC_EventManager.FBroadcast(TDC_GE.GE_InPocket);
            Debug.Log("Entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            TDC_EventManager.FBroadcast(TDC_GE.GE_OutPocket);
            Debug.Log("Exited");
        }
    }
}
