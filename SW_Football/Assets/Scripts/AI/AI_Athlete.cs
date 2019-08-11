/*************************************************************************************
Just generally a component that all players are going to have in common.

Each athlete has all the attributes, such as spd, agility, throw power, really everything.
I'm not going to add any attribute until I actually am using it though.
*************************************************************************************/
using UnityEngine;
using System.IO;

// Unfortunately, if I hide this in a struct, I can't see these in the editor.

public class AI_Athlete : MonoBehaviour
{
    public float            mSpd;
    public float            mThrPwr;
    public float            mBull;
    public float            mWgt;
    public float            mAnc;                       // basically how much strenght they can move themselves with. eg. 300 lbsm/s for giant OG.
    public float            mBks;               // Block Shedding. Will determine reductions and buffs for moves
    // public float            mGrp;               // grip, 100 = no reduction. 0 = you have no hands and cannot block.
    public float            mBrst;              // players "burst" from standing still. Calculates how much they can accelerate themselves. Decays to 0 at top spd.

    // we take the entire line in and find our tag.
    public string               mTag = "NON";

    public bool                 mWaitForSnap = true;

    private Rigidbody           rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();

        mSpd = 5f;
        mThrPwr = 25f;

        if(mWaitForSnap){
            rBody.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    public void OnSnap()
    {
        mWaitForSnap = false;
        rBody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    public void OnPlayOver()
    {
        mWaitForSnap = true;
        rBody.velocity = Vector3.zero;
        rBody.constraints = RigidbodyConstraints.FreezePosition;
    }
}
