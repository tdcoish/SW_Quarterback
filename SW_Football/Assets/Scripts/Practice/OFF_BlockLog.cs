/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class OFF_BlockLog : MonoBehaviour
{
    private Rigidbody           cRigid;

    private void Start()
    {
        cRigid = GetComponent<Rigidbody>();
    }

    // Call this when the play is actually running.
    public void FRunBlocking()
    {

    }
}
