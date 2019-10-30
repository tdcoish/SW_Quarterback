/*************************************************************************************
Renders the set itself.
We don't need to render the players or hoops, they exist in the editor.
Mainly just the routes itself.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class ED_RP_GFXset : MonoBehaviour
{
    public ED_RP_GFX_Rt                     GFX_RtNd;
    public ED_OP_GFX_RT_Trail               GFX_RT_Trail;

    public void FRenderSet(List<DATA_ORoute> rRoutes, List<ED_RP_Rec> rRecs, int grdToYards, ED_RP_Grd grd)
    {
        // ------------ delete the old graphics from the scene.
        ED_RP_GFX_Rt[] rtGfx = FindObjectsOfType<ED_RP_GFX_Rt>();
        foreach(ED_RP_GFX_Rt g in rtGfx){
            Destroy(g.gameObject);
        }

        // ------------ render the routes 
        for(int i=0; i<rRoutes.Count; i++)
        {
            for(int j=0; j<rRecs.Count; j++){
                if(rRecs[j].mTag == rRoutes[i].mOwner){
                    // now render the route from his perspective.
                    TDC_IntVec iVecConv = new TDC_IntVec(rRecs[j].mIxX, rRecs[j].mIxY);

                    for(int k=1; k<rRoutes[i].mSpots.Count; k++){
                        Vector2 vSpot = rRoutes[i].mSpots[k];
                        TDC_IntVec v = new TDC_IntVec((int)vSpot.x + iVecConv.x, (int)vSpot.y + iVecConv.y);
                        vSpot = rRoutes[i].mSpots[k-1];
                        TDC_IntVec v1 = new TDC_IntVec((int)vSpot.x + iVecConv.x, (int)vSpot.y + iVecConv.y);
                        ED_RP_GFX_Rt g = Instantiate(GFX_RtNd, grd.mSquares[v.x, v.y].transform.position, transform.rotation);
                        g.transform.SetParent(grd.transform);

                        RenderTrail(grd.mSquares[v.x, v.y].transform.position, grd.mSquares[v1.x, v1.y].transform.position, grd);
                    }
                }
            }
        }
    }

    // just put one right in the middle for now.
    private void RenderTrail(Vector2 vStart, Vector2 vEnd, ED_RP_Grd grd)
    {
        Vector2 vPos = (vEnd - vStart) / 2f;
        vPos += vStart;

        ED_OP_GFX_RT_Trail t = Instantiate(GFX_RT_Trail, vPos, transform.rotation);
        t.transform.SetParent(grd.transform);
    }
}
