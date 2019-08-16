/*************************************************************************************
The thing that the turrets shoot.
*************************************************************************************/
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PP_Projectile : MonoBehaviour
{

    public GE_Event             GE_TennisBallHitPlayer;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PC_Controller>())
        {
            GE_TennisBallHitPlayer.Raise(null);
            Destroy(gameObject);
        }
    }
}
