/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PE_DisplayPlayJobs : MonoBehaviour
{
    public GameObject                   PF_RouteSpot;
    public PE_Route                     PF_Route;
    public PE_BlockImg                  PF_BlockImage;

    public void FDisplayJobs()
    {
        // Destroy existing jobs.
        PE_Route[] oldRoutes = FindObjectsOfType<PE_Route>();
        for(int i=0; i<oldRoutes.Length; i++)
        {
            oldRoutes[i].FDestroySelf();
        }

        PE_BlockImg[] blockImages = FindObjectsOfType<PE_BlockImg>();
        for(int i=0; i<blockImages.Length; i++)
        {
            Destroy(blockImages[i].gameObject);
        }

        // For all the receivers in the scene, spawn a route object.
        // actually, spawn THEIR route, on them.
        PE_Role[] roles = FindObjectsOfType<PE_Role>();
        foreach (PE_Role role in roles)
        {
            if(role.mRole == "Route")
            {
                DATA_Route route = IO_RouteList.FLOAD_ROUTE_BY_NAME(role.mDetails);
                if(route == null)
                {
                    Debug.Log("Woah, incorrect route: " + role.mDetails);
                    return;
                }
                else
                {
                    PE_Route rt = Instantiate(PF_Route);
                    rt.mTag = role.mTag;
                    foreach (Vector2 spot in route.mSpots)
                    {
                        Vector2 vSpot = spot;
                        vSpot /= 10f;           // manual conversion, hacky I know.
                        vSpot += (Vector2)role.transform.position;
                        // on the player who they belong to.
                        var node = Instantiate(PF_RouteSpot, vSpot, role.transform.rotation);
                        rt.mNodes.Add(node);
                        rt.mLineRenderer.positionCount = rt.mNodes.Count;
                        Vector3 lineSpot = node.transform.position; lineSpot.z = 0f;
                        rt.mLineRenderer.SetPosition(rt.mNodes.Count-1, lineSpot);
                    }
                }
            }
            else if (role.mRole == "Pass Block" || role.mRole == "Run Block")
            {
                Vector2 vBlockSpot = role.transform.position;
                vBlockSpot.y -= 0.1f;           // manual fudge factor.
                Instantiate(PF_BlockImage, vBlockSpot, transform.rotation);
            }
        } 
    }
}
