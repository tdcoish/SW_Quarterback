/*************************************************************************************
Sort of like the practice play art, except just for the routes.
*************************************************************************************/
using UnityEngine;

public class RP_DrawRoutes : MonoBehaviour
{
    public GFX_PreSnap              GFX_RouteTrail;

    public void FShowRouteGraphics()
    {
        RP_Receiver[] recs = FindObjectsOfType<RP_Receiver>();
        foreach(RP_Receiver r in recs)
        {
            RenderRoute(r.transform.position, r.mRoute);
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
    private void RenderRoute(Vector3 vRecPos, string sRoute)
    {
        Vector3 vPos = vRecPos;
        vPos.y = 1f;

        DATA_Route rt = IO_RouteList.FLOAD_ROUTE_BY_NAME(sRoute);
        for(int i=0; i+1<rt.mSpots.Length; i++)
        {
            // render a bunch of spots, along the path from a -> b.
            Vector3 vStartPos = vPos + UT_VecConversion.ConvertVec2(rt.mSpots[i]);
            Vector3 vFinalPos = vPos + UT_VecConversion.ConvertVec2(rt.mSpots[i+1]);
            
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
