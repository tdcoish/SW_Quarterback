using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PROJ_Football : MonoBehaviour
{
    private Rigidbody               mRigid;

    // Start is called before the first frame update
    void Start()
    {
        mRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel = mRigid.velocity;

        if(vel != Vector3.zero){
            transform.rotation = Quaternion.LookRotation(vel);
            vel *= 0.999f;
            mRigid.velocity = vel;
        }
    }
}
