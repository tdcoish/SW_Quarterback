/*************************************************************************************
Outputs the application.dataPath
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class TEST_Build : MonoBehaviour
{
    public Text             mTXT;
    void Start()
    {
        mTXT.text = "PATH: " + Application.dataPath;
    }
}
