/*************************************************************************************
The loading screen for pocket passer. Async loads, pretty simple.

Kind of hacky that I keep doing the whole PP_Loaded every frame that it's done, but whatever.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MN_PP_LOAD : MonoBehaviour
{

    public Button               mPlayBTN;
    public Text                 mLoadTXT;

    public bool                 mLoaded = false;

    private AsyncOperation      mAsyncOp;

    void Start()
    {
        PP_Unloaded();

        StartCoroutine(LOADPPAsync());
    }

    void Update()
    {
        
    }


    IEnumerator LOADPPAsync()
    {
        mAsyncOp = SceneManager.LoadSceneAsync("SN_PocketPasser", LoadSceneMode.Single);
        mAsyncOp.allowSceneActivation = false;

        while(!mAsyncOp.isDone)
        {
            mLoadTXT.text = "LOADING... " + mAsyncOp.progress * 100f;

            if(mAsyncOp.progress >= 0.9f)
            {
                if(!mLoaded){
                    PP_Loaded();
                }
            }

            yield return null;
        }


    }

    private void PP_Unloaded()
    {
        mPlayBTN.gameObject.SetActive(false);
        mLoadTXT.gameObject.SetActive(true); 
    }

    private void PP_Loaded()
    {
        mLoaded = true;
        mPlayBTN.gameObject.SetActive(true);
        mLoadTXT.gameObject.SetActive(false);
    }

    public void OnPressedPlay()
    {
        Debug.Log("They pressed play!");
        mAsyncOp.allowSceneActivation = true;
    }
}
