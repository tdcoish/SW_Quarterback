/*************************************************************************************
For now just async loads, then switches the scenes. 

Also, quiets then increases music volume.
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
    public AudioMixerSnapshot       SNP_Quiet;

    private bool                    mAudioQuieted = false;

    void Start()
    {
        SNP_Quiet.TransitionTo(0.1f);
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
