﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Camera : MonoBehaviour
{
    private Camera                  mCam;
    private PC_Controller           mCont;

    private void Start()
    {
        mCam = GetComponent<Camera>();
        if(!mCam){
            Debug.Log("No Cam Found");
        }

        mCont = GetComponentInParent<PC_Controller>();
    }

    // attach a camera to whatever object you put this in.
    private void Update(){
        //SetFreeOrientation();
    }

    private void SetFreeOrientation()
    {

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // rotate camera view around y axis.
        transform.RotateAround(transform.position, Vector3.up, mouseX);
        Vector3 xAx = Vector3.Cross(transform.forward, Vector3.up);
        transform.RotateAround(transform.position, xAx, mouseY);
    }
    
    private void Orient(){

    }


}
