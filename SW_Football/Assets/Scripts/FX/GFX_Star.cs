/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class GFX_Star : MonoBehaviour
{

    void Update()
    {
        Vector3 vPos = transform.position;
        vPos.y = -1f;
        transform.position = vPos;       
        Vector3 vRot = transform.rotation.eulerAngles;
        vRot.x = 90f;
        transform.rotation = Quaternion.Euler(vRot);
    }
}
