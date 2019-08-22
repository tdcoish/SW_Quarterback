/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class MN_QuitScreen : MonoBehaviour
{

    void Start()
    {
        Invoke("QuitGame", 1f);
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

}
