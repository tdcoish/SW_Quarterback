/*************************************************************************************
New manager for the practice offense scene.
*************************************************************************************/
using UnityEngine;

public class PRAC_Off_Man : MonoBehaviour
{
    private PRAC_Off_SetupPlayers               cPlayerSetup;


    public PLY_SnapSpot                         rSnapSpot;

    void Start()
    {
        IO_Settings.FLOAD_SETTINGS();

        cPlayerSetup = GetComponent<PRAC_Off_SetupPlayers>();    

        cPlayerSetup.FSetUpPlayers("Sail", rSnapSpot);
    }

    void Update()
    {
        
    }
}
