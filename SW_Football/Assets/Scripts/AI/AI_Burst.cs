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
*************************************************************************************/
using UnityEngine;

public class AI_Burst : MonoBehaviour
{
    private Rigidbody               cRigid;

    private AI_Athlete              cAthlete;
    private AI_TakesShove           cTakesShove;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        cAthlete = GetComponent<AI_Athlete>();  
        cTakesShove = GetComponent<AI_TakesShove>();

        // for now everyone can accelerate a 100 lbs man 5 meters/s
        cAthlete.mBrst = 500f;  
    }

    void Update()
    {
        
    }

    // Some script calls us and tells us which direction we would like to go. 
    // We then try to accelerate in that direction.
    public void FCalcAcceleration(Vector3 dir)
    {

        // burst decays linearly to zero.
        // // ultimately have to do this based on pythagorean direction. Imagine accelerating to side at full speed.
        // float fBrst = cAthlete.mBrst / (1 - (cRigid.velocity.magnitude / cAthlete.mSpd));
        // if(fBrst <= 0f){
        //     Debug.Log("Already at top speed");
        //     return;
        // }

        // for now, just linear acceleration. Much easier to test with.
        // calculate the velocity IN THE DIRECTION WE WANT TO ACCELERATE IN.
        float vInCorrectDir = Vector3.Dot(Vector3.Normalize(cRigid.velocity), Vector3.Normalize(dir));

        if(vInCorrectDir <= 0f){
            Debug.Log("Want to accelerate somewhat backwards");
            vInCorrectDir = 0f;
        }
        else if(vInCorrectDir < 0.5f){
            Debug.Log("Wants to go about 0-45 degrees from current trajectory.");
        }

        vInCorrectDir *= cRigid.velocity.magnitude;

        if(vInCorrectDir > cAthlete.mSpd){
            Debug.Log("Going too fast");
            return;
        }        
        float acc = cAthlete.mBrst * Time.deltaTime;
        acc /= cAthlete.mWgt;

        Debug.Log("Acc: " + acc);

        // now we accelerate in the direction we want.
        Vector3 newVel = cRigid.velocity + dir*acc;
        // now keep the velocity well within the bounds.
        if(newVel.magnitude > cAthlete.mSpd){
            newVel *= cAthlete.mSpd/newVel.magnitude;
            Debug.Log("Need to slow down");
        }
        cRigid.velocity = newVel;
    }
}
