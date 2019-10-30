/*************************************************************************************
The file that saves and loads all the RP sets.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class IO_RP
{

    public static void FWriteSet(DT_RP_Set set)
    {   
        // ------------------------- ERROR Checking
        if(set.mName == "NAME ME"){
            Debug.Log("ERROR. No name given");
            return;
        }

        if(set.mHoops.Count != set.mRecs.Count || set.mRecs.Count != set.mRoutes.Count){
            Debug.Log("ERROR. Number of receivers, hoops, and routes do not match");
            return;
        }

        Debug.Log("Things should be saving");

        // Huh, I kind of want to not have to give unique names all the time.
        StreamWriter sw = new StreamWriter(Application.dataPath+"/FILE_IO/RoutePasser/"+set.mName+".txt");
        sw.WriteLine("NAME");
        sw.WriteLine(set.mName);
        sw.WriteLine("Num Recs:");
        sw.WriteLine(set.mRecs.Count);
        // You know, I really might as well just straight up write all at once.
        for(int i=0; i<set.mRecs.Count; i++)
        {
            sw.WriteLine(UT_Strings.FConvertVecToString(set.mRecs[i].mStart));
            sw.WriteLine(set.mRecs[i].mTag);
        }
        sw.WriteLine("Num Hoops:");
        sw.WriteLine(set.mHoops.Count);
        for(int i=0; i<set.mHoops.Count; i++)
        {
            sw.WriteLine(UT_Strings.FConvertVecToString(set.mHoops[i].mStart));
            sw.WriteLine(set.mHoops[i].mSize); 
            sw.WriteLine(set.mHoops[i].mTag);       
        }
        sw.WriteLine("Num Routes:");
        sw.WriteLine(set.mRoutes.Count);
        for(int i=0; i<set.mRoutes.Count; i++)
        {
            sw.WriteLine(set.mRoutes[i].mOwner);
            sw.WriteLine(set.mRoutes[i].mSpots.Count);
            for(int j=0; j<set.mRoutes[i].mSpots.Count; j++){
                sw.WriteLine(UT_Strings.FConvertVecToString(set.mRoutes[i].mSpots[j]));
            }
        }

        sw.Close();
    }

    public static DT_RP_Set FLoadSet(string name)
    {
        string path = Application.dataPath+"/FILE_IO/RoutePasser/"+name+".txt";
        string[] sLines = System.IO.File.ReadAllLines(path);

        DT_RP_Set set = new DT_RP_Set();

        for(int i=0; i<sLines.Length; i++){
            if(sLines[i].Contains("NAME")){
                set.mName = sLines[i+1];
            }

            if(sLines[i].Contains("Num Recs")){
                int numRecs = int.Parse(sLines[i+1]);
                int ix = i+2;
                for(int j=0; j<numRecs; j++){
                    DT_RP_Rec r = new DT_RP_Rec();
                    r.mStart = UT_Strings.FGetVec3FromString(sLines[ix++]);
                    r.mTag = sLines[ix++];
                    set.mRecs.Add(r);
                }
            }

            if(sLines[i].Contains("Num Hoops")){
                int numHoops = int.Parse(sLines[i+1]);
                int ix = i+2;
                for(int j=0; j<numHoops; j++){
                    DT_RP_Hoop h = new DT_RP_Hoop();
                    h.mStart = UT_Strings.FGetVec3FromString(sLines[ix++]);
                    h.mSize = float.Parse(sLines[ix++]);
                    h.mTag = sLines[ix++];
                    set.mHoops.Add(h);
                }
            }

            if(sLines[i].Contains("Num Routes")){
                int numRoutes = int.Parse(sLines[i+1]);
                int ix = i+2;
                for(int j=0; j<numRoutes; j++){
                    DATA_ORoute r = new DATA_ORoute();
                    r.mOwner = sLines[ix++];
                    ix++;       // skip over NUM SPOTS line
                    int numSpots = int.Parse(sLines[ix++]);
                    r.mSpots = new List<Vector2>();
                    for(int k=0; k<numSpots; k++){
                        Vector2 v = UT_Strings.FGetVecFromString(sLines[ix++]);
                        r.mSpots.Add(v);
                    }

                    set.mRoutes.Add(r);
                }
            }
        }

        return set;
    }
}
