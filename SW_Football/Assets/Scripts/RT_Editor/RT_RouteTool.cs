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

    public List<Vector3>            mCurRoute;

    void Start()
    {
        cEdMan = GetComponentInParent<RT_SceneManager>();
    }

    void Update()
    {
        // need some way of knowing where the "field" is, so to speak.
        // let's give the field a trigger volume.

        if(Input.GetMouseButtonDown(0)){
            if(mRouteToolOpened){

                // first, we raycast to make sure we're over the field. Because we can't spawn a player randomly off the field.
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                
                if(hit.collider != null)
                {
                    if(hit.collider.GetComponent<RT_Field>() != null){
                        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        pos.z = 90;
                        Instantiate(PF_RouteNode, pos, transform.rotation);
                        mCurRoute.Add(pos);
                    }
                }
            }
        }
    }

    // when they press the New Route button, we start into the adding route nodes.
    public void BT_NewRoute()
    {
        mCurRoute = new List<Vector3>();
        mRouteToolOpened = true;

        // we also spawn a new route at the feet of whichever player is currently active.
        int actInd = cEdMan.GetActivePlayerIndex();
        if(actInd != -1){
            Instantiate(PF_RouteNode, cEdMan.rPlayers[actInd].transform.position, transform.rotation);
            mCurRoute.Add(cEdMan.rPlayers[actInd].transform.position);
        }else{
            Debug.Log("Problem, no player active");
        }
    }

    public void BT_DoneRoute()
    {
        mRouteToolOpened = false;
    }

    public void SpawnPoint(Vector3 pos){
        var clone = Instantiate(PF_RouteNode, pos, transform.rotation);

        mCurRoute.Add(pos);
    }
}
