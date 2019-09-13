/*************************************************************************************
This is entirely about the gamer themselves.
*************************************************************************************/
using UnityEngine;
using System.IO;


public static class IO_GamerInfo
{

    public static DATA_Gamer                  mInfo;

    public static void FWriteGamerData(DATA_Gamer dt)
    {
        string sPath = Application.dataPath + "/FILE_IO/GamerInfo/info.txt";
        StreamWriter sw = new StreamWriter(sPath);

        sw.WriteLine("GAMER");
        sw.WriteLine("Timothy");
        sw.WriteLine("POCKET PASSER SCORES, EASY TO PETERMAN");
        sw.WriteLine(dt.mPPHighScores.mEasyScore);
        sw.WriteLine(dt.mPPHighScores.mNormalScore);
        sw.WriteLine(dt.mPPHighScores.mHardScore);
        sw.WriteLine(dt.mPPHighScores.mPetermanScore);
        sw.WriteLine("PITCH AND CATCH SCORES, EASY TO PETERMAN");
        sw.WriteLine(dt.mRPHighScores.mEasyScore);
        sw.WriteLine(dt.mRPHighScores.mNormalScore);
        sw.WriteLine(dt.mRPHighScores.mHardScore);
        sw.WriteLine(dt.mRPHighScores.mPetermanScore);
        sw.WriteLine("PASSES THROWN:");
        sw.WriteLine(dt.mPassesThrown);

        sw.Close();
    }

    public static DATA_Gamer FLoadGamerData()
    {
        string sPath = Application.dataPath + "/FILE_IO/GamerInfo/info.txt";
        string[] sLines = System.IO.File.ReadAllLines(sPath);

        DATA_Gamer dt = new DATA_Gamer();
        dt.mRPHighScores = new MinigameAchievement();
        dt.mPPHighScores = new MinigameAchievement();

        for(int i=0; i<sLines.Length; i++){
            if(sLines[i].Contains("POCKET")){
                dt.mPPHighScores.mEasyScore = int.Parse(sLines[i+1]);
                dt.mPPHighScores.mNormalScore = int.Parse(sLines[i+2]);
                dt.mPPHighScores.mHardScore = int.Parse(sLines[i+3]);
                dt.mPPHighScores.mPetermanScore = int.Parse(sLines[i+4]);
            }
            if(sLines[i].Contains("PASSES")){
                dt.mPassesThrown = int.Parse(sLines[i+1]);
            }
            if(sLines[i].Contains("PITCH")){
                dt.mRPHighScores.mEasyScore = int.Parse(sLines[i+1]);
                dt.mRPHighScores.mNormalScore = int.Parse(sLines[i+2]);
                dt.mRPHighScores.mHardScore = int.Parse(sLines[i+3]);
                dt.mRPHighScores.mPetermanScore = int.Parse(sLines[i+4]);
            }
        }

        return dt;
    }
}
