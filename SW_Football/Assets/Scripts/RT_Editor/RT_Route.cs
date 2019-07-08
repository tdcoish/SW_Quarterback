/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class RT_Route : MonoBehaviour
{

    public string                       mPlayerTag;
    public List<GameObject>             mNodes;
    public LineRenderer                 mLineRenderer;

    // Pretty weird how you have to do this on Awake, not Start.
    void Awake()
    {
        mLineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        
    }
}
