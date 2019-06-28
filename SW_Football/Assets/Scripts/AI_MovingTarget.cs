using UnityEngine;

public class AI_MovingTarget : MonoBehaviour
{
    private Rigidbody           mRigid;

    private float               mTime = 5f;
    private float               mLastChange = 0f;
    private float               mSpd = 2f;
    private bool                mGoRight = true;

    void Start()
    {
        mRigid = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        
        if(mGoRight){
            mRigid.velocity = transform.right * mSpd;
        }else{
            mRigid.velocity = transform.right * -mSpd;
        }

        if(Time.time - mLastChange > mTime){
            mGoRight = !mGoRight;
            mLastChange = Time.time;
        }
    }
}
