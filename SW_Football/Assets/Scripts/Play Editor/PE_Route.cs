/*************************************************************************************
This is an editor route. Just a representation of a route, not the route itself.

Probably have to save the tag, since then it's easier to go in and destroy this specific 
route when editing a players role.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class PE_Route : MonoBehaviour
{
    [Tooltip("Player That owns me")]
    public string                       mTag;
    public List<GameObject>             mNodes;
    public LineRenderer                 mLineRenderer;

    // Pretty weird how you have to do this on Awake, not Start.
    void Awake()
    {
        mLineRenderer = GetComponent<LineRenderer>();
    }

    public void FDestroySelf()
    {
        for(int i=0; i<mNodes.Count; i++)
        {
            Destroy(mNodes[i].gameObject);
        }
        Destroy(gameObject);
    }
}
