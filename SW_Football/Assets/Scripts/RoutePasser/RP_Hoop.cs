/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class RP_Hoop : MonoBehaviour
{
    private RP_Manager                  rManager;

    public string                       mWRTag;
    public Vector3                      mStartRot;

    public float                        mYRot = 20f;
    public float                        mPeriod = 2f;

    void Start()
    {
        rManager = FindObjectOfType<RP_Manager>();
    }

    // thinking about maybe having them sway back and forth.
    void Update()
    {
        float yRot = Mathf.Sin(Time.time * mPeriod) * mYRot;
        Vector3 vRot = mStartRot;
        vRot.y += yRot;
        transform.rotation = Quaternion.Euler(vRot);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PROJ_Football>())
        {
            rManager.OnThroughRing();
        }
    }
}
