/*************************************************************************************
Global static function that figures out who should be covering whom.

We have an array of receivers and their importance, as well as an array of coverage guys, and
their importance. This way we shouldn't get stuck in a situation where we have CB2 covering
TE2, while LB1 and LB2 get WR2 and WR3 respectively. Zone coverage is totally different.

We load in the order from a text file.
*************************************************************************************/
using UnityEngine;
using System.IO;
using System.Collections.Generic;

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

    // If we don't have the tag, that's not an elible receiver. Offense can only have 5, so QB + OL doesn't count.
    public static bool FELIGIBLE_RECEIVER(string sTag)
    {
        for(int i=0; i<mRecOrder.Length; i++)
        {
            if(mRecOrder[i] == sTag){
                return true;
            }
        }

        return false;
    }

    /**************************
    So if we pass in WR1 we should get 0, and if we pass in TE3 we should get 11 or something like that.
    ************************* */
    private static int FGET_OFF_TAG_IMPORTANCE(string sTag)
    {
        for(int i=0; i<mRecOrder.Length; i++)
        {
            if(mRecOrder[i] == sTag)
            {
                return i;
            }
        }

        return -1;
    }

    /**************************
    So if we pass in CB1 we should get 0, and if we pass in EDGE2 we should get 16 or something like that.
    ************************* */
    private static int FGET_DEF_TAG_IMPORTANCE(string sTag)
    {
        for(int i=0; i<mCovOrder.Length; i++)
        {
            if(mCovOrder[i] == sTag)
            {
                return i;
            }
        }

        return -1;
    }

    /************************
    Receive all the elible receivers for a play. Sort them in their order of receiving importance, so that we can just pull them
    off when it comes time for the defenders to man up.
    ********************* */
    public static List<PRAC_Off> FSORT_RECEIVER_IMPORTANCE(List<PRAC_Off> recs)
    {
        for(int i=1; i<recs.Count; i++)
        {
            for(int j=i; j>0; j--)
            {
                if(FGET_OFF_TAG_IMPORTANCE(recs[j].mJob.mTag) < FGET_OFF_TAG_IMPORTANCE(recs[j-1].mJob.mTag))
                {
                    // Swap the pointers.
                    PRAC_Off temp = recs[j];
                    recs[j] = recs[j-1];
                    recs[j-1] = temp;
                }
            }
        }

        return recs;
    }

    /************************
    Receive all the defenders in man coverage. Then we sort them from most valuable to least, so we should never get
    CB1 covering nobody while LB3 covers WR1.
    ********************* */
    public static List<PRAC_Def> FSORT_DEFENDER_IMPORTANCE(List<PRAC_Def> defsInMan)
    {
        
        for(int i=1; i<defsInMan.Count; i++)
        {
            for(int j=i; j>0; j--)
            {
                if(FGET_DEF_TAG_IMPORTANCE(defsInMan[j].mJob.mTag) < FGET_DEF_TAG_IMPORTANCE(defsInMan[j-1].mJob.mTag))
                {
                    // Swap the pointers.
                    PRAC_Def temp = defsInMan[j];
                    defsInMan[j] = defsInMan[j-1];
                    defsInMan[j-1] = temp;
                }
            }
        }

        return defsInMan;
    }
}
