/*************************************************************************************
The trophy rotates around in place. Just like the item creation from SO2.
*************************************************************************************/
using UnityEngine;

public class PP_Trophy : MonoBehaviour
{

    public float                mXSpd = 0.5f;
    public float                mYSpd = 1f;
    public float                mZSpd = 2f;


    private void Update()
    {
        Vector3 vLookDir = new Vector3();
        vLookDir.y = Time.time * mXSpd;
        vLookDir.x = Time.time * mYSpd;
        vLookDir.z = Time.time * mZSpd;

        transform.rotation = Quaternion.Euler(vLookDir*180f);
    }
}
