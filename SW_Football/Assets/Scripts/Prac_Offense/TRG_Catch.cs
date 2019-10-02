/*************************************************************************************
If we get hit with a football here, do something.
*************************************************************************************/
using UnityEngine;

public class TRG_Catch : MonoBehaviour
{
    private PRAC_Off_Ply                        cAth;
    void Start()
    {
        cAth = GetComponentInParent<PRAC_Off_Ply>();

        TDC_EventManager.FAddHandler(TDC_GE.GE_BallCaught, OnBallCaught);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>() != null)
        {
            Debug.Log("I just got hit by the football");
            cAth.FCaughtBall();

            TDC_EventManager.FBroadcast(TDC_GE.GE_BallCaught);
        }
    }

    // Most likely another receiver will trigger this, not us.
    public void OnBallCaught()
    {
        Debug.Log("Someone caught the ball");
    }
}
