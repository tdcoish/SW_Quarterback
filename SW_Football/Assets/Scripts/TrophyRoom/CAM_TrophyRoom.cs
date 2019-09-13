/*************************************************************************************
Basically just move to a pre-determined spot when they press certain buttons.
*************************************************************************************/
using UnityEngine;

[System.Serializable]
public class StructPos
{
    public Vector3                  mPos;
    public Vector3                  mRot;
}

public class CAM_TrophyRoom : MonoBehaviour
{
    public StructPos                        mStart;

    public StructPos[]                      mSpots;

    private MAN_TrophyRoom                  rMan;

    void Start()
    {
        rMan = FindObjectOfType<MAN_TrophyRoom>();

    }

}
