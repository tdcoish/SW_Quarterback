/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.IO;

public static class IO_OffensivePlays
{
    
    public static void FWritePlay(DATA_OffPlay p)
    {
        // ----------------------------------- First do some error checking to make sure the play is valid.
        if(p.mName == "NAME ME"){
            Debug.Log("ERROR. This play must be given a unique name");
            return;
        }
        // Should also check against a list of valid tags. eg. RB1 valid, NO_TAG not valid.
        foreach(string s in p.mRoles){
            if(s == "NO ROLE"){
                Debug.Log("ERROR. One or more players without a role");
                return;
            }
        }
        
        foreach(string s in p.mTags){
            if(s == "NO TAG"){
                Debug.Log("ERROR. One or more players without a tag");
                return;
            }
        }

        if(p.mRoles.Length != p.mTags.Length){
            Debug.Log("ERROR. Tag/Role length mismatches");
            Debug.Log("Num tags: " + p.mTags.Length);
            Debug.Log("Num Roles: " + p.mRoles.Length);
            return;
        }

        foreach(DATA_ORoute r in p.mRoutes){
            bool ownerValid = false;
            foreach(string s in p.mTags){
                if(s == r.mOwner){
                    ownerValid = true;
                }
            }
            if(!ownerValid){
                Debug.Log("ERROR. One or more routes does not have a valid owner.");
                Debug.Log("OWNER: " + r.mOwner);
            }
        }

        // ---------------------------------- Save to disc
        string sName = p.mName;
        StreamWriter sw = new StreamWriter(Application.dataPath+"/FILE_IO/OffensivePlays/"+sName+".txt");
        sw.WriteLine("NAME");
        sw.WriteLine(p.mName);
        sw.WriteLine("NUM PLAYERS");
        sw.WriteLine(p.mTags.Length);
        for(int i=0; i<p.mTags.Length; i++){
            sw.WriteLine(p.mTags[i]);
            sw.WriteLine(p.mRoles[i]);
        }
        sw.WriteLine("NUM RECEIVERS");
        sw.WriteLine(p.mRoutes.Count);
        sw.WriteLine("ROUTES");
        for(int i=0; i<p.mRoutes.Count; i++){
            sw.WriteLine(p.mRoutes[i].mOwner);
            sw.WriteLine("NUM SPOTS");
            sw.WriteLine(p.mRoutes[i].mSpots.Count);
            for(int j=0; j<p.mRoutes[i].mSpots.Count; j++){
                sw.WriteLine(UT_Strings.FConvertVecToString(p.mRoutes[i].mSpots[j]));
            }
        }

        sw.Close();
    }
}
