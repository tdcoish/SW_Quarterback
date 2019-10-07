/*************************************************************************************
Pocket passer target.
*************************************************************************************/
using UnityEngine;

public class PP_Target : MonoBehaviour
{
    public GameObject               PF_Particles;

    public float                    mLastTimeHit = -10f;

    public float                mYSpd = 1f;

    // Gonna see how a slow rotation works.
    void Update()
    {
        Vector3 vLookDir = new Vector3();
        vLookDir.y = Time.time * mYSpd;
        // vLookDir.x = Time.time * mXSpd;
        // vLookDir.z = Time.time * mZSpd;

        transform.rotation = Quaternion.Euler(vLookDir*180f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>())
        {
            mLastTimeHit = Time.time;

            Instantiate(PF_Particles, transform.position, transform.rotation);

            TDC_EventManager.FBroadcast(TDC_GE.GE_PP_TargetHit);
        }
    }
}
