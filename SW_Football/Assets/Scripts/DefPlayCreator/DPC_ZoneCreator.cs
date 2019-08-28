/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DPC_ZoneCreator : MonoBehaviour
{
    // has reference to point representing the snap spot.
    public GameObject                   rSnapSpot;

    // has reference to field, since we need to know the units conversion.
    public PE_Field                     rField;

    public Text                         rZoneName;
    public Text                         rZonePos;

    // When you press down, we spawn a tagged gameobject, for the node spot.
    public DPC_ZoneSpot                 PF_ZoneSpot; 

    public string                       mZoneToSpawn = "Curl Right";
    
    // The play editor calls us.
    public void FRun_Update()
    {
        // // Basically, every time that we click, spawn a point, and destroy any exising points.
        // if(Input.GetMouseButtonDown(0)){
        //     // first, we raycast to make sure we're over the field. Because we can't spawn a player randomly off the field.
        //     RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
        //     if(hit.collider != null)
        //     {
        //         if(hit.collider.GetComponent<PE_Field>() != null){
        //             Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //             pos.z = 90;
        //             SpawnPointDestroyOld(pos);
        //         }
        //     }
        // }

        // // Also, do the conversion and shove the zone details into the zone.
        // DPC_ZoneSpot spot = FindObjectOfType<DPC_ZoneSpot>();
        // if(spot != null)
        // {
        //     // now "shove" the detail about it into itself.
        //     // do a conversion to find the pixels -> yards.
        //     Vector2 vYards = spot.transform.position - rSnapSpot.transform.position;
        //     vYards /= 50f/rField.GetComponent<RectTransform>().rect.width;
        //     vYards.x = (float)System.Math.Round(vYards.x, 0);
        //     vYards.y = (float)System.Math.Round(vYards.y, 0);
        //     spot.mZone.mSpot = vYards;
        //     rZonePos.text = "("+vYards.x+", "+vYards.y+")";

        //     // Now find out what the name of the zone is.
        //     spot.mZone.mName = rZoneName.text;
        // }
        if(Input.GetMouseButtonDown(1))
        {

            // // Now we just load in all the zones, then display them.
            DATA_Zone zone = IO_ZoneList.FLOAD_ZONE_BY_NAME(mZoneToSpawn);
            Vector2 vZoneSpot = zone.mSpot;
            vZoneSpot /= 10f;
            vZoneSpot += (Vector2)rSnapSpot.transform.position;
            var clone = Instantiate(PF_ZoneSpot, vZoneSpot, transform.rotation);
            clone.mZone = zone;
        }

    }

    // We aren't doing any conversion or anything like that.
    private void SpawnPointDestroyOld(Vector2 pos)
    {
        DPC_ZoneSpot[] spots = FindObjectsOfType<DPC_ZoneSpot>();
        foreach (DPC_ZoneSpot spot in spots)
        {
            Destroy(spot.gameObject);
        }

        Instantiate(PF_ZoneSpot, pos, transform.rotation);
    }

    public void BT_SaveZone()
    {
        DPC_ZoneSpot zone = FindObjectOfType<DPC_ZoneSpot>();
        IO_ZoneList.FWRITE_ZONE(zone.mZone);
    }

}
