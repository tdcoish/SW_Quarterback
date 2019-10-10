/*************************************************************************************
Glad I'm doing this now. There are clear scaling issues going forwards.
These lists should be made in text files, not here, or in the editor.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class AD_Prac : MonoBehaviour
{
    public AudioSource                          mAnnouncer;
    public AudioSource                          mFX;    

    public List<ClipAndGivenName>               mAnnClips = new List<ClipAndGivenName>();
    public List<ClipAndGivenName>               mFXClips = new List<ClipAndGivenName>();

    public void FPlayClipAnnouncer(string name)
    {
        for(int i=0; i<mAnnClips.Count; i++){
            if(mAnnClips[i].mName == name){
                mAnnouncer.clip = mAnnClips[i].mClip;
                mAnnouncer.Play();
            }
        }
    }
    
    public void FPlayClipFX(string name)
    {
        for(int i=0; i<mFXClips.Count; i++){
            if(mFXClips[i].mName == name){
                mFX.clip = mFXClips[i].mClip;
                mFX.Play();
            }
        }
    }

    public void FPlayWhistle()
    {
        FPlayClipFX("Whistle");
    }

    public void FPlayOver(bool bRecCaught, bool bInt)
    {
        Debug.Log("Playing over");
        if(bInt){
            FPlayClipAnnouncer("PD0");
        }else{
            if(bRecCaught){
                FPlayClipAnnouncer("PLY_Good0");
            }else{
                FPlayClipAnnouncer("PLY_Fail0");
            }
        }
    }
}
