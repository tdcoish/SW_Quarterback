/*************************************************************************************
Just a list of all the valid tags.
*************************************************************************************/
using System.Collections.Generic;

public static class UT_ValidTags
{
    public static List<string>              mTags = new List<string>();

    public static void FLoadList()
    {
        mTags.Add("WR1");
        mTags.Add("WR1");
        mTags.Add("WR2");
        mTags.Add("WR3");
        mTags.Add("WR4");
        mTags.Add("WR5");
        mTags.Add("TE1");
        mTags.Add("TE2");
        mTags.Add("TE3");
        mTags.Add("RB1");
        mTags.Add("RB2");
        mTags.Add("RB3");
        mTags.Add("QB");
        mTags.Add("OL1");
        mTags.Add("OL2");
        mTags.Add("OL3");
        mTags.Add("OL4");
        mTags.Add("OL5");
        mTags.Add("DL1");
        mTags.Add("DL2");
        mTags.Add("DL3");
        mTags.Add("DL4");
        mTags.Add("LB1");
        mTags.Add("LB2");
        mTags.Add("LB3");
        mTags.Add("CB1");
        mTags.Add("CB2");
        mTags.Add("CB3");
        mTags.Add("CB4");
        mTags.Add("CB5");
        mTags.Add("FS");
        mTags.Add("SS");
        mTags.Add("WS");
    }

    public static bool FIsTagValid(string tag)
    {
        if(mTags.Contains(tag)){
            return true;
        }
        return false;
    }

}
