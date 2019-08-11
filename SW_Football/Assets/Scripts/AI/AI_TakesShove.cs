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
    
    private void Update()
    {
        DampenShovesOverTime();
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
                        Debug.Log("Existing shove harder");
                        return;
                    }
                }
                Debug.Log("Removed shove: " + mShoves[i].mShover);
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

        Vector3 vSelfForces = Vector3.zero;

        Vector3 vEnemyForces = Vector3.zero;
        Debug.Log("Enemy forces: " + vEnemyForces);
        mAllForces = Vector3.zero;

        // add up all the shoves.
        for(int i=0; i<mShoves.Count; i++)
        {
            if(mShoves[i].mShover == "SELF"){
                vSelfForces += mShoves[i].mForce;
            }else{
                vEnemyForces += mShoves[i].mForce;
            }
        }

        // now we reduce the cumulative effects of enemy shoving.
        float mag = vEnemyForces.magnitude;
        if(mag != 0f){
            mag *= (100f-cAthlete.mBks)/100f;           // so 20 block shedding reduces blocks by 20%
            mag -= cAthlete.mAnc;
            mag /= cAthlete.mWgt;
            if(mag < 0f) mag = 0f;

            vEnemyForces *= (mag / vEnemyForces.magnitude);
        }

        Debug.Log("Enemy forces: " + vEnemyForces);

        // now we divide our self shoves by our weight.
        vSelfForces /= cAthlete.mWgt;
        Debug.Log("Self forces: " + vSelfForces);

        // finally, we just add up the forces of both here.
        mAllForces = vEnemyForces + vSelfForces;
        Debug.Log("Total forces : " + mAllForces);
    }

    // This is going to get a lot more complicated eventually, but for now we just quickly dampen any shoves.
    private void DampenShovesOverTime()
    {
        for(int i=0; i<mShoves.Count; i++){
            float mag = mShoves[i].mForce.magnitude;
            mag -= Time.deltaTime * 100f;           // - x lbsm/s every second
            if(mag < 0f) mag = 0f;
            mShoves[i].mForce *= (mag/mShoves[i].mForce.magnitude);
        }

    }

}
