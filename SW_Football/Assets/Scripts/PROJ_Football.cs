using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PROJ_Football : MonoBehaviour
{
    private Rigidbody               cRigid;

    [SerializeField]
    private SO_Vec3                 mPos;

    private bool                    mGrounded = false;

    public GameObject               PF_Particles;

    [SerializeField]
    private GE_Event                GE_FB_HitGround;

    // Start is called before the first frame update
    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        mPos.Val = transform.position;
        Vector3 vel = cRigid.velocity;

        if(vel != Vector3.zero){
            transform.rotation = Quaternion.LookRotation(vel);
            //vel *= 0.999f;
            cRigid.velocity = vel;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(mGrounded) return;

        if(collision.other.GetComponent<EN_FieldGround>() != null){
            GE_FB_HitGround.Raise(null);
            mGrounded = true;
        }
    }

    void OnDestroy()
    {
        Instantiate(PF_Particles, transform.position, transform.rotation);
    }
}
