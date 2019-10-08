/*************************************************************************************
Plays all the players audio.
*************************************************************************************/
using UnityEngine;

public class AD_Player : MonoBehaviour
{
    public AudioSource                      mThrowRelease;
    public AudioSource                      mThrowCancel;
    public AudioSource                      mThrowStart;

    public AudioSource                      mANThrowRelease1;
    public AudioSource                      mANThrowRelease2;
    public AudioSource                      mANThrowRelease3;
    public AudioSource                      mANThrowRelease4;

    // Make pow between 0-1.
    public void FBallThrown(float pow)
    {
        mThrowRelease.Play();

        if(pow > 0.9f)
        {
            float rand = Random.Range(0f, 1f);
            if(rand < 0.2f){
                mANThrowRelease1.Play();
            }else if(rand < 0.5f){
                mANThrowRelease2.Play();
            }else if(rand < 0.8f){
                mANThrowRelease3.Play();
            }else{
                mANThrowRelease4.Play();
            }
        }
    }

}
