/*************************************************************************************
Offence plays only for now.

Plays have starting positions, tags, and roles. Maybe I should include some metadata about
how many players there are. I've been just assuming 11.
*************************************************************************************/
using UnityEngine;

[System.Serializable]
public class DT_PlayerRole
{
    public string               mTag;
    public string               mRole;
    public string               mDetail;
    public Vector2              mStart;
}

public class DATA_Play
{
    public string                   mName;
    public DT_PlayerRole[]          mPlayerRoles;
}
