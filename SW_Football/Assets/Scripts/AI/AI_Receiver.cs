/*************************************************************************************
Right now just checks if you hit its target.
*************************************************************************************/
using UnityEngine;

public class AI_Receiver : MonoBehaviour
{


    private AI_Target       mTarget;

    void Start()
    {
        mTarget = GetComponentInChildren<AI_Target>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter()
    {

    }
}
