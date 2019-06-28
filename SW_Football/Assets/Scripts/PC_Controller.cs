using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************************
Two camera modes, one handles like FPS, other handles like Half Life 2 vehicle controls, where 
movement is independent of where the camera is looking.
**************************************************************************************** */

public class PC_Controller : MonoBehaviour
{
    [SerializeField]
    private PROJ_Football           PF_Football;

    [SerializeField]
    private GameObject              mThrowPoint;

    public float                    mSpd = 5f;
    public float                    mThrowSpd = 10f;

    private Rigidbody               mRigid;

    // if false, then we're doing vehicle-style controls
    private bool                    mFPSVision = true;

    // Start is called before the first frame update
    void Start()
    {
        mRigid = GetComponent<Rigidbody>();
        if(!mRigid){
            Debug.Log("No Rigidbody");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleThrowing();
    }

    private void HandleThrowing()
    {
        if(Input.GetMouseButtonDown(0)){
            PROJ_Football clone = Instantiate(PF_Football, mThrowPoint.transform.position, transform.rotation);
            clone.GetComponent<Rigidbody>().velocity = transform.forward * mThrowSpd;
        }
    }

    private void HandleMovement()
    { 
        float xVel = 0f;
        float yVel = 0f;
        // make our quarterback run around.
        if(Input.GetKey(KeyCode.A)){
            xVel -= mSpd;
        }
        if(Input.GetKey(KeyCode.D)){
            xVel += mSpd;
        }
        if(Input.GetKey(KeyCode.W)){
            yVel += mSpd;
        }
        if(Input.GetKey(KeyCode.S)){
            yVel -= mSpd;
        }

        // technically I should normalize the speed, but whatever.
        mRigid.velocity = new Vector3(xVel, 0f, yVel);
    }
}
