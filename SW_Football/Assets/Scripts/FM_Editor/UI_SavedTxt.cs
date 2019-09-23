/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class UI_SavedTxt : MonoBehaviour
{
    public Text                     mMsg;

    void Start(){
        mMsg = GetComponent<Text>();
    }
    void Update()
    {
        Color c = mMsg.color;
        c.a -= Time.deltaTime * 1f;
        mMsg.color = c;
    }

    public void FSetVisible(){
        Color c = mMsg.color;
        c.a = 1f;
        mMsg.color = c;
    }
}
