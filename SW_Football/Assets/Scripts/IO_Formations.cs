/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.IO;

public class DATA_Formation
{
    public string                       mName;
    public string[]                     mTags;
    public Vector2[]                    mSpots;
}
public static class IO_Formations
{
    
    public static void FWRITE_FORMATION(DATA_Formation f)
    {
        if(f.mSpots.Length != f.mTags.Length){
            Debug.Log("ERROR SAVING FORMATION, spot tag length mismatch");
            return;
        }

        string sName = f.mName;
        StreamWriter sw = new StreamWriter(Application.dataPath+"/FILE_IO/Formations/"+sName+".txt");
        sw.WriteLine("NAME");
        sw.WriteLine(f.mName);
        sw.WriteLine("NUM PLAYERS");
        sw.WriteLine(f.mSpots.Length);
        sw.WriteLine("POSITIONS AND TAGS");
        for(int i=0; i<f.mSpots.Length; i++)
        {
            sw.WriteLine(f.mTags[i]);
            sw.WriteLine(UT_Strings.FConvertVecToString(f.mSpots[i]));
        }

        sw.Close();
    }

    public static DATA_Formation FLOAD_FORMATION(string sName)
    {
        string sPath = Application.dataPath+"/FILE_IO/Formations/"+sName+".txt";
        string[] sLines = System.IO.File.ReadAllLines(sPath);

        DATA_Formation f = new DATA_Formation();

        for(int i=0; i<sLines.Length; i++)
        {
            if(sLines[i].Contains("NUM PLAY")){
                int numPlayers = int.Parse(sLines[i+1]);
                f.mTags = new string[numPlayers];
                f.mSpots = new Vector2[numPlayers];
            }
        }

        for(int i=0; i<sLines.Length; i++)
        {
            if(sLines[i].Contains("NAME")){
                f.mName = sLines[i+1];
            }

            if(sLines[i].Contains("POSITIONS AND")){
                int ind = i + 1;
                for(int j=0; j<f.mTags.Length; j++){
                    f.mTags[j] = sLines[ind++];
                    f.mSpots[j] = UT_Strings.FGetVecFromString(sLines[ind++]);
                }
            }
        }

        return f;
    }
}
