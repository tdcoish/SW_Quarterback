/*************************************************************************************
This script takes over when the defender needs to just tackle the runner. For now just 
run directly to the tackler.
*************************************************************************************/
using UnityEngine;

public class AI_Tackler : MonoBehaviour
{
    private AI_ZoneDefence              cZoneDef;
    private Rigidbody           rBody;

    public bool                 mActivated = false;

    private Transform           rBallCarrier;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        if(!rBody){
            Debug.Log("No rigidbody found");
        }

        cZoneDef = GetComponent<AI_ZoneDefence>();
        if(!cZoneDef){
            Debug.Log("No zone defence found");
        }
    }

    void Update()
    {
        // for now just run straight to the ballcarrier.
        if(mActivated)
        {
            Vector3 dis = transform.position - rBallCarrier.position;
            dis = Vector3.Normalize(dis);
            rBody.velocity = dis * cZoneDef.mMaxVel;
        }
    }

    public void OnCatch()
    {
        rBallCarrier = FindObjectOfType<PROJ_Football>().transform;
    }
}
