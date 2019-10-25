/*************************************************************************************
Component that manages the turrets.
*************************************************************************************/
using UnityEngine;

public class PP_Man_Tur : MonoBehaviour
{

    private PP_Manager          cPPMan;

    public PP_Turret[]          refTurrets;
    public float                mLastShotFire;
    
    void Start()
    {
        cPPMan = GetComponent<PP_Manager>();
        refTurrets = FindObjectsOfType<PP_Turret>();    
    }

    public void FHandleTurrets(bool ballInAir)
    {
        if(ballInAir){
            return;
        }

        if(Time.time - mLastShotFire > cPPMan.lDifData.mTurretFireRate)
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
