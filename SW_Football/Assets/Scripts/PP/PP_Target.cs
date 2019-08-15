/*************************************************************************************
Pocket passer target.
*************************************************************************************/
using UnityEngine;

public class PP_Target : MonoBehaviour
{
    public GameObject               PF_Particles;

    public GE_Event                 GE_TargetHit;

    private void Start()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>())
        {
            Instantiate(PF_Particles, transform.position, transform.rotation);

            GE_TargetHit.Raise(null);
        }
    }
}
