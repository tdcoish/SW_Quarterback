/*************************************************************************************
Basically just the screen manager for the play editor scene.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class PE_SN_Manager : MonoBehaviour
{
    public GameObject           mEditorScreen;
    public GameObject           mViewerScreen;

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
