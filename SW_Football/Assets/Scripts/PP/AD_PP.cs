/*************************************************************************************
Tries to handle some events, but not all.

As of right now it won't be possible to play multiple audio effects at once, since there's only
a single audio source.
*************************************************************************************/
using UnityEngine;

public class AD_PP : MonoBehaviour
{
    public AudioClip              mTargetSuccess;
    public AudioClip              mTargetFailure;
    public AudioClip              mOutOfPocket;
    public AudioClip              mSacked;
    public AudioClip              mBallHitGround;

    public AudioSource              mSrc;

    private void Start()
    {
        mSrc = GetComponent<AudioSource>();
    }

    public void FPlayClip(AudioClip clip)
    {
        mSrc.clip = clip;
        mSrc.Play();
    }
}
