/*************************************************************************************
For the loaded play, display the routes from the receivers.
*************************************************************************************/
using UnityEngine;

public class PE_DisplayPlayRoutes : MonoBehaviour
{

    public GameObject                   PF_RouteSpot;
    public PE_Route                     PF_Route;

    public void FDisplayRoutes()
    {
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
                    Debug.Log("Yeah, found: " + role.mDetails);
                    PE_Route rt = Instantiate(PF_Route);
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
        } 
    }
}
