/*************************************************************************************
The manager for the actual on field game itself.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAN_Sport : MonoBehaviour
{
    private float               mTime;
    void Start()
    {
        mTime = Time.time;
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("SN_MN_Main");
        }
        if(Time.time - mTime > 4f)
        {
            SceneManager.LoadScene("SN_MN_Main");
        }
    }
}
