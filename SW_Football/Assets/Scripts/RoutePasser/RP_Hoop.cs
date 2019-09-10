/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class RP_Hoop : MonoBehaviour
{
    private RP_Manager                  rManager;

    public string                       mWRTag;

    void Start()
    {
        rManager = FindObjectOfType<RP_Manager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>())
        {
            rManager.OnThroughRing();
        }
    }
}
