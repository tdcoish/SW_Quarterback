/*************************************************************************************
Load in the default formation, theoretically I need to shove some roles in there, but 
fuck it, I'll just add those in later. The really obnoxious part is that I'm going to 
have to outright add route data into the role for each player.

Alright, here's the hard part. Now I have to outright make the route editor come up. That 
means I also have to have the 
What if I just let you straight up put routes in there? Maybe I would have to make a line
renderer, but that wouldn't be too hard.
Yeah, for now, make an edit route tool so you can just do it right there.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DATA_ORoute{
    public string               mOwner;
    public List<Vector2>        mSpots = new List<Vector2>();
}

public class DATA_OffPlay{
    public string               mName;
    public string[]             mTags;
    public string[]             mRoles;
    public List<DATA_ORoute>     mRoutes = new List<DATA_ORoute>();
}

public class ED_OP_Man : MonoBehaviour
{
    public enum STATE{
        S_BEGIN,
        S_NONE_SELECTED,
        S_SELECTED_PLAYER,
        S_ROUTE_EDITING,
        S_END
    }
    public STATE                                    mState;

    public Image                        PF_Ath;
    public Image                        PF_Marker;
    public ED_OP_GFX_Job                GFX_Block;
    public ED_OP_GFX_Job                GFX_QB;
    public ED_OP_GFX_Job                GFX_Rec;
    public ED_OP_GFX_RT_ND              GFX_Rt_Nd;
    public ED_OP_GFX_RT_ND              GFX_Rt_Nd_Set;      // This is what's used for the finished route. First is the route placer.
    public ED_OP_GFX_RT_Trail           GFX_RT_Trail;

    public Text                 mCurTag;
    public Text                 mCurRole;
    public Text                 mNewRole;

    public GameObject           mUI_ChooseEditRoute;
    public GameObject           mUI_RouteSaveCancel;

    public List<ED_OP_GFX_RT_ND>        rRouteNodes;
    public List<string>         lRoles;
    private int                 ixRl;
    public List<ED_OP_Ply>      mAths;
    private int                 ixPly;

    public ED_OP_Grid           rGrid;
    public Vector2              mSnapSpot;          // in indices

    public DATA_OffPlay         mPlay;

    void Start()
    {
        mAths = new List<ED_OP_Ply>();
        rRouteNodes = new List<ED_OP_GFX_RT_ND>();

        mPlay = new DATA_OffPlay();
        mPlay.mName = "NAME ME";

        mSnapSpot.x = rGrid.mAxLth / 2;
        mSnapSpot.y = rGrid.mAxLth - 5;

        LoadValidRoles();
        ENTER_BEGIN();
    }

    void Update()
    {
        switch(mState){
            case STATE.S_BEGIN: RUN_BEGIN(); break;
            case STATE.S_NONE_SELECTED: RUN_NONE_SELECTED(); break;
            case STATE.S_SELECTED_PLAYER: RUN_SELECTED(); break;
            case STATE.S_ROUTE_EDITING: RUN_ROUTE_EDITING(); break;
        }
    }
    
    // Get all the players set up on the grid.
    private void ENTER_BEGIN(){
        mState = STATE.S_BEGIN;

    }
    private void RUN_BEGIN(){
        DATA_Formation f = IO_Formations.FLOAD_FORMATION("Default");
        
        for(int i=0; i<f.mSpots.Length; i++){
            int x = (int)mSnapSpot.x;
            int y = (int)mSnapSpot.y;
            x += (int)(f.mSpots[i].x / 2);
            y += (int)f.mSpots[i].y / 2;
            Vector3 vPos = rGrid.FGetPos(x, y);
            var clone = Instantiate(PF_Ath, vPos, transform.rotation);
            clone.rectTransform.SetParent(rGrid.transform);

            ED_OP_Ply p = clone.GetComponent<ED_OP_Ply>();
            p.mTag = f.mTags[i];
            p.mRole = "NO_ROLE";
            p.mIxX = x;
            p.mIxY = y;
            mAths.Add(p);
        }

        EXIT_BEGIN();
        ENTER_NONE_SELECTED();
    }
    private void EXIT_BEGIN(){
    }
    private void ENTER_NONE_SELECTED(){
        RenderJobs();
        mState = STATE.S_NONE_SELECTED;
    }
    private void RUN_NONE_SELECTED()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
            
            if(hit.collider != null){
                if(hit.collider.GetComponent<ED_OP_Ply>() != null){
                    Debug.Log("player");
                    Debug.Log(hit.transform.position);
                    ED_OP_Ply p = hit.collider.GetComponent<ED_OP_Ply>();
                    for(int i=0; i<mAths.Count; i++){
                        if(mAths[i].mTag == p.mTag){
                            ixPly = i;
                            EXIT_NONE_SELECTED();
                            ENTER_SELECTED();
                            break;
                        }
                    }
                }
            }

        }
    }
    private void EXIT_NONE_SELECTED(){
    }
    private void ENTER_SELECTED(){
        mCurTag.text = mAths[ixPly].mTag;
        mCurRole.text = mAths[ixPly].mRole;

        mState = STATE.S_SELECTED_PLAYER;

        Vector3 vPos = mAths[ixPly].transform.position;
        var clone = Instantiate(PF_Marker, vPos, transform.rotation);
        clone.rectTransform.SetParent(rGrid.transform);
        ixRl = 0;
    }
    private void RUN_SELECTED(){
        if(Input.GetMouseButtonDown(1)){
            EXIT_SELECTED();
            ENTER_NONE_SELECTED();
            return;
        }

        if(mAths[ixPly].mRole == "ROUTE"){
            mUI_ChooseEditRoute.SetActive(true);
        }else{
            mUI_ChooseEditRoute.SetActive(false);
        }
    }
    private void EXIT_SELECTED(){
        mCurTag.text = "TAG ...";
        mCurRole.text = "ROLE ...";
        mNewRole.text = "...";

        ED_OP_Mark[] marks = FindObjectsOfType<ED_OP_Mark>();
        foreach(ED_OP_Mark m in marks){
            Destroy(m.gameObject);
        }

        mUI_ChooseEditRoute.SetActive(false);
    }

    private void ENTER_ROUTE_EDITING()
    {
        mState = STATE.S_ROUTE_EDITING;
        mUI_RouteSaveCancel.SetActive(true);

        // Put a route node down right where the player is.
        Vector3 vRecPos = rGrid.FGetPos(mAths[ixPly].mIxX, mAths[ixPly].mIxY);
        var clone = Instantiate(GFX_Rt_Nd, vRecPos, transform.rotation);
        ED_OP_GFX_RT_ND n = clone.GetComponent<ED_OP_GFX_RT_ND>();
        n.mIxX = mAths[ixPly].mIxX;
        n.mIxY = mAths[ixPly].mIxY;
        rRouteNodes.Add(n);
        clone.GetComponent<Image>().rectTransform.SetParent(rGrid.transform);
    }
    private void EXIT_ROUTE_EDITING(){
        mUI_RouteSaveCancel.SetActive(false);
        RenderJobs();
    }
    private void RUN_ROUTE_EDITING()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
            
            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<ED_OP_Square>() != null)
                {
                    Debug.Log("Hit grid square");
                    Debug.Log(hit.transform.position);
                    ED_OP_Square s = hit.collider.GetComponent<ED_OP_Square>();
                    Vector3 vRecPos = rGrid.FGetPos(s.x, s.y);
                    var clone = Instantiate(GFX_Rt_Nd, vRecPos, transform.rotation);
                    ED_OP_GFX_RT_ND n = clone.GetComponent<ED_OP_GFX_RT_ND>();
                    n.mIxX = s.x;
                    n.mIxY = s.y;
                    rRouteNodes.Add(n);
                    clone.GetComponent<Image>().rectTransform.SetParent(rGrid.transform);
                }
            }

        }
    }

    public void BT_RoleNext(){
        ixRl++;
        if(ixRl > lRoles.Count-1){
            ixRl = 0;
        }
        mNewRole.text = lRoles[ixRl];
    }
    public void BT_RolePrev(){
        ixRl--;
        if(ixRl < 0){
            ixRl = lRoles.Count-1;
        }
        mNewRole.text = lRoles[ixRl];
    }
    public void BT_UpdateRole()
    {
        mAths[ixPly].mRole = mNewRole.text;
        mCurRole.text = mAths[ixPly].mRole;

        RenderJobs();
    }

    public void BT_RouteEdit()
    {
        EXIT_SELECTED();
        ENTER_ROUTE_EDITING();
    }
    public void BT_RouteCancel()
    {
        foreach(ED_OP_GFX_RT_ND n in rRouteNodes){
            Destroy(n.gameObject);
        }
        rRouteNodes.Clear();

        RouteStopEdit();
    }
    /*********************
    Should check that we don't already have a route for this receiver. If we do, just overwrite
    that route.
    ******************************************************************************** */
    public void BT_RouteUse()
    {
        if(rRouteNodes.Count < 2){
            Debug.Log("ERROR. Must have at least 2 route nodes");
            return;
        }

        DATA_ORoute r = new DATA_ORoute();
        r.mOwner = mAths[ixPly].mTag;
        // ------------------------------- Remove existing route if their is one.
        for(int i=0; i<mPlay.mRoutes.Count; i++){
            if(mPlay.mRoutes[i].mOwner == r.mOwner){
                mPlay.mRoutes.RemoveAt(i);
                Debug.Log("Removing existing route for: " + r.mOwner);
                break;
            }
        }

        // ------------------------------ Fill the positions according to the current list
        r.mSpots = new List<Vector2>();
        r.mSpots.Add(new Vector2(0, 0));
        for(int i=1; i<rRouteNodes.Count; i++){

            Vector2 v = new Vector2(); 
            v.x = rRouteNodes[i].mIxX - mAths[ixPly].mIxX;
            v.y = rRouteNodes[i].mIxY - mAths[ixPly].mIxY;
            Debug.Log("Spot: " + v);
            r.mSpots.Add(v);
        }
        mPlay.mRoutes.Add(r);
        Debug.Log("Number of routes: " + mPlay.mRoutes.Count);
    }
    public void RouteStopEdit()
    {
        EXIT_ROUTE_EDITING();
        ENTER_SELECTED();
    }

    // spawn a little graphic depending on the job?
    private void RenderJobs()
    {
        ED_OP_GFX_Job[] gfx = FindObjectsOfType<ED_OP_GFX_Job>();
        foreach(ED_OP_GFX_Job g in gfx){
            Destroy(g.gameObject);
        }

        for(int i=0; i<mAths.Count; i++)
        {
            if(mAths[i].mRole == "BLOCK"){
                Vector3 vPos = mAths[i].transform.position;
                vPos.y -= 2f;
                var clone = Instantiate(GFX_Block, vPos, transform.rotation);
                clone.GetComponent<Image>().rectTransform.SetParent(rGrid.transform);
            } else if(mAths[i].mRole == "ROUTE"){
                Vector3 vPos = mAths[i].transform.position;
                vPos.y -= 2f;
                var clone = Instantiate(GFX_Rec, vPos, transform.rotation);
                clone.GetComponent<Image>().rectTransform.SetParent(rGrid.transform);
            } else if(mAths[i].mRole == "QB"){
                Vector3 vPos = mAths[i].transform.position;
                vPos.y -= 2f;
                var clone = Instantiate(GFX_QB, vPos, transform.rotation);
                clone.GetComponent<Image>().rectTransform.SetParent(rGrid.transform);
            }
        }

        // render a whole bunch of nodes.
        for(int i=0; i<mPlay.mRoutes.Count; i++)
        {
            // ------------- Find the player owning the route, and then bump each node by that much.
            Vector2 vPlayerPosInIdices = new Vector2();
            for(int j=0; j<mAths.Count; j++){
                if(mAths[j].mTag == mPlay.mRoutes[i].mOwner){
                    vPlayerPosInIdices.x = mAths[j].mIxX;
                    vPlayerPosInIdices.y = mAths[j].mIxY;
                    break;
                }
            }

            for(int j=0; j<mPlay.mRoutes[i].mSpots.Count; j++){
                Vector2 v2 = mPlay.mRoutes[i].mSpots[j];
                v2 += vPlayerPosInIdices;
                var clone = Instantiate(GFX_Rt_Nd_Set, rGrid.FGetPos((int)v2.x, (int)v2.y), transform.rotation);
                clone.GetComponent<Image>().rectTransform.SetParent(rGrid.transform);
                
                Debug.Log("Spawning stlkjsd");
            }

        }

        // //Now we gotta render the little dots representing the route.
        // // Luckily, we can actually just place one in the middle of each point.
        // for(int i=1; i<rRouteNodes.Count; i++)
        // {
        //     Vector3 vMid = rRouteNodes[i].transform.position - ((rRouteNodes[i].transform.position - rRouteNodes[i-1].transform.position) / 2f);
        //     var clone = Instantiate(GFX_RT_Trail, vMid, transform.rotation);
        //     clone.GetComponent<Image>().rectTransform.SetParent(rGrid.transform);
        // }
    }

    public void LoadValidRoles()
    {
        lRoles = new List<string>();
        lRoles.Add("BLOCK");
        lRoles.Add("QB");
        lRoles.Add("ROUTE");
    }
}
