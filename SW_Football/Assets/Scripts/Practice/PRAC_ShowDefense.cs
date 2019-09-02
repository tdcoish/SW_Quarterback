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

    public void FShowDefensivePlay(DATA_Play defPlay, PLY_SnapSpot snapSpot)
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

    // Just cleanup all the spawned role graphics.
    public void FStopShowingDefensivePlay()
    {
        GFX_Zone[] zoneGraphics = FindObjectsOfType<GFX_Zone>();
        foreach(GFX_Zone zn in zoneGraphics)
        {
            Destroy(zn.gameObject);
        }
    }

}
