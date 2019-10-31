/*************************************************************************************
Sort of like the practice play art, except just for the routes.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class RP_DrawRoutes : MonoBehaviour
{
    public GFX_PreSnap              GFX_RouteTrail;

    public void FShowRouteGraphics()
    {
        RP_Receiver[] recs = FindObjectsOfType<RP_Receiver>();
        foreach(RP_Receiver r in recs)
        {
            // TODO: FIx this.
            RenderRoute(r.GetComponent<OFF_RouteLog>().mRouteSpots);
        }
    }

    public void FStopShowingRoutes()
    {
        GFX_PreSnap[] gfx = FindObjectsOfType<GFX_PreSnap>();
        foreach(GFX_PreSnap g in gfx)
        {
            Destroy(g.gameObject);
        }
    }

    // I've settled on the first prototype being a trail of evenly spaced VERY small dots.
    private void RenderRoute(List<Vector3> spots)
    {

        for(int i=1; i<spots.Count; i++)
        {
            // render a bunch of spots, along the path from a -> b.
            Vector3 vStartPos = spots[i];
            Vector3 vFinalPos = spots[i-1];
            
            Vector3 vIterPos = vStartPos;
            Vector3 vDir = Vector3.Normalize(vFinalPos - vStartPos);
            while(Vector3.Dot(vDir, vFinalPos - vIterPos) > 0f)
            {
                Instantiate(GFX_RouteTrail, vIterPos, transform.rotation);
                vIterPos += vDir * 1f;
            }
        }
    }


}
