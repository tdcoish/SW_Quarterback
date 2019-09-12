/*************************************************************************************
For now just async loads, then switches the scenes. 

Also, muffles the music, like we're in the pause screen.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;

public class PP_QUIT_Man : MonoBehaviour
{

    private AsyncOperation          mAsyncOp;
    private AsyncOperation          mAsyncVolume;

    public AudioMixer               refAudioMixer;
    public AudioMixerSnapshot       SNP_Normal;
    public AudioMixerSnapshot       SNP_Paused;

    private bool                    mAudioQuieted = false;

    void Start()
    {
        SNP_Paused.TransitionTo(0.1f);
        PROJ_Football[] fbs = FindObjectsOfType<PROJ_Football>();
        foreach(PROJ_Football f in fbs){
            Destroy(f.gameObject);
        }
        PP_Projectile[] pps = FindObjectsOfType<PP_Projectile>();
        foreach(PP_Projectile p in pps){
            Destroy(p.gameObject);
        }
        StartCoroutine(LOAD_MAIN_MENU_ASYNC());
    }

    IEnumerator LOAD_MAIN_MENU_ASYNC()
    {
        mAsyncOp = SceneManager.LoadSceneAsync("SN_MN_Main", LoadSceneMode.Single);
        mAsyncOp.allowSceneActivation = false;

        while(!mAsyncOp.isDone)
        {
            if(true)
            {
                // Debug.Log("Audio quieted");
                mAsyncOp.allowSceneActivation = true;
            }

            yield return null;
        }

        Debug.Log("Level Loaded");
    }
}
