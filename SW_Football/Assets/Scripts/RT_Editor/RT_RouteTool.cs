/*************************************************************************************
Handles creating new routes for the RT_SceneManager
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class RT_RouteTool : MonoBehaviour
{

    private RT_SceneManager         cEdMan;

    [SerializeField]
    private GameObject              PF_RouteNode;

    public bool                     mRouteToolOpened = false;

    public RT_Route                 PF_RouteObj;
    public RT_Route                 mCurRoute;
    public List<RT_Route>           rRoutes;

    // for now I'm giving us the line renderer. Migrate this out later.
    // public LineRenderer             mLineRenderer;

    void Start()
    {
        cEdMan = GetComponentInParent<RT_SceneManager>();
        rRoutes = new List<RT_Route>();
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0)){
            if(mRouteToolOpened){

                // first, we raycast to make sure we're over the field. Because we can't spawn a player randomly off the field.
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                
                if(hit.collider != null)
                {
                    if(hit.collider.GetComponent<RT_Field>() != null){
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
        int actInd = cEdMan.GetActivePlayerIndex();
        if(actInd == -1){
            Debug.Log("Problem, no player active");
            return;
        }

        mRouteToolOpened = true;

        mCurRoute = Instantiate(PF_RouteObj, cEdMan.rPlayers[actInd].transform.position, transform.rotation);
        mCurRoute.mNodes = new List<GameObject>();
        
        mCurRoute.mPlayerTag = cEdMan.rPlayers[actInd].mTag; 
        SpawnPoint(cEdMan.rPlayers[actInd].transform.position);

        rRoutes.Add(mCurRoute);
    }

    public void BT_DoneRoute()
    {
        mRouteToolOpened = false;
    }

    public void SpawnPoint(Vector3 pos){
        var clone = Instantiate(PF_RouteNode, pos, transform.rotation);
        mCurRoute.mNodes.Add(clone);

        // ugly hack, but whatever.
        mCurRoute.mLineRenderer.positionCount = mCurRoute.mNodes.Count;
        Vector3 lineSpot = clone.transform.position; lineSpot.z = 0;
        mCurRoute.mLineRenderer.SetPosition(mCurRoute.mNodes.Count-1, lineSpot);
    }


}
