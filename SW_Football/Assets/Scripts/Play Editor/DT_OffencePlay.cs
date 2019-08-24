/*************************************************************************************
Gonna be some similarities between offense and defence, that's not that important.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

// Yeah you need system.serializable
[System.Serializable]
public class DT_PlayerRole
{
    public Vector2          mStart;
    public string           mTag;
    public string           mRole;
}

[CreateAssetMenu(fileName="SO_OffencePlay", menuName="PLAYS/SO_OffencePlay")]
public class DT_OffencePlay : ScriptableObject
{
    public string                       mName;
    public List<DT_PlayerRole>          mPlayerRoles;
}
