/*************************************************************************************
Given to all players that need to block? Maybe just all players period, but we disable this
unless they need to block. That makes more sense because almost everyone needs to block
at some point, interceptions, receptions, etcetera.

In pass protection he's trying to both get between the rusher and the quarterback, as well
as trying his best push the defender away from the quarterback.
*************************************************************************************/
using UnityEngine;

public class AI_Blocker : MonoBehaviour
{

    private AI_Athlete          cAthlete;
    private Rigidbody           cRigid;

    // for now I'll just give him the defender to block
    private AI_Rusher           refRusher;
    private PC_Controller       refPlayer;

    public GameObject           RefRandoSpotMarker;

    private bool                mActive = false;

    void Start()
    {
        cAthlete = GetComponent<AI_Athlete>();
        cRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1) get midway between the rusher and the passer.
        if(mActive)
        {
            Vector3 midSpot = refPlayer.transform.position;
            Vector3 dis = refRusher.transform.position - refPlayer.transform.position;
            dis/=2f;
            midSpot += dis;

            //Instantiate(RefRandoSpotMarker, midSpot, transform.rotation);

            Vector3 vel = midSpot - transform.position;
            vel = Vector3.Normalize(vel);
            vel *= cAthlete.mSpd;
            cRigid.velocity = vel;
        }
    }

    public void OnSnap()
    {
        mActive = true;

        refRusher = FindObjectOfType<AI_Rusher>();
        refPlayer = FindObjectOfType<PC_Controller>();
    }
}
