/*************************************************************************************
All the settings in the game, or at least all of them right now, get saved to and loaded
from a single text file.
*************************************************************************************/
using UnityEngine;
using System.IO;

[System.Serializable]
public class DATA_Player
{
    public float                mAccRate = 50f;
    public float                mMoveSpd = 5f;
    public float                mShiftChargeSlow = 3f;      // how much slower they charge the throw power with shift
    public float                mThrowChargeTime = 2f;
    public float                mThrowSpd = 25f;
}

[System.Serializable]
public class DATA_Settings
{
    public DATA_Player      lPlayerData;

    public float            lInaccuracyBias;
    public float            lLookPenalty;
    public float            lLookSensitity;
    public float            lMasterVolume;
    public float            lMovementPenalty;
}

public static class IO_Settings
{
    public static DATA_Settings             mSet;

    public static void FLOAD_SETTINGS()
    {
        string path = Application.dataPath+"/Settings/Settings.txt";

        StreamReader sr = new StreamReader(path);
        string sLine = sr.ReadLine();
        
        mSet = new DATA_Settings();
        // First we load the player data.
        mSet.lPlayerData = new DATA_Player();
        sLine = sr.ReadLine();
        int k = sLine.IndexOf(':') + 1;
        mSet.lPlayerData.mAccRate = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        mSet.lPlayerData.mMoveSpd = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        mSet.lPlayerData.mShiftChargeSlow = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        mSet.lPlayerData.mThrowChargeTime = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        mSet.lPlayerData.mThrowSpd = float.Parse(sLine.Substring(k));

        // ------------------------------ Everything else
        sr.ReadLine();
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        mSet.lInaccuracyBias = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        mSet.lLookPenalty = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        mSet.lLookSensitity = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        mSet.lMasterVolume = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        mSet.lMovementPenalty = float.Parse(sLine.Substring(k));

    }

    public static void FWRITE_SETTINGS()
    {
        string path = Application.dataPath+"/Settings/Settings.txt";
        StreamWriter sw = new StreamWriter(path);
        string sLine = "Player";
        sw.WriteLine(sLine);
        sw.WriteLine("Acc:" + mSet.lPlayerData.mAccRate.ToString());
        sw.WriteLine("Spd:" + mSet.lPlayerData.mMoveSpd.ToString());
        sw.WriteLine("Charge Slow:" + mSet.lPlayerData.mShiftChargeSlow.ToString());
        sw.WriteLine("Charge Time:" + mSet.lPlayerData.mThrowChargeTime.ToString());
        sw.WriteLine("Throw Power:" + mSet.lPlayerData.mThrowSpd.ToString());
        sw.WriteLine("------------------------------------");
        sw.WriteLine("Look Inaccuracy Y Axis Bias:" + mSet.lInaccuracyBias.ToString());
        sw.WriteLine("Look Penalty:" + mSet.lLookPenalty.ToString());
        sw.WriteLine("Look Sensitivity:" + mSet.lLookSensitity.ToString());
        sw.WriteLine("Master Volume:" + mSet.lMasterVolume.ToString());
        sw.WriteLine("Movement Penalty:" + mSet.lMovementPenalty.ToString());

        sw.Close();
    }

}










