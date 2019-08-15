/*************************************************************************************
Pocket passer target.
*************************************************************************************/
using UnityEngine;

public class PP_Target : MonoBehaviour
{
    public GameObject               PF_Particles;

    public GE_Event                 GE_TargetHit;

    public float                    mLastTimeHit = -10f;

    private void Start()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>())
        {
            mLastTimeHit = Time.time;

            Instantiate(PF_Particles, transform.position, transform.rotation);

            // We shove our position so the manager can know if the correct target was hit.
            GE_TargetHit.Raise(transform.position);
        }
    }
}
