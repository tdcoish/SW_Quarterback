/*************************************************************************************
Basically this is what receives "shoves" or hits or whatever. All entities are going 
to have this.

We need a way where players "renew" their shoves, and don't scale them. How to do that?
I think we need a list of shoves and from which player, and which ones are active. Then we 
need players to throw their tags at us when they shove us, so if they shove us again, then we 
first destroy the effects of the previous shove from that player, then we add the new shove. 

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

// The vector is not a unit vector, it contains the strength in all axis (or just x and z
public class AI_Shove{

    public Vector3              mForce;
    public string               mShover;

    public AI_Shove(Vector3 vForce = new Vector3(), string sShover = "NONAME")
    {
        mForce = vForce;
        mShover = sShover;
    }
}

public class AI_TakesShove : MonoBehaviour
{

    private AI_Athlete          cAthlete;

    public List<AI_Shove>       mShoves;

    // the cumulative direction and strength of forces on a player.
    // most of the time there will be just one force, however, there may be quite a bit more.
    public Vector3              mAllForces;

    private void Start()
    {
        cAthlete = GetComponent<AI_Athlete>();

        mShoves = new List<AI_Shove>();
    }
    
    private void FixedUpdate()
    {
        //DampenShovesOverTime();
    }

    public void FTakeShove(AI_Shove shove, bool alwaysOverrideOld = false)
    {
        // since players can't double shove, we first delete any existing shove by that player
        for(int i=0; i<mShoves.Count; i++){
            if(mShoves[i].mShover == shove.mShover)
            {
                // if the player pushed them harder in the past, don't remove the shove.
                if(!alwaysOverrideOld){
                    if(mShoves[i].mForce.magnitude > shove.mForce.magnitude){
                        return;
                    }
                }
                mShoves.RemoveAt(i);
                break;
            }
        }

        mShoves.Add(shove);
    }

    /***************************************************************************************************************
    It will be unusual for a player to have multiple shoves acting upon them simultaneously. However, in the case where 
    this is true, such as double teams, we need to add up all the shove vectors. Then we dampen all of them through stats.

    1) Divide shove by hand fighting - currently not it
    2) Minus by internal strength
    3) Divide result by weight.
    If shove is now negative, then we just ignore the shove completely. In this respect, if a 350 lbs NT gets hit by a punter, 
    there's no movement at all.


    Since I changed the system to where you can "shove" yourself to get yourself moving, we need to first add up all the 
    enemy shoves, deal with those, then add up our own shoves. Then we get our total shoving.
    ************************************************************************************************************** */
    public void FRecalculateShoves()
    {
        // Debug.Log("Enemy forces: " + vEnemyForces);
        mAllForces = Vector3.zero;
        
        if(mShoves.Count == 0){
            return;
        }

        // add up all the shoves.
        for(int i=0; i<mShoves.Count; i++)
        {
            mAllForces += mShoves[i].mForce;
        }

        // now we reduce the cumulative effects of enemy shoving.
        float mag = mAllForces.magnitude;
        if(mag != 0f){
            //mag *= (100f-cAthlete.mBks)/100f;           // so 20 block shedding reduces blocks by 20%, eventually I have to do this in the takesShove part.
            mag -= cAthlete.mAnc;                       // set anch to 0f for now for testing.
            mag /= cAthlete.mWgt;
            if(mag < 0f) mag = 0f;

            mAllForces *= (mag / mAllForces.magnitude);
        }
    }

    // This is going to get a lot more complicated eventually, but for now we just quickly dampen any shoves.
    public void FDampenShovesOverTime()
    {
        for(int i=0; i<mShoves.Count; i++){
            float mag = mShoves[i].mForce.magnitude;
            mag -= Time.fixedDeltaTime * 1000f;           // - x lbsm/s every second
            if(mag < 0f){
                mag = 0f;
                mShoves.RemoveAt(i);
                i--;
                continue;
            } 
            mShoves[i].mForce *= (mag/mShoves[i].mForce.magnitude);
        }

    }

}
