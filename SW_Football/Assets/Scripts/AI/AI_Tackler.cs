/*************************************************************************************
This script takes over when the defender needs to just tackle the runner. For now just 
run directly to the tackler.

Actually, they should run to some spot in front of the ball carrier, getting closer and 
closer until they touch.
*************************************************************************************/
using UnityEngine;

public class AI_Tackler : MonoBehaviour
{
    private AI_Athlete              cAthlete;
    private AI_ZoneDefence              cZoneDef;
    private Rigidbody           rBody;

    public bool                 mActivated = false;

    private Transform           rBallCarrier;

    void Start()
    {
        cAthlete = GetComponent<AI_Athlete>();
        if(!cAthlete){
            Debug.Log("No Athlete comp");
        }
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
            rBody.velocity = dis * cAthlete.mSpd;
        }
    }

    public void OnCatch()
    {
        rBallCarrier = FindObjectOfType<PROJ_Football>().transform;
    }
}
