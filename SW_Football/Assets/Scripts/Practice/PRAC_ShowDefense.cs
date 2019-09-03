/*************************************************************************************
When you hold down the button, we show the defensive play art.

Doing this one first because it's easier to just spawn zones.
*************************************************************************************/
using UnityEngine;

public class PRAC_ShowDefense : MonoBehaviour
{

    public GFX_Zone                 GFX_ShallowZone;
    public GFX_Zone                 GFX_MidZone;
    public GFX_Zone                 GFX_DeepZone;
    public GFX_PreSnap              GFX_ManCover;
    public GFX_PreSnap              GFX_Rush;

    public GFX_PreSnap              GFX_PassBlock;
    public GFX_PreSnap              GFX_RouteTrail;

    public void FShowAllPlayRoles(DATA_Play offPlay, DATA_Play defPlay, PLY_SnapSpot snapSpot)
    {
        FShowDefensivePlay(defPlay, snapSpot);
    }

    private void FShowDefensivePlay(DATA_Play defPlay, PLY_SnapSpot snapSpot)
    {
        for(int i=0; i<defPlay.mPlayerRoles.Length; i++)
        {
            if(defPlay.mPlayerRoles[i].mRole == "Zone")
            {
                // get the zone details.
                DATA_Zone zn = IO_ZoneList.FLOAD_ZONE_BY_NAME(defPlay.mPlayerRoles[i].mDetail);
                GFX_Zone zoneGFX;
                if(zn.mSpot.y > 19f)
                {
                    // render using the "deep zone" version.
                    zoneGFX = GFX_DeepZone;
                }else if(zn.mSpot.y > 9f)
                {
                    // render using mid zone version
                    zoneGFX = GFX_MidZone;
                }else{
                    // render using shallow zone version.
                    zoneGFX = GFX_ShallowZone;
                }

                Vector3 pos = snapSpot.transform.position;
                pos.z += zn.mSpot.y;
                pos.x += zn.mSpot.x;
                Instantiate(zoneGFX, pos, transform.rotation);
            }
        }
    }

    private void FShowOffensivePlay(DATA_Play offPlay, PLY_SnapSpot snapSpot)
    {
        for(int i=0; i<offPlay.mPlayerRoles.Length; i++)
        {
            switch(offPlay.mPlayerRoles[i].mRole)
            {
                case "Pass Block" : RenderPassBlock(offPlay.mPlayerRoles[i], snapSpot); break;
                case "Route" : RenderRoute(offPlay.mPlayerRoles[i], snapSpot); break;
            }
        }
    }

    public void FStopShowingPlayArt()
    {
        FStopShowingDefensivePlay();
    }

    // Just cleanup all the spawned role graphics.
    private void FStopShowingDefensivePlay()
    {
        GFX_Zone[] zoneGraphics = FindObjectsOfType<GFX_Zone>();
        foreach(GFX_Zone zn in zoneGraphics)
        {
            Destroy(zn.gameObject);
        }
    }

    private void RenderZone(DT_PlayerRole role, PLY_SnapSpot snapSpot)
    {
        // get the zone details.
        DATA_Zone zn = IO_ZoneList.FLOAD_ZONE_BY_NAME(role.mDetail);
        GFX_Zone zoneGFX;
        if(zn.mSpot.y > 19f)
        {
            // render using the "deep zone" version.
            zoneGFX = GFX_DeepZone;
        }else if(zn.mSpot.y > 9f)
        {
            // render using mid zone version
            zoneGFX = GFX_MidZone;
        }else{
            // render using shallow zone version.
            zoneGFX = GFX_ShallowZone;
        }

        Vector3 pos = snapSpot.transform.position;
        pos.z += zn.mSpot.y;
        pos.x += zn.mSpot.x;
        Instantiate(zoneGFX, pos, transform.rotation);
    }

    private void RenderRush(DT_PlayerRole role, PLY_SnapSpot snapSpot)
    {
        // Make it just behind the player.
        Vector3 vPos = UT_VecConversion.ConvertVec2(role.mStart);
        vPos.z -= 1f;
        vPos.y = 1f;
        vPos += snapSpot.transform.position;
        Instantiate(GFX_Rush, vPos, transform.rotation);
    }

    private void RenderPassBlock(DT_PlayerRole role, PLY_SnapSpot snapSpot)
    {
        // Make it just behind the player.
        Vector3 vPos = role.mStart;
        vPos.z = vPos.y - 1f;
        vPos.y = 1f;
        vPos += snapSpot.transform.position;
        Instantiate(GFX_PassBlock, vPos, transform.rotation);
    }

    // I've settled on the first prototype being a trail of evenly spaced VERY small dots.
    // Let's start by just spawning on the route points.
    private void RenderRoute(DT_PlayerRole role, PLY_SnapSpot snapSpot)
    {
        Vector3 vPos = role.mStart;
        vPos.z = vPos.y - 1f;
        vPos.y = 1f;
        vPos += snapSpot.transform.position;

        DATA_Route rt = IO_RouteList.FLOAD_ROUTE_BY_NAME(role.mDetail);
        for(int i=0; i<rt.mSpots.Length; i++)
        {
            vPos += UT_VecConversion.ConvertVec2(rt.mSpots[i]);
            Instantiate(GFX_RouteTrail, vPos, transform.rotation);
        }
    }

    // Just spawn a man coverage "line" in front of the player for now.
    // Eventually we spawn one in between the player and the player they are covering.
    private void RenderManCover(DT_PlayerRole role, PLY_SnapSpot snapSpot)
    {
        Vector3 vPos = role.mStart;
        vPos.z = vPos.y - 1f;
        vPos.y = 1f;
        vPos += snapSpot.transform.position;
        Instantiate(GFX_ManCover, vPos, transform.rotation);
    }

}
