﻿/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class AI_Target : MonoBehaviour
{
    [SerializeField]
    private GE_Event                HitTarget;

    private AI_Receiver             mOwner;
    void Start()
    {
        mOwner = GetComponentInParent<AI_Receiver>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>() != null){
            HitTarget.Raise(null);
        }
    }
}
