/*************************************************************************************
Can convert from minutes to seconds and back.
*************************************************************************************/
using UnityEngine;

// minutes and seconds as seperate variables.
public struct TDC_Time{
    public int                      mMin;
    public int                      mSec;
}

public static class UT_MinSec
{
    public static int FMinToSecs(TDC_Time t)
    {
        int secs = t.mMin * 60;
        secs += t.mSec;

        return secs;
    }

    public static TDC_Time FSecsToMin(int secs)
    {
        TDC_Time t = new TDC_Time();
        t.mMin = secs/60;
        t.mSec = secs%60;

        return t;
    }
}
