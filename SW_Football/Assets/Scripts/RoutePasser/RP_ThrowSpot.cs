/*************************************************************************************
Sort of represents the pocket. You have to be inside, or else the game just breaks.
*************************************************************************************/
using UnityEngine;

public class RP_ThrowSpot : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            Debug.Log("Player hit me");
        }
    }
}
