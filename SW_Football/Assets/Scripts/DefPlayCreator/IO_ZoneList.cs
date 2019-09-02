/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.IO;

// For now a zone is just a spot with a name.
[System.Serializable]
public class DATA_Zone
{
    public string               mName;
    public Vector2              mSpot;
}

public static class IO_ZoneList
{
    public static DATA_Zone[]               mZones;

    // Will return false if the write was a failure.
    public static bool FWRITE_ZONE(DATA_Zone zone)
    {
        if(zone.mName == string.Empty)
        {
            Debug.Log("Can't save un-named zone");
            return false;
        }

        string path = Application.dataPath+"/Plays/Zones/"+zone.mName+".bin";

        BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));
        bw.Write(zone.mName);
        bw.Write(zone.mSpot.x);
        bw.Write(zone.mSpot.y);
        
        bw.Close();

        return true;
    }

    public static bool FCHECK_ZONE_EXISTS(string sName)
    {
        foreach (DATA_Zone zone in mZones)
        {
            if(zone.mName == sName)
            {
                return true;
            }
        }

        return false;
    }

    public static void FLOAD_ZONES()
    {
        string path = Application.dataPath+"/Plays/Zones/";

        string[] fPathNames = Directory.GetFiles(path, "*.bin");

        mZones = new DATA_Zone[fPathNames.Length];
        for(int i=0; i<fPathNames.Length; i++)
        {
            BinaryReader br = new BinaryReader(new FileStream(fPathNames[i], FileMode.Open));
            mZones[i] = new DATA_Zone();
            mZones[i].mName = br.ReadString();
            mZones[i].mSpot.x = br.ReadSingle();
            mZones[i].mSpot.y = br.ReadSingle();

            br.Close();
        }
    }

    // assumes that the routes have all been loaded already
    public static DATA_Zone FLOAD_ZONE_BY_NAME(string sName)
    {
        DATA_Zone zone = new DATA_Zone();
        
        for(int i=0; i<mZones.Length; i++)
        {
            if(mZones[i].mName == sName)
            {
                zone = mZones[i];
                return zone;
            }
        }
        
        Debug.Log("Zone not found");
        return null;
    }

    // Here we take our already loaded binary files, and convert them to text files.
    public static void FCONVERT_TO_TEXT_FILES()
    {
        // need the num of routes? Sure.
        StreamWriter sw = new StreamWriter(Application.dataPath+"/ZonesText/zones.txt");
        sw.WriteLine(mZones.Length);
        for(int i=0; i<mZones.Length; i++)
        {
            sw.WriteLine(mZones[i].mName);
            sw.WriteLine("("+(int)mZones[i].mSpot.x+","+(int)mZones[i].mSpot.y+")");
        }
        sw.Close();
    }
}
