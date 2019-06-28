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
        transform.rotation = Quaternion.LookRotation(vel);
    }
}
