/*************************************************************************************
All the data for the players achievements. Passes thrown, minigame success, etcetera.
*************************************************************************************/
using UnityEngine;

// Each minigame gets this.
public class MinigameAchievement{
    public int                  mEasyScore;
    public int                  mNormalScore;
    public int                  mHardScore;
    public int                  mPetermanScore;
}

public class DATA_Gamer
{
    public MinigameAchievement      mRPHighScores;
    public MinigameAchievement      mPPHighScores;

    public int                  mPassesThrown = 0;
}
