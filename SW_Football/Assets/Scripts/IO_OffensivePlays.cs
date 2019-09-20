/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.IO;
using System.Collections.Generic;

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

    public static DATA_OffPlay FLoadPlay(string name)
    {
        string path = Application.dataPath+"/FILE_IO/OffensivePlays/"+name+".txt";
        string[] sLines = System.IO.File.ReadAllLines(path);

        DATA_OffPlay p = new DATA_OffPlay();
        
        for(int i=0; i<sLines.Length; i++)
        {
            if(sLines[i].Contains("NUM PLAYERS")){
                int numPlayers = int.Parse(sLines[i+1]);
                p.mTags = new string[numPlayers];
                p.mRoles = new string[numPlayers];
            }
        }

        for(int i=0; i<sLines.Length; i++)
        {
            if(sLines[i].Contains("NAME")){
                p.mName = sLines[i+1];
            }

            if(sLines[i].Contains("NUM PLAYERS")){
                int ix = i+2;
                for(int j=0; j<p.mTags.Length; j++){
                    p.mTags[j] = sLines[ix++];
                    p.mRoles[j] = sLines[ix++];
                }
            }

            if(sLines[i].Contains("NUM RECEIVERS")){
                int num = int.Parse(sLines[i+1]);
                int ix = i+3;
                for(int j=0; j<num; j++)
                {
                    DATA_ORoute r = new DATA_ORoute();
                    r.mOwner = sLines[ix++];
                    ix++;       // skip over NUM SPOTS line
                    int numSpots = int.Parse(sLines[ix++]);
                    r.mSpots = new List<Vector2>();
                    for(int k=0; k<numSpots; k++){
                        Vector2 v = UT_Strings.FGetVecFromString(sLines[ix++]);
                        r.mSpots.Add(v);
                    }

                    p.mRoutes.Add(r);
                }
            }
        }

        return p;
    }
}
