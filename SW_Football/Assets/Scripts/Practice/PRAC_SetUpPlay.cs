/*************************************************************************************
Called by the manager, when it's time to set up a play.

1) Instantiate all players, shove their responsibilities in there.
2) Re-arrange the array, by left->right.
3) Implement their details.

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class PRAC_SetUpPlay : MonoBehaviour
{

    public PRAC_Off             PF_OffensivePlayer;
    public PRAC_Def             PF_DefensivePlayer;

    // Gonna have to delete these guys as well.
    public GameObject           PF_RouteNode;

    // For now we are ignoring the defensive play name.
    public void FSetUpPlay(string sOffName, string sDefName, PLY_SnapSpot rSnapSpot)
    {
        // ------------------------------- LOAD IN PLAYERS
        DATA_Play playToRun = IO_PlayList.FLOAD_PLAY_BY_NAME(sOffName);

        if(playToRun == null){
            Debug.Log("No play of that name");
        }

        // Now we just pretend there were no issues, so we put our players in their spots.
        for(int i=0; i<playToRun.mPlayerRoles.Length; i++)
        {
            Vector3 vPlayerSpot = new Vector3();
            vPlayerSpot.x = playToRun.mPlayerRoles[i].mStart.x;
            vPlayerSpot.z = playToRun.mPlayerRoles[i].mStart.y;
            vPlayerSpot += rSnapSpot.transform.position;
            var clone = Instantiate(PF_OffensivePlayer, vPlayerSpot, transform.rotation);
            clone.mJob.mTag = playToRun.mPlayerRoles[i].mTag;
            clone.mJob.mRole = playToRun.mPlayerRoles[i].mRole;
            clone.mJob.mDetail = playToRun.mPlayerRoles[i].mDetail;
        }

        int randomDefPlay = Random.Range(0, IO_DefPlays.mPlays.Length);
        // DATA_Play defPlay = IO_DefPlays.mPlays[randomDefPlay];
        DATA_Play defPlay = IO_DefPlays.FLOAD_PLAY_BY_NAME(sDefName);
        PRAC_UI ui = FindObjectOfType<PRAC_UI>();
        ui.mDefensivePlayName.text = defPlay.mName;
        
        // spawn a defensive player according to the play.
        for(int i=0; i<defPlay.mPlayerRoles.Length; i++)
        {
            Vector3 vPlayerSpot = new Vector3();
            vPlayerSpot.x = defPlay.mPlayerRoles[i].mStart.x;
            vPlayerSpot.z = defPlay.mPlayerRoles[i].mStart.y;
            vPlayerSpot += rSnapSpot.transform.position;
            var clone = Instantiate(PF_DefensivePlayer, vPlayerSpot, transform.rotation);
            clone.mJob.mTag = defPlay.mPlayerRoles[i].mTag;
            clone.mJob.mRole = defPlay.mPlayerRoles[i].mRole;
            clone.mJob.mDetail = defPlay.mPlayerRoles[i].mDetail;
        }

        // --------------------------------------------- Set all athletes to waiting for snap.
        PRAC_Ath[] athletes = FindObjectsOfType<PRAC_Ath>();
        foreach(PRAC_Ath athlete in athletes)
        {
            athlete.mState = PRAC_Ath.PRAC_ATH_STATE.SPRE_SNAP;
        }


        // --------------------------------------------- Re-arrange offensive and defensive player pointers left->right.
        PRAC_Off[] offenders = FindObjectsOfType<PRAC_Off>();
        PRAC_Def[] defenders = FindObjectsOfType<PRAC_Def>();
        SortPlayersLeftToRight(offenders);
        SortPlayersLeftToRight(defenders);


        // -------------------------------------- For route runners, load in their routes.
        IO_RouteList.FLOAD_ROUTES();
        for(int i=0; i<offenders.Length; i++)
        {
            if(offenders[i].mJob.mRole == "Route")
            {
                // Yep, memory not allocated error if we don't do this here.
                offenders[i].GetComponent<OFF_RouteLog>().mRouteSpots = new List<Vector3>();

                DATA_Route rt = IO_RouteList.FLOAD_ROUTE_BY_NAME(offenders[i].mJob.mDetail);
                foreach (Vector2 routeSpot in rt.mSpots)
                {
                    Vector3 vSpot = offenders[i].transform.position;
                    vSpot.x += routeSpot.x; vSpot.z += routeSpot.y;
                    // Also, shove the spot into the receiver, just for now.
                    offenders[i].GetComponent<OFF_RouteLog>().mRouteSpots.Add(vSpot);
                    // Instantiate(PF_RouteNode, vSpot, transform.rotation);
                }
            }
        }

        // ---------------------------------------------------------- Shove the player "into" the QB character.
        for(int i=0; i<offenders.Length; i++)
        {
            if(offenders[i].mJob.mTag == "QB")
            {
                Vector3 vSpot = offenders[i].transform.position;
                vSpot.y = 1f;
                FindObjectOfType<PC_Controller>().transform.position = vSpot;
            }
        }

        // ------------------------------------------ Now we're assigning the man for man coverage. 
        // An offense has 5 eligible receivers on every play, so we actually SHOULD go by that, not their routes.
        // Update, we've added PRS_AssignMan which globally stores the order that tags should be covered, and the same for defense.

        List<PRAC_Off> elibibleRecs = new List<PRAC_Off>();
        foreach(PRAC_Off off in offenders)
        {
            if(PRS_AssignMan.FELIGIBLE_RECEIVER(off.mJob.mTag)){
                elibibleRecs.Add(off);
            }
        }

        elibibleRecs = PRS_AssignMan.FSORT_RECEIVER_IMPORTANCE(elibibleRecs);

        List<PRAC_Def> defsInMan = new List<PRAC_Def>();
        foreach(PRAC_Def def in defenders)
        {
            if(def.mJob.mRole == "Man")
            {
                Debug.Log("Added");
                defsInMan.Add(def);
            }
        }
        
        defsInMan = PRS_AssignMan.FSORT_DEFENDER_IMPORTANCE(defsInMan);

        // Now we finally start assigning man coverage responsibility.
        for(int i=0; i<defsInMan.Count; i++)
        {   
            if(elibibleRecs.Count <= 0){
                break;
            }

            defsInMan[i].GetComponent<DEF_ManLog>().rMan = elibibleRecs[0];
            elibibleRecs.RemoveAt(0);
        }

        // ---------- ALIGN MAN DEFENDERS TO THE GUY THEY'RE COVERING
        for(int i=0; i<defsInMan.Count; i++)
        {
            Vector3 vManPos = defsInMan[i].GetComponent<DEF_ManLog>().rMan.transform.position;
            vManPos.z += 7f;
            defsInMan[i].transform.position = vManPos;
        }

    }

    private void SortPlayersLeftToRight(PRAC_Off[] athletes)
    {
        for(int i=2; i<athletes.Length; i++)
        {
            for(int j=i; j>0; j--)
            {
                if(athletes[j].transform.position.x < athletes[j-1].transform.position.x)
                {
                    PRAC_Off temp = athletes[j];
                    athletes[j] = athletes[j-1];
                    athletes[j-1] = temp;
                }
            }
        }
    }

    private void SortPlayersLeftToRight(PRAC_Def[] athletes)
    {
        for(int i=2; i<athletes.Length; i++)
        {
            for(int j=i; j>0; j--)
            {
                if(athletes[j].transform.position.x < athletes[j-1].transform.position.x)
                {
                    PRAC_Def temp = athletes[j];
                    athletes[j] = athletes[j-1];
                    athletes[j-1] = temp;
                }
            }
        }
    }

}
