/*************************************************************************************
Component that manages the turrets.
*************************************************************************************/
using UnityEngine;

public class PP_Man_Tur : MonoBehaviour
{

    public PP_Turret[]          refTurrets;
    public float                mLastShotFire;
    public float                mFireRate = 1f;
    
    void Start()
    {
        refTurrets = FindObjectsOfType<PP_Turret>();    
    }

    public void FHandleTurrets()
    {
        if(Time.time - mLastShotFire > mFireRate)
        {
            int ind = Random.Range(0, refTurrets.Length);
            refTurrets[ind].FFireTurret();
            mLastShotFire = Time.time;
        }
    }

    public void FActivateTurrets()
    {
        for(int i=0; i<refTurrets.Length; i++){
            refTurrets[i].FActivate();
        }
    }

    public void FDeactivateTurrets()
    {
        for(int i=0; i<refTurrets.Length; i++){
            refTurrets[i].FDeactivate();
        }
    }
}
