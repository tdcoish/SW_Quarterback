/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MN_Play_LOAD : MonoBehaviour
{
    public Button               mPlayBTN;
    public Text                 mLoadTXT;

    public bool                 mLoaded = false;

    private AsyncOperation      mAsyncOp;

    void Start()
    {
        Play_Unloaded();

        StartCoroutine(LOADPPAsync());
    }

    IEnumerator LOADPPAsync()
    {
        mAsyncOp = SceneManager.LoadSceneAsync("SN_ShowMatchup", LoadSceneMode.Single);
        mAsyncOp.allowSceneActivation = false;

        while(!mAsyncOp.isDone)
        {
            mLoadTXT.text = "LOADING... " + mAsyncOp.progress * 100f;

            if(mAsyncOp.progress >= 0.9f)
            {
                if(!mLoaded){
                    Play_Loaded();
                }
            }

            yield return null;
        }


    }

    private void Play_Unloaded()
    {
        mPlayBTN.gameObject.SetActive(false);
        mLoadTXT.gameObject.SetActive(true); 
    }

    private void Play_Loaded()
    {
        mLoaded = true;
        mPlayBTN.gameObject.SetActive(true);
        mLoadTXT.gameObject.SetActive(false);
    }

    public void OnPressedPlay()
    {
        mAsyncOp.allowSceneActivation = true;
    }
}
