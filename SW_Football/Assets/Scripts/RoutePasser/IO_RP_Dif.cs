/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class IO_RP_Dif
{
    public static string                    mDifSelected = "NA";
    
    public static void FSaveSet(DATA_RP_Set set)
    {
        string sPath = Application.dataPath+"/FILE_IO/PitchAndCatch/"+set.mName+".txt";

        StreamWriter sw = new StreamWriter(sPath);
        sw.WriteLine("SET FOR PITCH AND CATCH");
        sw.WriteLine("NAME");
        sw.WriteLine(set.mName);
        sw.WriteLine("THROW TIME");
        sw.WriteLine(set.mTimeToThrow);
        sw.WriteLine("PC SPOT");
        sw.WriteLine(UT_Strings.FConvertVecToString(set.mPCSpot));
        sw.WriteLine("RECEIVER DATA");
        sw.WriteLine(set.mReceiverData.Count);
        for(int i=0; i<set.mReceiverData.Count; i++)
        {
            sw.WriteLine(set.mReceiverData[i].mTag);
            sw.WriteLine(set.mReceiverData[i].mRoute);
            sw.WriteLine(UT_Strings.FConvertVecToString(set.mReceiverData[i].mStartPos));
        }
        sw.WriteLine("RING DATA");
        sw.WriteLine(set.mRingData.Count);
        foreach(DATA_RP_Ring r in set.mRingData){
            sw.WriteLine(r.mTag);
            sw.WriteLine(UT_Strings.FConvertVecToString(r.mScale));
            sw.WriteLine(UT_Strings.FConvertVecToString(r.mDir));
            sw.WriteLine(UT_Strings.FConvertVecToString(r.mStartPos));
        }

        sw.Close();
    }

    public static DATA_RP_Set FLoadSet(string sName)
    {
        string sPath = Application.dataPath+"/FILE_IO/PitchAndCatch/"+sName+".txt";
        string[] sLines = System.IO.File.ReadAllLines(sPath);

        DATA_RP_Set s = new DATA_RP_Set();
        s.mReceiverData = new List<DATA_RP_Receiver>();
        s.mRingData = new List<DATA_RP_Ring>();
        for(int i=0; i<sLines.Length; i++)
        {
            if(sLines[i].Contains("NAME")){
                s.mName = sLines[i+1];
            }
            if(sLines[i].Contains("THROW")){
                s.mTimeToThrow = float.Parse(sLines[i+1]);
            }
            if(sLines[i].Contains("PC")){
                s.mPCSpot = UT_Strings.FGetVec3FromString(sLines[i+1]);
            }
            if(sLines[i].Contains("RECEIVER")){
                int num = int.Parse(sLines[i+1]);
                int startInd = i+2;
                for(int j=0; j<num; j++)
                {
                    startInd += j*3;
                    DATA_RP_Receiver r = new DATA_RP_Receiver();
                    r.mTag = sLines[startInd];
                    r.mRoute = sLines[startInd+1];
                    r.mStartPos = UT_Strings.FGetVec3FromString(sLines[startInd+2]);
                    s.mReceiverData.Add(r);
                }
            }
            if(sLines[i].Contains("RING")){
                int num = int.Parse(sLines[i+1]);
                int startInd = i+2;
                for(int j=0; j<num; j++)
                {
                    startInd += j*4;
                    DATA_RP_Ring r = new DATA_RP_Ring();
                    r.mTag = sLines[startInd];
                    r.mScale = UT_Strings.FGetVec3FromString(sLines[startInd+1]);
                    r.mDir = UT_Strings.FGetVec3FromString(sLines[startInd+2]);
                    r.mStartPos = UT_Strings.FGetVec3FromString(sLines[startInd+3]);
                    s.mRingData.Add(r);
                }
            }
        }

        return s;
    }
}
