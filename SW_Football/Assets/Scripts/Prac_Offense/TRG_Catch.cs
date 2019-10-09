/*************************************************************************************
If we get hit with a football here, do something.
*************************************************************************************/
using UnityEngine;

public class TRG_Catch : MonoBehaviour
{
    private PRAC_Ath                            cAth;
    void Start()
    {
        cAth = GetComponentInParent<PRAC_Ath>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>() != null)
        {
            if(cAth != null){
                cAth.FCaughtBall();
            }

            if(GetComponentInParent<PRAC_Off_Ply>()){
                TDC_EventManager.FBroadcast(TDC_GE.GE_BallCaught_Rec);
            }else{
                TDC_EventManager.FBroadcast(TDC_GE.GE_BallCaught_Int);
            }
        }
    }
}
