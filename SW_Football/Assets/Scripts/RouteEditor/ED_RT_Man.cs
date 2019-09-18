/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ED_RT_Man : MonoBehaviour
{
    public enum STATE{
        S_BEGIN,
        S_PLACING,
        S_END
    }
    public STATE                                    mState;

    public Image                                    PF_Player;
    public Image                                    PF_Trail;
    public InputField                               mNameField;

    public ED_RT_Grid                               rGrid;

    // The spots of the route
    public List<ED_RT_Rt_Node>                      mNodes;

    void Start()
    {
        mNodes = new List<ED_RT_Rt_Node>();
    }

    void Update()
    {

        switch(mState){
            case STATE.S_BEGIN: RUN_BEGIN(); break;
            case STATE.S_PLACING: RUN_PLACING(); break;
        }
    }
    
    // I first place a receiver in a certain spot, and that's it.
    private void RUN_BEGIN()
    {
        PlaceNode(rGrid.mSqrLnth/2, rGrid.mSqrLnth-1);

        mState = STATE.S_PLACING;
    }

    private void RUN_PLACING()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
            
            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<ED_RT_Square>() != null)
                {
                    Debug.Log("Hit grid square");
                    Debug.Log(hit.transform.position);
                    ED_RT_Square s = hit.collider.GetComponent<ED_RT_Square>();
                    PlaceNode(s.x, s.y);
                    RenderRoute();
                }
            }

        }
    }

    private void PlaceNode(int x, int y)
    {
        Vector3 vRecPos = rGrid.FGetPos(x, y);
        var clone = Instantiate(PF_Player, vRecPos, transform.rotation);
        clone.rectTransform.SetParent(rGrid.transform);
        
        ED_RT_Rt_Node n = clone.GetComponent<ED_RT_Rt_Node>();
        n.x = x;
        n.y = y;
        mNodes.Add(n);
    }

    // Walk through the entire route and make the squares in between the active squares light blue.
    private void RenderRoute()
    {
        // Make all the spots in between light up.
        // Just go by indices.
        for(int i=1; i<mNodes.Count; i++)
        {
            float xDis = mNodes[i - 1].GetComponent<Image>().transform.position.x - mNodes[i].GetComponent<Image>().transform.position.x;
            float yDis = mNodes[i - 1].GetComponent<Image>().transform.position.y - mNodes[i].GetComponent<Image>().transform.position.y;
            float fDis = Mathf.Sqrt(xDis*xDis + yDis*yDis);
            Debug.Log("Distance: " + fDis);
            float fStep = 10f;

            Vector2 vStep = new Vector2(xDis, yDis);
            vStep = Vector3.Normalize(vStep);
            vStep *= fStep;

            float xStart = mNodes[i].GetComponent<Image>().transform.position.x;
            float yStart = mNodes[i].GetComponent<Image>().transform.position.y;
            for(int j=0; j<fDis/fStep; j++)
            {
                xStart += vStep.x;
                yStart += vStep.y;
                Vector2 v = new Vector2(xStart, yStart);
                var clone = Instantiate(PF_Trail, v, transform.rotation);
                clone.rectTransform.SetParent(rGrid.transform);
            }
        }
    }

    // Delete all the nodes. Clear the node list.
    // Delete all graphics.
    public void BT_Cancel()
    {
        foreach(ED_RT_Rt_Node n in mNodes){
            Destroy(n.gameObject);
        }
        ED_RT_Trail[] trails = FindObjectsOfType<ED_RT_Trail>();
        foreach(ED_RT_Trail t in trails){
            Destroy(t.gameObject);
        }

        mNodes.Clear();
        mNodes = new List<ED_RT_Rt_Node>();

        mState = STATE.S_BEGIN;
    }

    public void BT_Save()
    {
        Debug.Log("Name: " + mNameField.text);
        if(mNameField.text == ""){
            Debug.Log("No valid name yet");
            return;
        }

        DATA_Route r = new DATA_Route();
        r.mName = mNameField.text;
        r.mSpots = new Vector2[mNodes.Count];
        r.mSpots[0].x = 0f;
        r.mSpots[0].y = 0f;
        // Now I need to convert these guys.
        for(int i=1; i<r.mSpots.Length; i++){
            Vector2 v = new Vector2();
            int xDif = mNodes[i].x - mNodes[i-1].x;
            int yDif = mNodes[i].y - mNodes[i-1].y;

            v.x = xDif * 2f;
            v.y = yDif * -2f;
            Debug.Log(v);

            r.mSpots[i] = v;
        }
        
        IO_RouteList.FWRITE_GRID_ROUTE(r);
    }
}
