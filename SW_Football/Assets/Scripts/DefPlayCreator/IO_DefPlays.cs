/*************************************************************************************
Just like the offensive ones.
*************************************************************************************/
using UnityEngine;
using System.IO;

public class IO_DefPlays : MonoBehaviour
{
    public static DATA_Play[]               mPlays;

    public static bool FWRITE_PLAY(DATA_Play play)
    {
        if(play.mName == string.Empty)
        {
            Debug.Log("Can't save empty play name");
            return false;
        }

        string sPath = Application.dataPath+"/Plays/Defense/";
        
        // first check if that play already exists.
        string[] fPathNames = Directory.GetFiles(sPath, "*.bin");
        sPath += play.mName+".bin";
        foreach(string sName in fPathNames)
        {
            if(sName == sPath)
            {
                Debug.Log("Error. Play name conflict.");
                Debug.Log("Overwriting anyways");
                //return false;
            }
        }


        BinaryWriter bw = new BinaryWriter(new FileStream(sPath, FileMode.Create));
        bw.Write(play.mName);
        // here's where I write the number of players.
        bw.Write(play.mPlayerRoles.Length);
        foreach(DT_PlayerRole pRole in play.mPlayerRoles)
        {
            bw.Write(pRole.mTag);
            bw.Write(pRole.mRole);
            bw.Write(pRole.mDetail);            // this would be things like the specific route, or how to run block.
            bw.Write(pRole.mStart.x);
            bw.Write(pRole.mStart.y);
        }
        Debug.Log("Play Saved: " + play.mName);
        bw.Close();

        return true;
    }    

    // Loads in all plays.
    public static void FLOAD_PLAYS()
    {
        string path = Application.dataPath+"/Plays/Defense/";

        string[] fPathNames = Directory.GetFiles(path, "*.bin");
        foreach(string sName in fPathNames)
        {
            //Debug.Log(sName);
        }

        mPlays = new DATA_Play[fPathNames.Length];
        for(int i=0; i<fPathNames.Length; i++)
        {
            BinaryReader br = new BinaryReader(new FileStream(fPathNames[i], FileMode.Open));
            mPlays[i] = new DATA_Play();
            mPlays[i].mName = br.ReadString();
            // Debug.Log("Play Name: " + mPlays[i].mName);
            mPlays[i].mPlayerRoles = new DT_PlayerRole[br.ReadInt32()];         // read in the number of players
            for(int j=0; j<mPlays[i].mPlayerRoles.Length; j++)
            {
                DT_PlayerRole temp = new DT_PlayerRole();
                temp.mTag = br.ReadString();
                temp.mRole = br.ReadString();
                temp.mDetail = br.ReadString();
                temp.mStart.x = br.ReadSingle();
                temp.mStart.y = br.ReadSingle();

                mPlays[i].mPlayerRoles[j] = temp;
            }
        }
    }

    public static DATA_Play FLOAD_PLAY_BY_NAME(string sName)
    {
        DATA_Play play = new DATA_Play();
        
        for(int i=0; i<mPlays.Length; i++)
        {
            if(mPlays[i].mName == sName)
            {
                play = mPlays[i];
                Debug.Log("Play Name: " + play.mName);
                return play;
            }

            Debug.Log("Plays loaded: " + mPlays[i].mName);
        }
        
        Debug.Log("Play not found: " + sName);
        return null;
    }
}
