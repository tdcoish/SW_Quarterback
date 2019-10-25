﻿/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class ED_RP_Man : MonoBehaviour
{
    public enum STATE{
        S_BEGIN,
        S_NONE_SELECTED,
        S_SELECTED_PLAYER,
        S_ROUTE_EDITING,
        S_END
    }
    public STATE                                    mState;

    private DT_RP_Set                               mSet;

    public ED_RP_Snap                               PF_Snap;
    public ED_RP_Snap                               rSnap;
    public ED_RP_Grd                                rGrd;
    public ED_RP_Pos                                rPos;
    public ED_RP_Rt                                 rRtTl;

    public int                                      ixRec;
    public List<ED_RP_Rec>                          rRecs;
    public ED_RP_Rec                                PF_Receiver;

    void Start()
    {
        mState = STATE.S_BEGIN;

        rRecs = new List<ED_RP_Rec>();
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

    private void RUN_BEGIN(){
        // need to place the snap at the correct point?
        rSnap = Instantiate(PF_Snap, rGrd.mSquares[12,5].transform.position, transform.rotation);
        rSnap.transform.SetParent(rGrd.transform);

        mState = STATE.S_NONE_SELECTED;
    }
    private void RUN_NONE_SELECTED(){

        if(Input.GetMouseButtonDown(0))
        {
            LayerMask mask = LayerMask.GetMask("RP_Receiver");
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero, 10f, mask);
            
            if(hit.collider != null){
                Debug.Log("Hit something : " + hit.collider);
                if(hit.collider.GetComponent<ED_RP_Rec>() != null){
                    Debug.Log("rece");
                    Debug.Log(hit.transform.position);
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
            }

        }
    }
    private void RUN_SELECTED(){

    }
    private void RUN_ROUTE_EDITING(){}

    private void EXIT_NONE_SELECTED(){}
    private void ENTER_SELECTED(){
        mState = STATE.S_SELECTED_PLAYER;
        rPos.gameObject.SetActive(true);
        rRtTl.gameObject.SetActive(true);
    }
    private void EXIT_SELECTED(){
        rPos.gameObject.SetActive(false);
    }

    // Always spawn a new receiver right in the middle?
    public void BT_NewRec()
    {
        ED_RP_Rec r = Instantiate(PF_Receiver, rGrd.mSquares[10,5].transform.position, transform.rotation);
        r.mIxX = 10;
        r.mIxY = 5;
        r.transform.SetParent(rGrd.transform);
        rRecs.Add(r);
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

        DATA_ORoute r = rRtTl.FReturnActiveRoute(rRecs[ixRec].mTag, rRecs[ixRec].mIxX, rRecs[ixRec].mIxY, 2);        

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

        // RouteStopEdit();
    }

    public void BT_MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SN_MN_Main");
    }
}
