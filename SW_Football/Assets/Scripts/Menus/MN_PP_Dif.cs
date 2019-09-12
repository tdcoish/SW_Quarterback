/*************************************************************************************
Basically the same shit.
*************************************************************************************/
using UnityEngine;

public class MN_PP_Dif : MonoBehaviour
{
    public void BT_Rookie()
    {
        IO_PP_Dif.mDif = "EASY";
        FindObjectOfType<MN_Manager>().BT_PocketPasserReady();
    }
    public void BT_Normie()
    {
        IO_PP_Dif.mDif = "NORMAL";
        FindObjectOfType<MN_Manager>().BT_PocketPasserReady();
    }   
    public void BT_Sexy()
    {
        IO_PP_Dif.mDif = "HARD";
        FindObjectOfType<MN_Manager>().BT_PocketPasserReady();
    }
    public void BT_Peterman()
    {
        IO_PP_Dif.mDif = "PETERMAN";
        FindObjectOfType<MN_Manager>().BT_PocketPasserReady();
    }
}
