/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ED_RP_Man : MonoBehaviour
{
    public enum STATE{
        S_BEGIN,
        S_NONE_SELECTED,
        S_SELECTED_PLAYER,
        S_SELECTED_HOOP,
        S_ROUTE_EDITING,
        S_END
    }
    public STATE                                    mState;
    private ED_RP_GFXset                            cGfx;

    public DT_RP_Set                                mSet;

    public ED_RP_Snap                               PF_Snap;
    public ED_RP_Snap                               rSnap;
    public ED_RP_Grd                                rGrd;
    public ED_RP_Pos                                rPos;
    public ED_RP_Rt                                 rRtTl;
    public ED_RP_Tag                                rTagger;

    public InputField                               IF_Name;

    public int                                      mNumHoops = 0;
    public int                                      ixHoop;
    public List<ED_RP_Hp>                           rHoops;
    public ED_RP_Hp                                 PF_Hoop;
    public int                                      mNumRecs = 0;
    public int                                      ixRec;
    public List<ED_RP_Rec>                          rRecs;
    public ED_RP_Rec                                PF_Receiver;

    void Start()
    {
        cGfx = GetComponent<ED_RP_GFXset>();
        mState = STATE.S_BEGIN;

        rRecs = new List<ED_RP_Rec>();
        rHoops = new List<ED_RP_Hp>();
        mSet = new DT_RP_Set();
    }

    void Update()
    {
        switch(mState){
            case STATE.S_BEGIN: RUN_BEGIN(); break;
            case STATE.S_NONE_SELECTED: RUN_NONE_SELECTED(); break;
            case STATE.S_SELECTED_PLAYER: RUN_SELECTED(); break;
            case STATE.S_SELECTED_HOOP: RUN_HOOP_SELECTED(); break;
            case STATE.S_ROUTE_EDITING: RUN_ROUTE_EDITING(); break;
        }
    }

    private void RUN_BEGIN(){
        // need to place the snap at the correct point?
        rSnap = Instantiate(PF_Snap, rGrd.mSquares[12,5].transform.position, transform.rotation);
        rSnap.transform.SetParent(rGrd.transform);

        mState = STATE.S_NONE_SELECTED;
    }
    private void ENTER_NONE_SELECTED(){
        mState = STATE.S_NONE_SELECTED;
    }
    private void RUN_NONE_SELECTED(){

        if(Input.GetMouseButtonDown(0))
        {  
            LayerMask mask = LayerMask.GetMask("RP_Receiver");
            mask |= LayerMask.GetMask("RP_Ring");
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero, 10f, mask);
            
            if(hit.collider != null){
                if(hit.collider.GetComponent<ED_RP_Rec>() != null){
                    ED_RP_Rec p = hit.collider.GetComponent<ED_RP_Rec>();

                    for(int i=0; i<rRecs.Count; i++){
                        if(rRecs[i] == p){
                            Debug.Log("Found it");
                            ixRec = i;
                            EXIT_NONE_SELECTED();
                            ENTER_SELECTED();
                            break;
                        }
                    }
                }
                else if(hit.collider.GetComponent<ED_RP_Hp>() != null){
                    // TODO: ENTER HOOP EDITING STATE.
                    ED_RP_Hp h = hit.collider.GetComponent<ED_RP_Hp>();
                    for(int i=0; i<rHoops.Count; i++)
                    {
                        if(rHoops[i] == h){
                            Debug.Log("Foudn hoop");
                            ixHoop = i;
                            EXIT_NONE_SELECTED();
                            ENTER_HOOP_SELECTED();
                            break;
                        }
                    }
                }
            }
        }
    }
    private void RUN_SELECTED(){
        if(Input.GetMouseButtonDown(1)){
            EXIT_SELECTED();
            ENTER_NONE_SELECTED();
        }
    }
    private void RUN_HOOP_SELECTED()
    {
        if(Input.GetMouseButtonDown(1)){
            EXIT_HOOP_SELECTED();
            ENTER_NONE_SELECTED();
        }
    }

    private void RUN_ROUTE_EDITING(){
        // Every time they press down, we add a route to the route list.
        if(Input.GetMouseButtonDown(0))
        {
            LayerMask mask = LayerMask.GetMask("RP_GrdSqr");
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero, 10f, mask);
            
            if(hit.collider != null){
                if(hit.collider.GetComponent<ED_RP_Sqr>() != null){
                    ED_RP_Sqr s = hit.collider.GetComponent<ED_RP_Sqr>();
                    Debug.Log("X: " + s.x + ", Y: " + s.y);

                    // So now we need to put down another node
                    rRtTl.FPlaceNode(s.x, s.y, rGrd);
                }
            }
        }
    }

    private void EXIT_NONE_SELECTED(){}
    private void ENTER_SELECTED(){
        mState = STATE.S_SELECTED_PLAYER;
        rPos.gameObject.SetActive(true);
        rRtTl.gameObject.SetActive(true);
        rTagger.gameObject.SetActive(true);
        rTagger.FSetCurTag(rRecs[ixRec].mTag);
    }
    private void EXIT_SELECTED(){
        rPos.gameObject.SetActive(false);
        rRtTl.gameObject.SetActive(false);
        rTagger.gameObject.SetActive(false);
    }
    private void ENTER_HOOP_SELECTED(){
        mState = STATE.S_SELECTED_HOOP;
        rPos.gameObject.SetActive(true);
        rTagger.gameObject.SetActive(true);
        rTagger.FSetCurTag(rHoops[ixHoop].mTag);
    }
    private void EXIT_HOOP_SELECTED(){
        rPos.gameObject.SetActive(false);
        rTagger.gameObject.SetActive(false);
    }

    // Always spawn a new receiver right in the middle?
    public void BT_NewRec()
    {
        ED_RP_Rec r = Instantiate(PF_Receiver, rGrd.mSquares[10,5].transform.position, transform.rotation);
        r.mIxX = 10;
        r.mIxY = 5;
        r.transform.SetParent(rGrd.transform);
        r.mTag = "WR" + mNumRecs; mNumRecs++;
        rRecs.Add(r);
    }

    public void BT_NewHoop()
    {
        ED_RP_Hp h = Instantiate(PF_Hoop, rGrd.mSquares[13, 15].transform.position, transform.rotation);
        h.mIxX = 13;
        h.mIxY = 15;
        h.transform.SetParent(rGrd.transform);
        h.mTag = "WR" + mNumHoops; mNumHoops++;
        rHoops.Add(h);
    }

    public void BT_NewRoute()
    {
        mState = STATE.S_ROUTE_EDITING;
        rRtTl.FPlaceFirst(rRecs[ixRec].mIxX, rRecs[ixRec].mIxY, rGrd);
    }

    public void BT_NewSet()
    {

    }

    public void BT_SaveRoute()
    {
        if(rRtTl.rRouteNodes.Count < 2){
            Debug.Log("ERROR. Must have at least 2 route nodes");
            return;
        }

        Debug.Log("Owner should be: " + rRecs[ixRec].mTag);
        DATA_ORoute r = rRtTl.FReturnActiveRoute(rRecs[ixRec].mTag, rRecs[ixRec].mIxX, rRecs[ixRec].mIxY, 2);        
        Debug.Log("Route Tag: " + r.mOwner);
        // ------------------------------- Now save that route in the play itself.
        // ------------------------------- Remove existing route if there is one.
        for(int i=0; i<mSet.mRoutes.Count; i++){
            if(mSet.mRoutes[i].mOwner == r.mOwner){
                mSet.mRoutes.RemoveAt(i);
                Debug.Log("Removing existing route for: " + r.mOwner);
                break;
            }
        }
        mSet.mRoutes.Add(r);

        Debug.Log("Number of routes: " + mSet.mRoutes.Count);
        
        // clear the existing nodes.
        rRtTl.FClear();
        cGfx.FRenderSet(mSet.mRoutes, rRecs, 2, rGrd);
        mState = STATE.S_NONE_SELECTED;
    }

    // Make sure that we gather up the play and then save it
    public void BT_SaveSet()
    {
        // Let me go ahead and try to add up all the things in the scene.
        mSet.mRecs.Clear();
        mSet.mHoops.Clear();
        ED_RP_Hp[] hoops = FindObjectsOfType<ED_RP_Hp>();
        foreach(ED_RP_Hp h in hoops){
            DT_RP_Hoop dh = new DT_RP_Hoop();
            dh.mTag = h.mTag;
            dh.mSize = 1f;
            Vector3 vStart = new Vector3(); vStart.y = 1f;
            vStart.x = h.mIxX - rSnap.mIxX;
            vStart.y = h.mIxY - rSnap.mIxY;
            // convert grid coordinates to starting position...
            dh.mStart = vStart; 
            mSet.mHoops.Add(dh);
        }
        ED_RP_Rec[] recs = FindObjectsOfType<ED_RP_Rec>();
        foreach(ED_RP_Rec r in recs){
            DT_RP_Rec dr = new DT_RP_Rec();
            dr.mTag = r.mTag;
            Vector3 vStart = new Vector3(); vStart.y = 1f;
            vStart.x = r.mIxX - rSnap.mIxX;
            vStart.y = r.mIxY - rSnap.mIxY;
            // convert grid coordinates to starting position...
            dr.mStart = vStart; 
            mSet.mRecs.Add(dr);
        }

        Debug.Log("Num Recs: " + mSet.mRecs.Count + ", Num Hoops: " + mSet.mHoops.Count + ", Num Routes: " + mSet.mRoutes.Count);
        IO_RP.FWriteSet(mSet);
    }

    public void BT_EndEditName()
    {
        mSet.mName = IF_Name.text;
    }

    public void BT_MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SN_MN_Main");
    }

}
