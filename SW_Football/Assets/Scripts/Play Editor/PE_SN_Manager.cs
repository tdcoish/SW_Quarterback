/*************************************************************************************
Basically just the screen manager for the play editor scene.

It will need to load in all the routes at the start of the scene.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class PE_SN_Manager : MonoBehaviour
{
    public GameObject           mEditorScreen;
    public GameObject           mViewerScreen;

    void Start()
    {
        IO_RouteList.FLOAD_ROUTES();
        // Not yet, since I haven't saved any
        // IO_PlayList.FLOAD_PLAYS();
    }

    public void OnViewScreenSelected()
    {
        mViewerScreen.SetActive(true);
        mEditorScreen.SetActive(false);
    }

    public void OnEditScreenSelected()
    {
        mEditorScreen.SetActive(true);
        mViewerScreen.SetActive(false);
    }

    public void OnMainMenuSelected()
    {
        SceneManager.LoadScene("SN_MN_Main");
    }
}
