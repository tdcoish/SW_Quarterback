/*************************************************************************************
This is the manager for the whole scene.

For starters, just make a bunch of zones, so we have data to load in.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class DPC_Man : MonoBehaviour
{
    public GameObject           mEditorScreen;
    public GameObject           mViewerScreen;

    public DPC_ZoneCreator         cZoneCreator;
    private DPC_Selector                cSelector;
    private DPC_PlayDisplayer       cPlayDisplayer;

    // This is roughly what the scene state is.
    public enum PLAYMENU_STATE
    {
        SDISPLAYPLAY,
        SCREATINGPLAY
    }
    public PLAYMENU_STATE           mState;

    void Start()
    {
        cSelector = GetComponentInChildren<DPC_Selector>();
        cPlayDisplayer = GetComponentInChildren<DPC_PlayDisplayer>();

        IO_ZoneList.FLOAD_ZONES();
        mState = PLAYMENU_STATE.SDISPLAYPLAY;
    }

    void Update()
    {
        switch(mState)
        {
            case PLAYMENU_STATE.SCREATINGPLAY: RUN_Editor(); break;
            case PLAYMENU_STATE.SDISPLAYPLAY: RUN_PlayViewer(); break;
        }
    }

    private void RUN_Editor()
    {
        // mEditorScreen.SetActive(true);
        // mViewerScreen.SetActive(false);

        // cZoneCreator.FRun_Update();
        cSelector.FRun_Update();
        cPlayDisplayer.FDisplayPlay();
        cPlayDisplayer.FDisplayPlayerDetails();
    }

    private void RUN_PlayViewer()
    {
        // mEditorScreen.SetActive(false);
        // mViewerScreen.SetActive(true);

    }

    public void BT_NewPlay()
    {
        // for now, skip the different levels.
        mState = PLAYMENU_STATE.SCREATINGPLAY;
    }

    public void OnMainMenuSelected()
    {
        SceneManager.LoadScene("SN_MN_Main");
    }
}
