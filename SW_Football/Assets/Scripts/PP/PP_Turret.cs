/*************************************************************************************
Just shoot at the player every x seconds.
*************************************************************************************/
using UnityEngine;

public class PP_Turret : MonoBehaviour
{
    
    public float                mFireRate;
    // so I can easily stagger them.
    public float                mLastShotTime;

    public PP_Projectile        PF_TennisBall;

    public GameObject           mProjSpawnPoint;

    public float                mSpreadRange = 5f;

    public bool                 mActive = false;

    void Update()
    {
        if(!mActive){
            return;
        }

        if(Time.time - mLastShotTime > mFireRate)
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

            mLastShotTime = Time.time;
        }    
    }

    public void FActivate()
    {
        mActive = true;
        mLastShotTime += Time.time;
    }

    public void FDeactivate()
    {
        mActive = false;
    }
}
