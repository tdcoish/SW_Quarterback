using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PROJ_Football : MonoBehaviour
{
    private Rigidbody               cRigid;

    [SerializeField]
    private SO_Vec3                 mPos;

    private bool                    mGrounded = false;

    public FX_Football              PF_PartAndSFX;

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

        EN_FieldGround field = UT_FindComponent.FindComponent<EN_FieldGround>(collision.gameObject);
        if(field != null){
            TDC_EventManager.FBroadcast(TDC_GE.GE_BallHitGround);
            mGrounded = true;
            FX_Football s = Instantiate(PF_PartAndSFX, transform.position, transform.rotation);
            s.mClunk.Play();
        }
    }

    void OnDestroy()
    {
        Instantiate(PF_PartAndSFX, transform.position, transform.rotation);
    }
}
