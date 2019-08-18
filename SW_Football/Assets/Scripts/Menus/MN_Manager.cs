/*************************************************************************************
For now, it just plays some music, then you can select the one and only game mode.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MN_Manager : MonoBehaviour
{

    public AudioSource              mMusBackground;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPressedPocketPasser()
    {
        SceneManager.LoadScene("SN_PocketPasser");
    }

    public void OnPressedQuit()
    {
        Debug.Log("Tried to quit");
        Application.Quit();
    }
}
