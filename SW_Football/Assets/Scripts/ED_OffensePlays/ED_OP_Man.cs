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

    public Text                 mCurTag;
    public Text                 mCurRole;
    public Text                 mNewRole;

    public List<string>         lRoles;
    private int                 ixRl;
    public List<ED_OP_Ply>      mAths;
    private int                 ixPly;

    public ED_OP_Grid           rGrid;
    public Vector2              mSnapSpot;          // in indices


    void Start()
    {
        mAths = new List<ED_OP_Ply>();

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
            p.x = x;
            p.y = y;
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
    private void EXIT_NONE_SELECTED(){}
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
    }

    private void ENTER_ROUTE_EDITING()
    {
        mState = STATE.S_ROUTE_EDITING;
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
    public void BT_RouteStopEdit()
    {
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
    }

    public void LoadValidRoles()
    {
        lRoles = new List<string>();
        lRoles.Add("BLOCK");
        lRoles.Add("QB");
        lRoles.Add("ROUTE");
    }
}
