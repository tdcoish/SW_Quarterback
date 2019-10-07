/*************************************************************************************
The thing that the turrets shoot.
*************************************************************************************/
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PP_Projectile : MonoBehaviour
{
    public GameObject           PF_DeathParticles;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            TDC_EventManager.FBroadcast(TDC_GE.GE_PP_SackBallHit);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        Instantiate(PF_DeathParticles, transform.position, transform.rotation);
    }
}
