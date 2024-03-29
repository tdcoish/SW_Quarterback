﻿/*************************************************************************************
Just shoot at the player every x seconds.
*************************************************************************************/
using UnityEngine;

public class PP_Turret : MonoBehaviour
{
    public PP_Projectile        PF_TennisBall;

    public GameObject           mProjSpawnPoint;

    public float                mSpreadRange = 5f;

    public bool                 mActive = false;
    private AD_Turret           cAud;
    private AN_Turret           cAnim;

    void Start()
    {
        cAud = GetComponentInChildren<AD_Turret>();
        cAnim = GetComponent<AN_Turret>();
    }

    void Update()
    {
        if(!mActive){
            return;
        }  
    }

    public void FFireTurret()
    {
        PP_Projectile clone = Instantiate(PF_TennisBall, mProjSpawnPoint.transform.position, transform.rotation);

        Vector3 dif = FindObjectOfType<PC_Controller>().transform.position - mProjSpawnPoint.transform.position;
        // now we add a very slight randomness to the projectile path.
        dif.y = 0f;

        float fRandAng = Random.Range(-mSpreadRange, mSpreadRange);
        dif = Quaternion.AngleAxis(fRandAng, Vector3.up) * dif;
        dif = Vector3.Normalize(dif);
        Vector3 vel = dif * 4f;          // figure out the speed later.

        clone.GetComponent<Rigidbody>().velocity = vel;

        cAud.FPlayFire();
        cAnim.FPlayFireAnim();
    }

    // This will ruin the staggering.
    public void FActivate()
    {
        mActive = true;
    }

    public void FDeactivate()
    {
        mActive = false;
    }
}
