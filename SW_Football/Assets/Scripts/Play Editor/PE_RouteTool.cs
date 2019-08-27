/*************************************************************************************
So the user can make new routes.

God I need to make some state machines.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PE_RouteTool : MonoBehaviour
{
    private PE_Selector             cSelector;

    [SerializeField]
    private GameObject              PF_RouteNode;           // this is just an image. We add the GO to mCurRoute after spawning.

    public bool                     mRouteToolOpened = false;

    public PE_Route                 PF_RouteObj;
    public PE_Route                 mCurRoute;          // our reference to the route. Confusing

    public InputField               rRouteName;

    public GameObject               rRouteUI;
    public GameObject               rOverwriteRouteUI;

    void Start()
    {
        cSelector = GetComponent<PE_Selector>();
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0)){
            if(mRouteToolOpened){

                // first, we raycast to make sure we're over the field. Because we can't spawn a player randomly off the field.
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                
                if(hit.collider != null)
                {
                    if(hit.collider.GetComponent<PE_Field>() != null){
                        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        pos.z = 90;
                        SpawnPoint(pos);
                    }
                }
            }
        }
    }

    // when they press the New Route button, we start into the adding route nodes.
    public void BT_NewRoute()
    {
        // we also spawn a new route at the feet of whichever player is currently active.
        int actInd = cSelector.FGetActivePlayerIndex();
        if(actInd == -1){
            Debug.Log("Problem, no player active");
            return;
        }

        PE_Route[] oldRoutes = FindObjectsOfType<PE_Route>();
        for(int i=0; i<oldRoutes.Length; i++)
        {
            if(oldRoutes[i].mTag == cSelector.rGuys[actInd].GetComponent<PE_Role>().mTag)
            {
                oldRoutes[i].FDestroySelf();
                break;
            }
        }

        rRouteUI.SetActive(true);
        mRouteToolOpened = true;

        mCurRoute = Instantiate(PF_RouteObj, cSelector.rGuys[actInd].transform.position, transform.rotation);
        mCurRoute.mNodes = new List<GameObject>();
        
        SpawnPoint(cSelector.rGuys[actInd].transform.position);
    }

    public void BT_Cancel()
    {
        // get rid of the current route and clean up.
        mCurRoute.FDestroySelf();

        CloseRoute();
    }

    private void CloseRoute()
    {
        mRouteToolOpened = false;
        rRouteUI.SetActive(false);
    }

    // save the current route to the disk.
    public void BT_RouteNamed()
    {
        // If the route already exists, then we ask them if they want to overwrite.
        if(IO_RouteList.FCHECK_ROUTE_EXISTS(rRouteName.text))
        {
            rOverwriteRouteUI.SetActive(true);
        }
        else
        {
            SaveRoute();
        }
    }

    public void BT_OverwriteRoute()
    {
        rOverwriteRouteUI.SetActive(false);
        SaveRoute();
    }
    public void BT_CancelOverwrite()
    {
        rOverwriteRouteUI.SetActive(false);
    }

    private void SaveRoute()
    {
        DATA_Route route = new DATA_Route();
        route.mName = rRouteName.text;
        route.mSpots = new Vector2[mCurRoute.mNodes.Count];
        for(int i=0; i<mCurRoute.mNodes.Count; i++)
        {
            Vector2 vConvertedSpot = new Vector2();
            vConvertedSpot = mCurRoute.mNodes[i].transform.position;
            vConvertedSpot -= (Vector2)mCurRoute.mNodes[0].transform.position;
            vConvertedSpot *= 10f;          // hardcoding because 500 pixel field == 50 meters. - HACK

            vConvertedSpot.x = (float)System.Math.Round(vConvertedSpot.x, 0);
            vConvertedSpot.y = (float)System.Math.Round(vConvertedSpot.y, 0);

            route.mSpots[i] = vConvertedSpot;
        }

        IO_RouteList.FWRITE_ROUTE(route);

        rRouteName.gameObject.SetActive(false);
        rRouteUI.SetActive(false);

        CloseRoute();
    }

    // Just opens up the name route menu.
    public void BT_Save()
    {
        rRouteName.gameObject.SetActive(true);
    }

    // Gotta make the point "snap" to yards, so instead of (0, 10.33493) -> (0, 10).
    private void SpawnPoint(Vector3 pos){
        var clone = Instantiate(PF_RouteNode, pos, transform.rotation);
        mCurRoute.mNodes.Add(clone);

        // ugly hack, but whatever.
        mCurRoute.mLineRenderer.positionCount = mCurRoute.mNodes.Count;
        Vector3 lineSpot = clone.transform.position; lineSpot.z = 0;
        mCurRoute.mLineRenderer.SetPosition(mCurRoute.mNodes.Count-1, lineSpot);
    }
}
