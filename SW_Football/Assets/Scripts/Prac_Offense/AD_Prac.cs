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

    public void FInterception()
    {
        int validClips = FindNumValidClips("INT", mAnnClips);
        
        int rand = Random.Range(0, validClips);
        string clipName = "INT" + rand.ToString();
        FPlayClipAnnouncer(clipName);
    }
    public void FTackle()
    {
        int validClips = FindNumValidClips("TACKLE", mAnnClips);
        
        int rand = Random.Range(0, validClips);
        string clipName = "TACKLE" + rand.ToString();
        FPlayClipAnnouncer(clipName);
    }
    public void FCatch()
    {
        int validClips = FindNumValidClips("CATCH", mAnnClips);
        
        int rand = Random.Range(0, validClips);
        string clipName = "CATCH" + rand.ToString();
        FPlayClipAnnouncer(clipName);
    }

    public void FPlayWhistle()
    {
        FPlayClipFX("Whistle");
    }

    public void FPlayOver(PRAC_PlayInfo info)
    {
        if(info.mWasCatch){
            FPlayClipAnnouncer("PLY_Good0");
        }else if(info.mWasIncompletion){
            FPlayClipAnnouncer("PLY_Fail0");
        }else if(info.mWasInterception){
            FPlayClipAnnouncer("PLY_Fail0");
        }
    }

    private int FindNumValidClips(string tag, List<ClipAndGivenName> clipList)
    {
        int validClips = 0;
        for(int i=0; i<clipList.Count; i++)
        {
            if(clipList[i].mName.Contains(tag)){
                validClips++;
            }
        }
        return validClips;
    }
}
