/*************************************************************************************
If we get hit with a football here, do something.
*************************************************************************************/
using UnityEngine;

public class TRG_Catch : MonoBehaviour
{
    private AI_CatchHandling                    cCatchHandler;

    void Start()
    {
        cCatchHandler = GetComponentInParent<AI_CatchHandling>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>() != null)
        {
            cCatchHandler.FENTER_Controlling();
        }
    }
}
