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
            Debug.Log("Rendering route");
            for(int j=0; j<rRecs.Count; j++){
                if(rRecs[j].mTag == rRoutes[i].mOwner){
                    Debug.Log("Found owner");   
                    // now render the route from his perspective.
                    TDC_IntVec iVecConv = new TDC_IntVec(rRecs[j].mIxX, rRecs[j].mIxY);

                    for(int k=0; k<rRoutes[i].mSpots.Count; k++){
                        Vector2 vSpot = rRoutes[i].mSpots[k];
                        TDC_IntVec v = new TDC_IntVec((int)vSpot.x + iVecConv.x, (int)vSpot.y + iVecConv.y);
                        ED_RP_GFX_Rt g = Instantiate(GFX_RtNd, grd.mSquares[v.x, v.y].transform.position, transform.rotation);
                        g.transform.SetParent(grd.transform);
                    }
                }
            }
        }
    }
}
