/*************************************************************************************
Gets called to run, basically just removes all spawned graphics from the screen, then 
spawns new graphics in.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class DPC_PlayDisplayer : MonoBehaviour
{
    public DPC_ZoneGFX              PF_ZoneGFX;

    public GameObject               rSnapSpot;

    public Text                     rTag;
    public Text                     rRole;
    public Text                     rDetail;

    public void FDisplayPlayerDetails()
    {
        // Get a reference to the selector, find out if anyone is selected, if not, set these all to UN
        // if so, set them to what the player is.
        DPC_Selector selector = FindObjectOfType<DPC_Selector>();
        if(selector.mActivePlayer == -1)
        {
            rTag.text = "UN";
            rRole.text = "UN";
            rDetail.text = "UN";
        }
        else
        {
            PE_Role role = selector.rGuys[selector.mActivePlayer].GetComponent<PE_Role>();
            rTag.text = role.mTag;
            rRole.text = role.mRole;
            rDetail.text = role.mDetails;
        }
    }

    // I guess for now we just start with zones.
    public void FDisplayPlay()
    {
        DPC_ZoneGFX[] zGX = FindObjectsOfType<DPC_ZoneGFX>();
        foreach(DPC_ZoneGFX gfx in zGX)
        {
            Destroy(gfx.gameObject);
        }

        // Now, iterate through all players, and spawn in a node representing their zone.
        PE_Role[] athletes = FindObjectsOfType<PE_Role>();
        foreach(PE_Role role in athletes)
        {
            if(role.mRole == "Zone")
            {
                // get the zone details from the IO
                DATA_Zone zone = IO_ZoneList.FLOAD_ZONE_BY_NAME(role.mDetails);
                if(zone != null)
                {
                    Vector2 vPos = rSnapSpot.transform.position;
                    vPos += zone.mSpot/10f;
                    Instantiate(PF_ZoneGFX, vPos, transform.rotation);
                }
            }
        }
    }
}
