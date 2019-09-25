/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class PRAC_Off_ShowGFX : MonoBehaviour
{

    public GFX_PreSnap              GFX_PassBlock;
    public GFX_PreSnap              GFX_RouteTrail;
    
    public void FShowOffensivePlay(DATA_OffPlay offPlay, PLY_SnapSpot snapSpot)
    {
        for(int i=0; i<offPlay.mRoles.Length; i++){
            // figure out where they start first.
            Vector2 vStart = new Vector2();
            DATA_Formation f = IO_Formations.FLOAD_FORMATION(offPlay.mFormation);
            for(int j=0; j<f.mSpots.Length; j++){
                if(f.mTags[j] == offPlay.mTags[i]){
                    vStart = f.mSpots[j];
                }
            }
            if(offPlay.mRoles[i] == "BLOCK"){
                RenderPassBlock(vStart, snapSpot);
            }
            // We have to first get the route data.
            else if(offPlay.mRoles[i] == "ROUTE"){
                for(int j=0; j<offPlay.mRoutes.Count; j++){
                    if(offPlay.mRoutes[j].mOwner == offPlay.mTags[i]){
                        // now we render the route piece by piece.
                        RenderRoute(offPlay.mRoutes[j].mSpots, vStart, snapSpot);
                    }
                }
            }
        }
    }

    private void RenderRoute(List<Vector2> nodes, Vector3 vStart, PLY_SnapSpot snapSpot)
    {
        // This should be the actual starting spot of the player.
        Vector3 vPos = vStart; vPos.z = vPos.y; vPos.y = 1f;
        vPos += snapSpot.transform.position;
        Instantiate(GFX_RouteTrail, vPos, transform.rotation);

        for(int i=1; i<nodes.Count; i++)
        {
            // render a bunch of spots, along the path from a -> b. We're actually going backwards, but it doesn't matter.
            Vector3 vStartPos = vPos + (UT_VecConversion.ConvertVec2(nodes[i]) * 2f);
            Vector3 vFinalPos = vPos + (UT_VecConversion.ConvertVec2(nodes[i-1]) * 2f);
            
            Vector3 vIterPos = vStartPos;
            Vector3 vDir = Vector3.Normalize(vFinalPos - vStartPos);
            while(Vector3.Dot(vDir, vFinalPos - vIterPos) > 0f)
            {
                Instantiate(GFX_RouteTrail, vIterPos, transform.rotation);
                vIterPos += vDir * 1f;
            }
        }
    }
    private void RenderPassBlock(Vector2 vStart, PLY_SnapSpot snapSpot)
    {
        // Make it just behind the player.
        Vector3 vPos = vStart;
        vPos.z = vPos.y - 1f;
        vPos.y = 1f;
        vPos += snapSpot.transform.position;
        Instantiate(GFX_PassBlock, vPos, transform.rotation);
    }

    public void FStopShowingPlayArt()
    {
        GFX_PreSnap[] gfx = FindObjectsOfType<GFX_PreSnap>();
        foreach(GFX_PreSnap gf in gfx)
        {
            Destroy(gf.gameObject);
        }
    }
}
