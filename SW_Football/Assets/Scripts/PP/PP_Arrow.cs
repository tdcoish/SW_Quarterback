/*************************************************************************************
Just goes up and down and rotates.
*************************************************************************************/
using UnityEngine;

public class PP_Arrow : MonoBehaviour
{

    public float                    mUpDis = 2f;
    public float                    mUpPeriod = 1f;
    public float                    mRotSpd = 180f;

    private float                   mStartY;

    void Start()
    {
        mStartY = transform.position.y;
    }

    void Update()
    {
        Vector3 vPos = transform.position;
        vPos.y = mStartY + Mathf.Cos(Time.time*(2/mUpPeriod)) * mUpDis;
        transform.position = vPos;

        Vector3 vRot = transform.rotation.eulerAngles;
        vRot.y += Time.deltaTime * mRotSpd;
        transform.rotation = Quaternion.Euler(vRot);
    }
}
