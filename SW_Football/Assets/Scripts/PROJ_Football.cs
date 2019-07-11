using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PROJ_Football : MonoBehaviour
{
    private Rigidbody               mRigid;

    [SerializeField]
    private SO_Vec3                 mPos;

    // Start is called before the first frame update
    void Start()
    {
        mRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        mPos.Val = transform.position;
        Vector3 vel = mRigid.velocity;

        if(vel != Vector3.zero){
            transform.rotation = Quaternion.LookRotation(vel);
            vel *= 0.999f;
            mRigid.velocity = vel;
        }
    }
}
