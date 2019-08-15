/*************************************************************************************
Start simple.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PP_Manager : MonoBehaviour
{
    
    public PP_UI                refUI;


    public void OnTargetHit()
    {
        refUI.TXT_Instr.text = "Congrats";
    }
}
