/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.IO;

public static class IO_PP_Dif 
{
    public static DATA_PP_Dif                       mDif;

    public static DATA_PP_Dif FGetCurrent()
    {
        DATA_PP_Dif dif = new DATA_PP_Dif();

        dif.mName = "EASY";

        dif.mPocketScale = Object.FindObjectOfType<PP_Pocket>().transform.localScale;
        dif.mTimeBetweenTargetChanges = 5f;
        
        PP_Turret[] turs = Object.FindObjectsOfType<PP_Turret>();
        dif.mTurretSpots = new Vector3[turs.Length];
        dif.mNumTurrets = turs.Length;
        for(int i=0; i<turs.Length; i++){
            dif.mTurretSpots[i] = turs[i].transform.position;
        }
        dif.mTurretFireRate = 2f;

        PP_Target[] targs = Object.FindObjectsOfType<PP_Target>();
        dif.mNumTargets = targs.Length;
        dif.mTargetSpots = new Vector3[targs.Length];
        for(int i=0; i<targs.Length; i++){
            dif.mTargetSpots[i] = targs[i].transform.parent.position;
        }
        dif.mTargetScales = new Vector3[targs.Length];
        for(int i=0; i<targs.Length; i++){
            dif.mTargetScales[i] = targs[i].transform.parent.localScale;
        }

        dif.mBronzeTrophy = 200;
        dif.mSilverTrophy = 1000;
        dif.mGoldTrophy = 1800;

        return dif;
    }

    public static void FSaveCurrent(DATA_PP_Dif dif)
    {

        string path = Application.dataPath + "/FILE_IO/PocketPasser/" + dif.mName + ".txt";

        StreamWriter sw = new StreamWriter(path);
        sw.WriteLine("Difficulty:" + dif.mName);
        sw.WriteLine("Pocket Scale:" + dif.mPocketScale);
        sw.WriteLine("TIME_BETWEEN_TARGET_CHANGES:" + dif.mTimeBetweenTargetChanges);
        sw.WriteLine("NUM_TURRETS:" + dif.mNumTurrets);
        foreach(Vector3 spot in dif.mTurretSpots){
            sw.WriteLine(UT_Strings.FConvertVecToString(spot));
        }
        sw.WriteLine("TURRET_FIRE_RATE:" + dif.mTurretFireRate);
        sw.WriteLine("NUM_TARGETS:" + dif.mNumTargets);
        foreach(Vector3 spot in dif.mTargetSpots){
            sw.WriteLine(UT_Strings.FConvertVecToString(spot));
        }
        foreach(Vector3 scale in dif.mTargetScales){
            sw.WriteLine(UT_Strings.FConvertVecToString(scale));
        }
        sw.WriteLine("BRONZE:" + dif.mBronzeTrophy);
        sw.WriteLine("SILVER:" + dif.mSilverTrophy);
        sw.WriteLine("GOLD:" + dif.mGoldTrophy);

        sw.Close();
    }

    public static DATA_PP_Dif FLoadDifficulty(string sName)
    {
        string sPath = Application.dataPath + "/FILE_IO/PocketPasser/" + sName + ".txt";

        DATA_PP_Dif dif = new DATA_PP_Dif();

        StreamReader sr = new StreamReader(sPath);
        string sLine = sr.ReadLine();
        int k = sLine.IndexOf(':') + 1;
        dif.mName = sLine.Substring(k);
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        dif.mPocketScale = UT_Strings.FGetVec3FromString(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        dif.mTimeBetweenTargetChanges = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        dif.mNumTurrets = int.Parse(sLine.Substring(k));
        dif.mTurretSpots = new Vector3[dif.mNumTurrets];
        for(int i=0; i<dif.mNumTurrets; i++)
        {
            sLine = sr.ReadLine();
            dif.mTurretSpots[i] = UT_Strings.FGetVec3FromString(sLine);
            Debug.Log(dif.mTurretSpots[i]);
        }
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        dif.mTurretFireRate = float.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        dif.mNumTargets = int.Parse(sLine.Substring(k));
        dif.mTargetScales = new Vector3[dif.mNumTargets];
        dif.mTargetSpots = new Vector3[dif.mNumTargets];
        for(int i=0; i<dif.mNumTargets; i++)
        {
            sLine = sr.ReadLine();
            dif.mTargetSpots[i] = UT_Strings.FGetVec3FromString(sLine);
        }
        for(int i=0; i<dif.mNumTargets; i++)
        {
            sLine = sr.ReadLine();
            dif.mTargetScales[i] = UT_Strings.FGetVec3FromString(sLine);
        }
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        dif.mBronzeTrophy = int.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        dif.mSilverTrophy = int.Parse(sLine.Substring(k));
        sLine = sr.ReadLine();
        k = sLine.IndexOf(':') + 1;
        dif.mGoldTrophy = int.Parse(sLine.Substring(k));
        
        return dif;
    }

    
}
