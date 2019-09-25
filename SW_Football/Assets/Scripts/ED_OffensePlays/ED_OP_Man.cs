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
using UnityEngine.SceneManagement;

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
    public enum ROLE_STATE{
        S_Changeable,
        S_Static
    }
    public ROLE_STATE                               mRoleSt;

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
    public Text                 mPlayName;
    public UI_SavedTxt          mSaved;
    
    public Dropdown             mDropFormation;
    public Dropdown             mDropPlays;

    public InputField           _inName;

    public GameObject           mUI_ChooseEditRoute;
    public GameObject           mUI_RouteSaveCancel;
    public GameObject           mUI_RoleChanger;

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

        IO_Formations.FLoadAllFormations();
        RefreshFormationDropdown();
        RefreshPlaysDropdown();

        mAths = new List<ED_OP_Ply>();
        rRouteNodes = new List<ED_OP_GFX_RT_ND>();

        mPlay = IO_OffensivePlays.FLoadPlay("Default");
        mPlay.mName = "NO NAME";

        mSnapSpot.x = rGrid.mAxLth / 2;
        mSnapSpot.y = rGrid.mAxLth - 5;

        LoadValidRoles();
        ENTER_BEGIN();

        // DATA_OffPlay oPlay = IO_OffensivePlays.FLoadPlay("Default");
        // oPlay.mName = "TestWrite";
        // oPlay.mFormation = "Default";
        // IO_OffensivePlays.FWritePlay(oPlay);
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
        mRoleSt = ROLE_STATE.S_Static;
    }
    private void RUN_BEGIN(){
        SetUpNewFormation(mPlay.mFormation);
        RenderJobs();

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
        if(mAths[ixPly].mTag.Contains("OL") || mAths[ixPly].mTag.Contains("QB")){
            mRoleSt = ROLE_STATE.S_Static;
            mUI_RoleChanger.SetActive(false);
        }else{
            mRoleSt = ROLE_STATE.S_Changeable;
            mUI_RoleChanger.SetActive(true);
        }

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
        mUI_RoleChanger.SetActive(false);
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
        foreach(ED_OP_GFX_RT_ND n in rRouteNodes){
            Destroy(n.gameObject);
        }
        rRouteNodes.Clear();

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
    // If they used to run a route, we have to remove that from the routes.
    public void BT_UpdateRole()
    {
        mAths[ixPly].mRole = mNewRole.text;
        mCurRole.text = mAths[ixPly].mRole;

        mPlay.mRoles[ixPly] = mAths[ixPly].mRole;

        // If we have a route with that owner, then just kill it.
        for(int i=0; i<mPlay.mRoutes.Count; i++){
            if(mPlay.mRoutes[i].mOwner == mAths[ixPly].mTag){
                Debug.Log("Getting rid of old route");
                mPlay.mRoutes.RemoveAt(i);
                break;
            }
        }
        RenderJobs();
    }

    public void BT_RouteEdit()
    {
        EXIT_SELECTED();
        ENTER_ROUTE_EDITING();
    }
    public void BT_RouteCancel()
    {
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

        RouteStopEdit();
    }
    public void RouteStopEdit()
    {
        EXIT_ROUTE_EDITING();
        ENTER_SELECTED();
    }

    private void DestroyJobGraphics()
    {
        ED_OP_GFX_Job[] gfx = FindObjectsOfType<ED_OP_GFX_Job>();
        foreach(ED_OP_GFX_Job g in gfx){
            Destroy(g.gameObject);
        }
        ED_OP_GFX_RT_ND[] gfx_nodes = FindObjectsOfType<ED_OP_GFX_RT_ND>();
        foreach(ED_OP_GFX_RT_ND n in gfx_nodes){
            Destroy(n.gameObject);
        }
        ED_OP_GFX_RT_Trail[] trails = FindObjectsOfType<ED_OP_GFX_RT_Trail>();
        foreach(ED_OP_GFX_RT_Trail t in trails){
            Destroy(t.gameObject);
        }
    }
    // spawn a little graphic depending on the job?
    private void RenderJobs()
    {
        DestroyJobGraphics();

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
                
                // ------------------- Also spawn little trail paths.
                if(j > 0){
                    Vector3 vEnd = rGrid.FGetPos((int)v2.x, (int)v2.y);
                    Vector2 vStart = mPlay.mRoutes[i].mSpots[j-1];
                    vStart += vPlayerPosInIdices;
                    Vector3 vIter = rGrid.FGetPos((int)vStart.x, (int)vStart.y);
                    float fStep = 5f;
                    Vector3 vDir = Vector3.Normalize(vEnd - vIter);
                    while(true){
                        vIter += vDir * fStep;
                        if(Vector3.Dot(vIter - vEnd, vDir) > 0f){
                            break;
                        }
                        var cloney = Instantiate(GFX_RT_Trail, vIter, transform.rotation);
                        cloney.GetComponent<Image>().rectTransform.SetParent(rGrid.transform);

                    }
                }
            }

        }
    }

    // Assumes formations have already been loaded in.
    private void RefreshFormationDropdown()
    {
        List<string> formationNames = new List<string>();
        foreach(DATA_Formation f in IO_Formations.mFormations){
            formationNames.Add(f.mName);
        }
        mDropFormation.ClearOptions();
        mDropFormation.AddOptions(formationNames);
    }
    private void RefreshPlaysDropdown()
    {
        mDropPlays.options = new List<Dropdown.OptionData>();
        string[] offPlayNames = IO_OffensivePlays.FReturnPlayNames();
        foreach(string s in offPlayNames){
            mDropPlays.options.Add(new Dropdown.OptionData(s));
        }
    }

    public void BT_FormationUpdate()
    {
        string text = mDropFormation.captionText.text;
        Debug.Log("They want to load this formation: " + text);
        SetUpNewFormation(text);
    }

    public void BT_PlaysUpdate()
    {
        string text = mDropPlays.captionText.text;
        Debug.Log("They want to see this play shown: " + text);
        DATA_OffPlay p = IO_OffensivePlays.FLoadPlay(text);
        p.mName = "NO NAME";
        SetUpNewFormation(p.mFormation);
        SetPlayerRolesFromPlayData(p);
        mPlay = p;
        RenderJobs();
    }

    // This has nasty side effects. For one, overwrites existing roles.
    private void SetUpNewFormation(string name)
    {
        // ----------------------------- Destroy the old spawned items
        ED_OP_Ply[] plys = FindObjectsOfType<ED_OP_Ply>();
        foreach(ED_OP_Ply p in plys){
            Destroy(p.gameObject);
        }

        // ----------------------------- Clear the play data.
        mPlay.mName = "NAME ME";
        mPlay.mFormation = name;
        mPlay.mRoutes.Clear();

        // ----------------------------- Spawn the new formation.
        mAths.Clear();
        DATA_Formation f = IO_Formations.FLOAD_FORMATION(name);
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
            if(p.mTag.Contains("OL")){
                p.mRole = "BLOCK";
            }else if(p.mTag.Contains("QB")){
                p.mRole = "QB";
            }
            p.mIxX = x;
            p.mIxY = y;
            mAths.Add(p);
        }

        // --------------------------- Clear the active route list and the graphics.
        rRouteNodes.Clear();
        DestroyJobGraphics(); 
    }

    private void SetPlayerRolesFromPlayData(DATA_OffPlay play)
    {
        if(mAths.Count != play.mRoles.Length){
            Debug.Log("ERROR. Number of players in scene does not match number of players in play");
            Debug.Log(mAths.Count);
            Debug.Log(play.mRoles.Length);
            return;
        }
        for(int i=0; i<mAths.Count; i++){
            for(int j=0; j<play.mRoles.Length; j++){
                if(play.mTags[j] == mAths[i].mTag){
                    mAths[i].mRole = play.mRoles[j];
                }
            }
        }
        return;
    }

    // I realize now that we need some way of building the play from the scene, at least for the most part.
    // God this is ugly. It's half baked, and relies on getting the rest previously.
    private void GetPlayFromSceneData()
    {   
        if(mPlay.mFormation != mDropFormation.options[mDropFormation.value].text){
            Debug.Log("The play formation doesn't match the dropdown formation. Probably an issue");
        }
        mPlay.mRoles = new string[mAths.Count];
        mPlay.mTags = new string[mAths.Count];
        for(int i=0; i<mAths.Count; i++){
            mPlay.mRoles[i] = mAths[i].mRole;
            mPlay.mTags[i] = mAths[i].mTag;
        }
    }
    public void BT_SavePlay()
    {
        if(mPlay.mName == "NAME ME"){
            Debug.Log("No name for this play");
            return;
        }
        // Course, only gets us some of the play. Sigh.
        GetPlayFromSceneData();
        foreach(string s in mPlay.mRoles){
            if(s == "NO_ROLE"){
                Debug.Log("One or more player does not have a role");
                return;
            }
        }
        for(int i=0; i<mAths.Count; i++)
        {
            mPlay.mTags[i] = mAths[i].mTag;
            mPlay.mRoles[i] = mAths[i].mRole;
        }
        // ---------------- give it a name corresponding to personnel, RB->TE
        int numRB = 0;
        int numTE = 0;
        foreach(string s in mPlay.mTags){
            if(s.Contains("TE")){
                numTE++;
            }else if(s.Contains("RB")){
                numRB++;
            }
        }
        string sOldName = mPlay.mName;
        string sPlayName = numRB+"-"+numTE+"_"+mPlay.mName;
        mPlay.mName = sPlayName;
        Debug.Log("Saving as: " + sPlayName);
        IO_OffensivePlays.FWritePlay(mPlay);
        mPlay.mName = sOldName;

        RefreshPlaysDropdown();
        mSaved.FSetVisible();
    }

    public void IF_Name(){
        mPlay.mName = _inName.text;
        mPlayName.text = mPlay.mName;
    }

    public void LoadValidRoles()
    {
        lRoles = new List<string>();
        lRoles.Add("BLOCK");
        lRoles.Add("QB");
        lRoles.Add("ROUTE");
    }

    public void BT_MainMenu()
    {
        SceneManager.LoadScene("SN_MN_Main");        
    }
}
