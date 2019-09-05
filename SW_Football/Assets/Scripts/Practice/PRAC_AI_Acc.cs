/*************************************************************************************
Goal based movement system. They have some simulated momentum, and they try to go to their 
goals based on that. I also simulate the human body to a certain extent.
*************************************************************************************/
using UnityEngine;

public class PRAC_AI_Acc : MonoBehaviour
{
    private Rigidbody               cRigid;

    public float                    mAcc = 5f;
    public float                    mSpd = 3f;

    // public bool                     mPretendEngaged = false;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
    }

    // Some script calls us and tells us which direction we would like to go. 
    // has kind of nasty side effect of just changing the velocity.
    public void FCalcAcc(Vector3 dir)
    {
        dir.y = 0f;
        dir = Vector3.Normalize(dir);

        // Find the correct vector of acceleration
        float fAngle = Vector3.SignedAngle(dir, transform.forward, Vector3.up);
        float fPercentMaxSpd = cRigid.velocity.magnitude / mSpd;
        float fAccAngle = fAngle * (1+fPercentMaxSpd);
        if(Mathf.Abs(fAccAngle) > 180f){
            fAccAngle *= 180f/Mathf.Abs(fAccAngle);
        } 

        Vector3 vAccDir = Quaternion.AngleAxis(-fAccAngle, Vector3.up) * transform.forward;
        vAccDir = Vector3.Normalize(vAccDir);

        // Debug.DrawLine(transform.position, transform.position+dir*10f, Color.green);
        // Debug.DrawLine(transform.position, transform.position+vAccDir*10f, Color.cyan);
        // Debug.DrawLine(transform.position, transform.position + cRigid.velocity, Color.black);

        // Find the acceleration WRT our current velocity.
        // We add a bit of a fudge factor using fVelDot to make decelerating from full speed faster,
        // and acceleration nearing full speed slower.
        float fAcc = Time.fixedDeltaTime * mAcc;
        float fVelDot = Vector3.Dot(vAccDir, transform.forward*cRigid.velocity.magnitude);
        fVelDot /= mSpd;
        fVelDot *= -0.5f;           // We want a noticeable but not overpowering effect.
        fVelDot += 1f;              // should technically be 0.5f for best accuracy.
        Vector3 vAcc = vAccDir * fAcc * fVelDot;
        // Vector3 vAcc = vAccDir * fAcc

        cRigid.velocity = cRigid.velocity + vAcc;

        // need some other way of limiting acc if our speed is too high, because you can get pushed past top speed.
        // Then again, maybe you just fall then?
        // workaround, if we're getting pushed, then no speed limit, but if we're not, then speed limit
        if(cRigid.velocity.magnitude > mSpd)
        {
            cRigid.velocity *= mSpd/cRigid.velocity.magnitude;
        }

        // Might have already done this.
        if(cRigid.velocity.magnitude != 0f)
        {
            transform.forward = cRigid.velocity.normalized;
        }

    }
}
