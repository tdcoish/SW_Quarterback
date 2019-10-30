/*************************************************************************************
Responsible for displaying the possible tags for a receiver to use.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class ED_RP_Tag : MonoBehaviour
{
    public Text                         mCurTag;

    public void FSetCurTag(string tag)
    {
        mCurTag.text = "Current Tag: " + tag;
    }
}
