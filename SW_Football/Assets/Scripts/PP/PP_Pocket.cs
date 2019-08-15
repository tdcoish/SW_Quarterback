/*************************************************************************************
Basically we just call some events when the player leaves the pocket, and comes back in.
*************************************************************************************/
using UnityEngine;

public class PP_Pocket : MonoBehaviour
{

    public GE_Event                 GE_LeftPocket;
    public GE_Event                 GE_EnteredPocket;

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
            GE_EnteredPocket.Raise(null);
            Debug.Log("Entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            GE_LeftPocket.Raise(null);
            Debug.Log("Exited");
        }
    }
}
