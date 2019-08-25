/*************************************************************************************
This is an editor route. Just a representation of a route, not the route itself.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class PE_Route : MonoBehaviour
{
    public List<GameObject>             mNodes;
    public LineRenderer                 mLineRenderer;

    // Pretty weird how you have to do this on Awake, not Start.
    void Awake()
    {
        mLineRenderer = GetComponent<LineRenderer>();
    }
}
