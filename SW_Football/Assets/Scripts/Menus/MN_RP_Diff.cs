/*************************************************************************************
Difficulty selection.
*************************************************************************************/
using UnityEngine;

public class MN_RP_Diff : MonoBehaviour
{
    public void BT_Rookie()
    {
        IO_RP_Dif.mDifSelected = "Rookie";
        FindObjectOfType<MN_Manager>().BT_RoutePasserReady();
    }
    public void BT_Normie()
    {
        IO_RP_Dif.mDifSelected = "Normie";
        FindObjectOfType<MN_Manager>().BT_RoutePasserReady();
    }   
    public void BT_Sexy()
    {
        IO_RP_Dif.mDifSelected = "Sexy";
        FindObjectOfType<MN_Manager>().BT_RoutePasserReady();
    }
    public void BT_Peterman()
    {
        IO_RP_Dif.mDifSelected = "Peterman";
        FindObjectOfType<MN_Manager>().BT_RoutePasserReady();
    }


}
