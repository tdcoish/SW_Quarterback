/*************************************************************************************
Sort of represents the pocket. You have to be inside, or else the game just breaks.
*************************************************************************************/
using UnityEngine;

public class RP_ThrowSpot : MonoBehaviour
{
    public GE_Event                     GE_EnteredSpot;
    public GE_Event                     GE_LeftSpot;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            GE_EnteredSpot.Raise(null);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            GE_LeftSpot.Raise(null);
        }
    }
}
