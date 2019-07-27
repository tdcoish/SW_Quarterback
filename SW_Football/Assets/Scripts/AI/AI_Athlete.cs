﻿/*************************************************************************************
Just generally a component that all players are going to have in common.
*************************************************************************************/
using UnityEngine;
using System.IO;

public class AI_Athlete : MonoBehaviour
{
    // we take the entire line in and find our tag.
    public string               mTag = "NON";
    
    public bool                 mWaitForSnap = true;

    private Rigidbody           rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();

        if(mWaitForSnap){
            rBody.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    public void OnSnap()
    {
        mWaitForSnap = false;
        rBody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    public void OnPlayOver()
    {
        mWaitForSnap = true;
        rBody.velocity = Vector3.zero;
        rBody.constraints = RigidbodyConstraints.FreezePosition;
    }
}