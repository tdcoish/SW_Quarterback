/*************************************************************************************
Global static function that figures out who should be covering whom.

We have an array of receivers and their importance, as well as an array of coverage guys, and
their importance. This way we shouldn't get stuck in a situation where we have CB2 covering
TE2, while LB1 and LB2 get WR2 and WR3 respectively. Zone coverage is totally different.

We load in the order from a text file.
*************************************************************************************/
using UnityEngine;
using System.IO;

public static class PRS_AssignMan
{


    public static string[]          mRecOrder;
    public static string[]          mCovOrder;

    // Text files are going to be saved as 
    public static void FLOAD_PRIORITIES()
    {
        // ----------------------------- Offensive first
        string path = Application.dataPath+"/Plays/Priorities/Receivers.txt";
        // first thing saved is number of tags.
        StreamReader sReader = new StreamReader(path);
        string sLine = sReader.ReadLine();
        int k = sLine.IndexOf(':') + 1;
        int eligibleReceivers = int.Parse(sLine.Substring(k));
        mRecOrder = new string[eligibleReceivers];
        for(int i=0; i<eligibleReceivers; i++)
        {
            mRecOrder[i] = sReader.ReadLine();
        }
        sReader.Close();

        // ------------------------------ Defensive Second
        path = Application.dataPath+"/Plays/Priorities/Defenders.txt";
        sReader = new StreamReader(path);
        sLine = sReader.ReadLine();
        k = sLine.IndexOf(':') + 1;
        int elibibleDefenders = int.Parse(sLine.Substring(k));
        mCovOrder = new string[elibibleDefenders];
        for(int i=0; i<elibibleDefenders; i++)
        {
            mCovOrder[i] = sReader.ReadLine();
        }
        sReader.Close();
    }
}
