/*************************************************************************************
Should just be three numbers, bronze, silver, gold.
*************************************************************************************/
using UnityEngine;

[CreateAssetMenu(fileName="DT+TrophyVals", menuName="DT/TrophyVals")]
public class DT_TrophyValues : ScriptableObject
{
    public float                mBronze = 400f;
    public float                mSilver = 1200f;
    public float                mGold = 1800f;
    public float                mPlatinum = 2200f;
}
