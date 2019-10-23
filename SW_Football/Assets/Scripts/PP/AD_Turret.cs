/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class AD_Turret : MonoBehaviour
{
    public AudioSource                  mFireSound;
    
    public void FPlayFire()
    {
        mFireSound.Play();
    }
}
