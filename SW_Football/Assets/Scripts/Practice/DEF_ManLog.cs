/*************************************************************************************
Logic behind playing man.
*************************************************************************************/
using UnityEngine;

public class DEF_ManLog : MonoBehaviour
{
    private Rigidbody           cRigid;

    // Gets assigned to us somehow.
    public PRAC_Off             rMan;

    private void Start()
    {
        cRigid = GetComponent<Rigidbody>();
    }

    // Call this when the play is actually running.
    public void FRunMan()
    {
        if(rMan == null)
        {
            Debug.Log("No man to cover");
            return;
        }
        // just straight up run to the guy.
        Vector3 dis = rMan.transform.position - transform.position;
        dis.y = 0f;
        dis = Vector3.Normalize(dis);

        cRigid.velocity = dis * 4f;
    }
}
