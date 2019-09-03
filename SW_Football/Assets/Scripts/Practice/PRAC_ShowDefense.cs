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
    public GFX_PreSnap              GFX_ZoneTrail;          // to show which player has which zone.
    public GFX_PreSnap              GFX_ManCover;
    public GFX_PreSnap              GFX_Rush;

    public GFX_PreSnap              GFX_PassBlock;
    public GFX_PreSnap              GFX_RouteTrail;

    public void FShowAllPlayRoles(DATA_Play offPlay, DATA_Play defPlay, PLY_SnapSpot snapSpot)
    {
        FShowOffensivePlay(offPlay, snapSpot);
        FShowDefensivePlay(defPlay, snapSpot);
    }

    private void FShowOffensivePlay(DATA_Play offPlay, PLY_SnapSpot snapSpot)
    {
        for(int i=0; i<offPlay.mPlayerRoles.Length; i++)
        {
            RenderRole(offPlay.mPlayerRoles[i], snapSpot);
        }
    }

    private void FShowDefensivePlay(DATA_Play defPlay, PLY_SnapSpot snapSpot)
    {
        for(int i=0; i<defPlay.mPlayerRoles.Length; i++)
        {
            RenderRole(defPlay.mPlayerRoles[i], snapSpot);
        }
    }

    private void RenderRole(DT_PlayerRole role, PLY_SnapSpot snapSpot)
    {
        switch(role.mRole)
        {
            case "Zone": RenderZone(role, snapSpot); break;
            case "Man": RenderManCover(role, snapSpot); break;
            case "Pass Rush": RenderRush(role, snapSpot); break;
            case "Pass Block": RenderPassBlock(role, snapSpot); break;
            case "Route": RenderRoute(role, snapSpot); break;
        }
    }

    public void FStopShowingPlayArt()
    {
        GFX_PreSnap[] gfx = FindObjectsOfType<GFX_PreSnap>();
        foreach(GFX_PreSnap gf in gfx)
        {
            Destroy(gf.gameObject);
        }

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

        Vector3 vZonePos = snapSpot.transform.position;
        vZonePos.z += zn.mSpot.y;
        vZonePos.x += zn.mSpot.x;
        Instantiate(zoneGFX, vZonePos, transform.rotation);

        Vector3 vStartPos = UT_VecConversion.ConvertVec2(role.mStart);
        vStartPos += snapSpot.transform.position;
        Vector3 vIterPos = vStartPos;
        Vector3 vDir = Vector3.Normalize(vZonePos - vStartPos);
        while(Vector3.Dot(vDir, vZonePos - vIterPos) > 0f)
        {
            Instantiate(GFX_ZoneTrail, vIterPos, transform.rotation);
            vIterPos += vDir * 0.5f;
        }

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
    private void RenderRoute(DT_PlayerRole role, PLY_SnapSpot snapSpot)
    {
        Vector3 vPos = role.mStart;
        vPos.z = vPos.y - 1f;
        vPos.y = 1f;
        vPos += snapSpot.transform.position;

        DATA_Route rt = IO_RouteList.FLOAD_ROUTE_BY_NAME(role.mDetail);
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

    // We're now doing something similar to the zone trail, just spawning little dots to
    // a spot in front of the guy we're covering.
    // Problem is we're no longer where the play says we should be, since we may have man aligned.
    private void RenderManCover(DT_PlayerRole role, PLY_SnapSpot snapSpot)
    {
        Vector3 vPos = new Vector3();
        
        Vector3 vManSpot = new Vector3(); 

        // Need to get references for all the offensive players in the scene, as well as all the defenders.
        PRAC_Off[] offs = FindObjectsOfType<PRAC_Off>();
        PRAC_Def[] defs = FindObjectsOfType<PRAC_Def>();
        
        for(int i=0; i<defs.Length; i++)
        {
            if(defs[i].mJob.mTag == role.mTag)
            {
                vPos = defs[i].transform.position;
                Instantiate(GFX_ZoneTrail, vPos, transform.rotation);
                for(int j=0; j<offs.Length; j++)
                {
                    if(offs[j].mJob.mTag == defs[i].GetComponent<DEF_ManLog>().rMan.mJob.mTag)
                    {
                        vManSpot = offs[j].transform.position;
                    }
                }
            }
        }

        vManSpot.z += 2f;

        // So we have our starting and ending position, now make a trail between them.
        Vector3 vDir = Vector3.Normalize(vManSpot - vPos);
        Vector3 vIterPos = vPos;
        while(Vector3.Dot(vDir, vManSpot - vIterPos) > 0f)
        {
            Instantiate(GFX_ZoneTrail, vIterPos, transform.rotation);
            vIterPos += vDir * 1f;
        }

    }

}
