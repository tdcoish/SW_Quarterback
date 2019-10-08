/*************************************************************************************
Always start multiple from 0. eg. TD0, TD1, FG_MISS0, etcetera.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

// Not the official name of the clip, eg. SFX_AN_Ready4, but my given one, which might be eg. PreSnap1
[System.Serializable]
public class ClipAndGivenName
{
    public string                                               mName;
    public AudioClip                                            mClip;
}

public class AD_Exhibition : MonoBehaviour
{
    public AudioSource                          mAnnouncer;

    public List<ClipAndGivenName>               mClips = new List<ClipAndGivenName>();

    public void FPlayClip(string name)
    {
        for(int i=0; i<mClips.Count; i++){
            if(mClips[i].mName == name){
                mAnnouncer.clip = mClips[i].mClip;
                mAnnouncer.Play();
            }
        }
    }

    public void FTurnover()
    {
        int validClips = FindNumValidClips("TURN");
        
        int rand = Random.Range(0, validClips);
        string clipName = "TURN" + rand.ToString();
        FPlayClip(clipName);
    }

    public void FTouchDown()
    {
        int validClips = FindNumValidClips("TD");

        int rand = Random.Range(0, validClips);
        string clipName = "TD" + rand.ToString();
        FPlayClip(clipName);
    }

    public void FFirstDown()
    {
        string clipName = "PLY_SUC0";
        FPlayClip(clipName);
    }

    public void FFieldGoal(bool success)
    {
        string clipName = "";
        if(success){
            clipName = "FG_Good";
        }else{
            clipName = "FG_Bad";
        }
        FPlayClip(clipName);
    }

    public void FOverTime()
    {
        FPlayClip("OVERTIME");
    }

    public void FGameOver()
    {
        string clipName = "END_GAME";
        FPlayClip(clipName);
    }

    private int FindNumValidClips(string tag)
    {
        int validClips = 0;
        for(int i=0; i<mClips.Count; i++)
        {
            if(mClips[i].mName.Contains(tag)){
                validClips++;
            }
        }
        return validClips;
    }
}
