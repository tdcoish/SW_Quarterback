/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class AD_Prac : MonoBehaviour
{
    public AudioSource                          mAudioSrc;

    public List<ClipAndGivenName>               mClips = new List<ClipAndGivenName>();

    public void FPlayClip(string name)
    {
        for(int i=0; i<mClips.Count; i++){
            if(mClips[i].mName == name){
                mAudioSrc.clip = mClips[i].mClip;
                mAudioSrc.Play();
            }
        }
    }

    public void FPlayWhistle()
    {
        FPlayClip("Whistle");
    }

    public void FPlayOver(bool bRecCaught, bool bInt)
    {
        if(bInt){
            FPlayClip("PD0");
        }else{
            if(bRecCaught){
                FPlayClip("PLY_Good0");
            }else{
                FPlayClip("PLY_Fail0");
            }
        }
    }
}
