/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class ED_RP_Rt : MonoBehaviour
{
    public ED_OP_GFX_RT_ND              GFX_Rt_Nd;
    // public ED_OP_GFX_RT_ND              GFX_Rt_Nd_Set;      // This is what's used for the finished route. First is the route placer.

    // This is just for the active route.
    public List<ED_OP_GFX_RT_ND>        rRouteNodes;

    void Start()
    {
        rRouteNodes = new List<ED_OP_GFX_RT_ND>();
    }

    public void FPlaceFirst(int ixX, int ixY, ED_RP_Grd grd)
    {
        rRouteNodes = new List<ED_OP_GFX_RT_ND>();
        // Put a route node down right where the player is.
        Vector3 vRecPos = grd.mSquares[ixX, ixY].transform.position;
        ED_OP_GFX_RT_ND n = Instantiate(GFX_Rt_Nd, vRecPos, transform.rotation);
        n.mIxX = ixX;
        n.mIxY = ixY;
        rRouteNodes.Add(n);
        n.transform.SetParent(grd.transform);
    }

    public void FPlaceNode(int ixX, int ixY, ED_RP_Grd grd)
    {
        Vector3 vRecPos = grd.mSquares[ixX, ixY].transform.position;
        ED_OP_GFX_RT_ND n = Instantiate(GFX_Rt_Nd, vRecPos, transform.rotation);
        n.mIxX = ixX;
        n.mIxY = ixY;
        rRouteNodes.Add(n);
        n.transform.SetParent(grd.transform);
    }

    public void FClear()
    {
        ED_OP_GFX_RT_ND[] ndGfx = FindObjectsOfType<ED_OP_GFX_RT_ND>();
        foreach(ED_OP_GFX_RT_ND g in ndGfx){
            Destroy(g.gameObject);
        }
        rRouteNodes.Clear();
    }

    public List<Vector2> FConvNodesToRoute(TDC_IntVec recPos)
    {
        List<Vector2> spots = new List<Vector2>();
        for(int i=0; i<rRouteNodes.Count; i++)
        {
            Vector2 v = new Vector2();
            v.x = rRouteNodes[i].mIxX; v.y = rRouteNodes[i].mIxY;
            v.x -= recPos.x; v.y -= recPos.y;
            spots.Add(v);
        }

        return spots;
    }

    /***************************************************************************************
    The indices are the grid position of the receiver. This matters of course, because the route
    is saved relative to them.
    grdSqrSize is the conversion factor between grids and yards. I believe that each square is 
    currently 2x2 yards.
    ************************************************************************************** */
    public DATA_ORoute FReturnActiveRoute(string owner, int ixRecX, int ixRecY, int grdSqrSize)
    {
        DATA_ORoute r = new DATA_ORoute();
        r.mOwner = owner;

        // ------------------------------ Fill the positions according to the current list
        r.mSpots = new List<Vector2>();
        r.mSpots.Add(new Vector2(0, 0));
        for(int i=1; i<rRouteNodes.Count; i++){

            Vector2 v = new Vector2(); 
            v.x = rRouteNodes[i].mIxX - ixRecX;
            v.y = rRouteNodes[i].mIxY - ixRecY;
            // v.x *= 2;
            // v.y *= 2;
            Debug.Log("Maybe convert here");
            r.mSpots.Add(v);
        }

        return r;
    }
}
