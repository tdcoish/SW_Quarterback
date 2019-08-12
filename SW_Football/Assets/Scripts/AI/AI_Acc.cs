/*************************************************************************************
New problem, the velocity function does not take into account momentum. Players need a certain
amount of force they can generate for their runs, as well as taking into account pre-existing 
velocity. Then we factor in the force of shoves, and we find the net acceleration of a player.

Small wrinkle, need to find a way to cap the velocity, unless a player is shoved. That can be 
done by having the "burst" decay as the speed increases, so it's 0 when we're at top speed.

However, burst decays to zero only along the vector of travel. Imagine a player at top speed. They
can't acclerate forward, but they can stop pretty fast, right? So we ultimately need to factor in
the axis of acceleration and how it differs from the current velocity.

You know what, we can view our own legs as giving us "shoves" in certain directions. Yeah. That
works for now.

Wait, there's a problem. Some huge guy with no quick twitch muscle fibers may have all the 
strength in the world, but very little power. So he won't accelerate himself very well, but he 
will be really hard to stop. That means that his ability to power through blockers will be better
than this stat would make you think, but his acceleration would be worse than this stat makes it.

Then again, force is force, and this is probably close enough.

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

    void Update()
    {
        
    }

    // Some script calls us and tells us which direction we would like to go. 
    public void FCalcAcc(Vector3 dir)
    {

        dir.y = 0f;
        dir = Vector3.Normalize(dir);

        float fDot = Vector3.Dot(dir, transform.forward);
        float fAngle = Vector3.SignedAngle(dir, transform.forward, Vector3.up);
        Debug.Log("Dif between acc dir and forward dir: " + fDot);
        Debug.Log("Dif in angles: " + fAngle);

        // now, what should the angle be?
        float fPercentMaxSpd = cRigid.velocity.magnitude / cAthlete.mSpd;
        float fAccAngle = fAngle * (1+fPercentMaxSpd);
        Debug.Log("Perc max spd: " + fPercentMaxSpd);
        Debug.Log("Acc Angle: " + fAccAngle);
        if(Mathf.Abs(fAccAngle) > 180f){
            fAccAngle *= 180f/Mathf.Abs(fAccAngle);
            Debug.Log("Would be greater than pure backwards");
        } 

        // okay, so now we need to actually accelerate the entity in the correct vector.
        // So what is that vector?
        Vector3 vAccDir = Quaternion.AngleAxis(-fAccAngle, Vector3.up) * transform.forward;
        // Vector3 vAccDir = Quaternion.AngleAxis(fAccAngle, Vector3.up) * dir;
        Debug.Log("Orig Dir: " + dir);
        Debug.Log("Acc Dir: " + vAccDir);

        Debug.DrawLine(transform.position, transform.position+dir*10f, Color.green);
        Debug.DrawLine(transform.position, transform.position+vAccDir*10f, Color.cyan);
        Debug.DrawLine(transform.position, transform.position + cRigid.velocity, Color.black);


        // now we find the actual direction of acceleration.
        Vector3 vDirOfAcc = dir;
        Vector3 vDirAsFullSpd = dir;

        // let me first calculate the ideal velocity, assuming we could perfectly accelerate to our top speed.
        Vector3 vIdealVel = dir * cAthlete.mSpd;

        // then we accelerate
        float acc = Time.fixedDeltaTime * cAthlete.mAcc;

        // Vector3 vAcc = dir * acc;
        Vector3 vAcc = vAccDir * acc;
        Debug.Log("Dir of Acc: " + vAccDir);
        Debug.Log("Acceleration this frame: " + vAcc);

        cRigid.velocity = cRigid.velocity + vAcc;
        Debug.Log("Velocity: " + cRigid.velocity);

        if(cRigid.velocity.magnitude > cAthlete.mSpd)
        {
            cRigid.velocity *= cAthlete.mSpd/cRigid.velocity.magnitude;
        }

        // Might have already done this.
        transform.forward = cRigid.velocity.normalized;

    }
}
