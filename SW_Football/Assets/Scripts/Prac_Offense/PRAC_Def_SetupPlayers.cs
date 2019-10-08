/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class PRAC_Def_SetupPlayers : MonoBehaviour
{
    public PRAC_Def_Ply                                 PF_DefPlayer;

    public void FSetUpPlayers(string sDefName, PLY_SnapSpot rSnapSpot)
    {
        DATA_Play p = IO_DefPlays.FLOAD_PLAY_BY_NAME(sDefName);
        Debug.Log("About to run: " + p.mName);

        List<PRAC_Def_Ply> plys = new List<PRAC_Def_Ply>();

        // ------------------------- Old style still has the starting position in there already.
        DATA_Play defPlay = IO_DefPlays.FLOAD_PLAY_BY_NAME(sDefName);
        // PRAC_UI ui = FindObjectOfType<PRAC_UI>();
        // ui.mDefensivePlayName.text = defPlay.mName;
        
        for(int i=0; i<defPlay.mPlayerRoles.Length; i++)
        {
            Vector3 vPlayerSpot = new Vector3();
            vPlayerSpot.x = defPlay.mPlayerRoles[i].mStart.x;
            vPlayerSpot.z = defPlay.mPlayerRoles[i].mStart.y;
            vPlayerSpot += rSnapSpot.transform.position;
            var clone = Instantiate(PF_DefPlayer, vPlayerSpot, transform.rotation);
            clone.mJob.mTag = defPlay.mPlayerRoles[i].mTag;
            clone.mJob.mRole = defPlay.mPlayerRoles[i].mRole;
            clone.mJob.mDetail = defPlay.mPlayerRoles[i].mDetail;
        }

        // ----------------------- Figure out who's covering who.

        return;
    }
}
