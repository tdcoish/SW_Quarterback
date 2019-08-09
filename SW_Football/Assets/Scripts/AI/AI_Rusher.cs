/*************************************************************************************
For now he just rushes directly at the quarterback. If "caught" in the sphere of influence
of a blocker, then he just tries to bullrush for now.

We need to get a reference to the quarterback.

More and more I think we need an overarching game manager that everything has a reference to.
*************************************************************************************/
using UnityEngine;

public class AI_Rusher : MonoBehaviour
{

    private Rigidbody           cRigid;

    private AI_Athlete          cAthlete;

    //private Transform           refQuarterback;
    private PC_Controller           refPlayer;

    public bool                 mActive = false;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        cAthlete = GetComponent<AI_Athlete>();

        cAthlete.mSpd = 1f;
    }

    // so we're trying to move towards the quarterback, let's just do that for now. Totally ignore blockers.
    void Update()
    {
        if(mActive)
        {
            Vector3 vel = refPlayer.transform.position - transform.position;
            vel = Vector3.Normalize(vel);
            vel *= cAthlete.mSpd;
            cRigid.velocity = vel;
        }
    }

    public void OnSnap()
    {
        refPlayer = FindObjectOfType<PC_Controller>();
        mActive = true;
    }
}
