﻿/*************************************************************************************
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

    AI_Shove(Vector3 vForce, string sShover)
    {
        mForce = vForce;
        mShover = sShover;
    }
}

public class AI_TakesShove : MonoBehaviour
{

    private AI_Athlete          cAthlete;

    public List<AI_Shove>               mShoves;

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

    public void TakeShove(AI_Shove shove)
    {
        // since players can't double shove, we first delete any existing shove by that player
        for(int i=0; i<mShoves.Count; i++){
            if(mShoves[i].mShover == shove.mShover)
            {
                // if the player pushed them harder in the past, don't remove the shove.
                if(mShoves[i].mForce.magnitude > shove.mForce.magnitude){
                    Debug.Log("Existing shove harder");
                    return;
                }
                mShoves.RemoveAt(i);
                Debug.Log("Removed existing shove");
                break;
            }
        }

        Debug.Log("Still here");

        mShoves.Add(shove);
    }

    private void RecalculateShoves()
    {
        mAllForces = Vector3.zero;

        // now here's where we calc the actual strength
        for(int i=0; i<mShoves.Count; i++)
        {
            mAllForces += mShoves[i].mForce;
        }
        // wow, that was easy.
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
