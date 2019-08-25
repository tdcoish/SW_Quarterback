/*************************************************************************************
Making this before I make binary files representing routes.

Don't save starting position.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName="DT+Route", menuName="DT/Route")]
public class DT_Route : ScriptableObject
{
    public string                   mName;
    public List<Vector2>            mSpots;
}
