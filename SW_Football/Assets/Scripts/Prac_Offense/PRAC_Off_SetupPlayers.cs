/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class PRAC_Off_SetupPlayers : MonoBehaviour
{
    public PRAC_Off_Ply                                 PF_OffPlayer;

    public void FSetUpPlayers(string sOffName, PLY_SnapSpot rSnapSpot)
    {
        DATA_OffPlay p = IO_OffensivePlays.FLoadPlay(sOffName);
        Debug.Log("About to run: " + p.mName);

        DATA_Formation f = IO_Formations.FLOAD_FORMATION(p.mFormation);
        Debug.Log("From this formation: " + f.mName);

        List<PRAC_Off_Ply> plys = new List<PRAC_Off_Ply>();

        // --------------- Set up the players according to the formation.
        for(int i=0; i<f.mTags.Length; i++)
        {
            Vector3 vSpot = new Vector3();
            vSpot.x = f.mSpots[i].x;
            vSpot.z = f.mSpots[i].y * -1f;
            vSpot.y = 1f;
            vSpot += rSnapSpot.transform.position;
            var clone = Instantiate(PF_OffPlayer, vSpot, transform.rotation);
            clone.mTag = f.mTags[i];

            plys.Add(clone);
        }

        // ---------------- Now give the players their roles.
        for(int i=0; i<p.mTags.Length; i++)
        {
            for(int j=0; j<plys.Count; j++){
                if(plys[j].mTag == p.mTags[i]){
                    plys[j].mRole = p.mRoles[i];
                }
            }
        }

        // --------------- Assign all the routes to the proper receivers.
        for(int i=0; i<p.mRoutes.Count; i++)
        {
            for(int j=0; j<plys.Count; j++){
                if(plys[j].mTag == p.mRoutes[i].mOwner){
                    plys[j].mSpots = p.mRoutes[i].mSpots;
                    for(int k=0; k<plys[j].mSpots.Count; k++)
                    {
                        Vector2 v = plys[j].mSpots[k];
                        v.y *= -1f;
                        plys[j].mSpots[k] = v;
                    }
                }
            }
        }
        
        // --------------------------- Shove the player "into" the QB position.
        for(int i=0; i<plys.Count; i++){
            if(plys[i].mTag == "QB"){
                FindObjectOfType<PC_Controller>().transform.position = plys[i].transform.position;
            }
        }

        return;
    }
}
