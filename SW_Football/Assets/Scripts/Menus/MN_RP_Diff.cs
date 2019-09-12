/*************************************************************************************
Difficulty selection.
*************************************************************************************/
using UnityEngine;

public class MN_RP_Diff : MonoBehaviour
{
    public void BT_Rookie()
    {
        RP_GB_Diff.mDif = "Rookie";
        FindObjectOfType<MN_Manager>().BT_RoutePasserReady();
    }
    public void BT_Normie()
    {
        RP_GB_Diff.mDif = "Normie";
        FindObjectOfType<MN_Manager>().BT_RoutePasserReady();
    }   
    public void BT_Sexy()
    {
        RP_GB_Diff.mDif = "Sexy";
        FindObjectOfType<MN_Manager>().BT_RoutePasserReady();
    }
    public void BT_Peterman()
    {
        RP_GB_Diff.mDif = "Peterman";
        FindObjectOfType<MN_Manager>().BT_RoutePasserReady();
    }


}
