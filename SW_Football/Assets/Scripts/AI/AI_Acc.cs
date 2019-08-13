/*************************************************************************************
UPDATE: Scrapped the pure force based system. Now we're going with a traditional system, then 
factoring in weight with regards to collisions. What this script does now is calculate the 
desired direction and strength of acceleration. Later, collisions will "dampen" that, and we 
will eventually get the desired momentum change.

NOTE: Collisions should have some penalty, regardless. Think of having to run at full speed
while fending off some block. Not gonna happen.

Update. Except for in the case of perfect straight line acceleration/deceleration, there will always be a difference between
the goal, and the imaginary point we should be accelerating to. This is because our momentum will always carry us beyond the point we
need to go to, unless we "overcorrect". I'm not sure what the exact overcorrection should be. When at full speed, the overcorrections should 
be 100%. As an example, if we are sprinting at full speed forwards, and must go to a point 90* to our left, then we should be accelerating completely
backwards, in other words, slowing down. On the other hand, if we are going zero speed, then we should be accelerating directly to our left, since we 
have no speed to compensate for in the first place.
However, this will break down if the goal is behind us. In that case, we will get weird behaviour if we try to double the angle, in fact, we would be 
accelerating forwards! It is very difficult to think of the perfect angle to move to when the play is behind us, but I'm going to just say that we 
attempt to double the angle, but cap it at 180*. If the play is behind us we just run to it, but if we are running at full speed, then we basically stop first 
before accelerating in the new direction.

UPDATE:
Now we want the player to accelerate slower and slower as they approach their top speed. At least forwards.
They accelerate/decelerate faster and faster as they approach that top speed. Maybe this should be linear?
*************************************************************************************/
using UnityEngine;

public class AI_Acc : MonoBehaviour
{
    private Rigidbody               cRigid;

    private AI_Athlete              cAthlete;
    private AI_TakesShove           cTakesShove;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        cAthlete = GetComponent<AI_Athlete>();  
        cTakesShove = GetComponent<AI_TakesShove>();

        // for now everyone can accelerate at 5 meters/s^2
        cAthlete.mAcc = 5f;  
    }

    // Some script calls us and tells us which direction we would like to go. 
    public void FCalcAcc(Vector3 dir)
    {
        dir.y = 0f;
        dir = Vector3.Normalize(dir);

        // Find the correct vector of acceleration
        float fAngle = Vector3.SignedAngle(dir, transform.forward, Vector3.up);
        float fPercentMaxSpd = cRigid.velocity.magnitude / cAthlete.mSpd;
        float fAccAngle = fAngle * (1+fPercentMaxSpd);
        if(Mathf.Abs(fAccAngle) > 180f){
            fAccAngle *= 180f/Mathf.Abs(fAccAngle);
        } 

        Vector3 vAccDir = Quaternion.AngleAxis(-fAccAngle, Vector3.up) * transform.forward;
        vAccDir = Vector3.Normalize(vAccDir);

        Debug.DrawLine(transform.position, transform.position+dir*10f, Color.green);
        Debug.DrawLine(transform.position, transform.position+vAccDir*10f, Color.cyan);
        Debug.DrawLine(transform.position, transform.position + cRigid.velocity, Color.black);

        // Find the acceleration WRT our current velocity.
        // We add a bit of a fudge factor using fVelDot to make decelerating from full speed faster,
        // and acceleration nearing full speed slower.
        float fAcc = Time.fixedDeltaTime * cAthlete.mAcc;
        float fVelDot = Vector3.Dot(vAccDir, transform.forward*cRigid.velocity.magnitude);
        fVelDot /= cAthlete.mSpd;
        fVelDot *= -0.5f;           // We want a noticeable but not overpowering effect.
        fVelDot += 1f;
        Vector3 vAcc = vAccDir * fAcc * fVelDot;
        // Vector3 vAcc = vAccDir * fAcc;

        cRigid.velocity = cRigid.velocity + vAcc;

        if(cRigid.velocity.magnitude > cAthlete.mSpd)
        {
            cRigid.velocity *= cAthlete.mSpd/cRigid.velocity.magnitude;
        }

        // Might have already done this.
        if(cRigid.velocity.magnitude != 0f)
        {
            transform.forward = cRigid.velocity.normalized;
        }

    }
}
