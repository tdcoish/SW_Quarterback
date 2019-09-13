/*************************************************************************************
Just spin that trophy around.
*************************************************************************************/
using UnityEngine;

public class TP_Spinner : MonoBehaviour
{

    void Update()
    {
        Vector3 vRot = transform.rotation.eulerAngles;
        vRot.y += Time.deltaTime * 30f;
        transform.rotation = Quaternion.Euler(vRot);
    }
}
