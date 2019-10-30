/*************************************************************************************
The positioner.
Ugh. Having to make this work with either the hoops or the receivers is a bitch.
*************************************************************************************/
using UnityEngine;

public struct TDC_IntVec{
    public TDC_IntVec(int inX, int inY){
        x = inX;
        y = inY;
    }
    public TDC_IntVec(float inX, float inY){
        x = (int)inX;
        y = (int)inY;
    }
    public int                  x;
    public int                  y;
}

public class ED_RP_Pos : MonoBehaviour
{
    private ED_RP_Man                                   cMan;

    void Start()
    {
        cMan = GetComponentInParent<ED_RP_Man>();    
    }

    public void BT_LeftHard()
    {
        MoveSelectedEntity(-5, 0, cMan.mState);
    }
    public void BT_Left()
    {
        MoveSelectedEntity(-1, 0, cMan.mState);
    }
    public void BT_UpHard()
    {
        MoveSelectedEntity(0, 5, cMan.mState);
    }
    public void BT_Up()
    {
        MoveSelectedEntity(0, 1, cMan.mState);
    }
    public void BT_RightHard()
    {
        MoveSelectedEntity(5, 0, cMan.mState);
    }
    public void BT_Right()
    {
        MoveSelectedEntity(1, 0, cMan.mState);
    }
    public void BT_DownHard()
    {
        MoveSelectedEntity(0, -5, cMan.mState);
    }
    public void BT_Down()
    {
        MoveSelectedEntity(0, -1, cMan.mState);
    }

    // Massive side effects, but it's worth it for readibility. Passing in state to make it more obvious.
    private void MoveSelectedEntity(int wd, int ht, ED_RP_Man.STATE state)
    {
        if(state == ED_RP_Man.STATE.S_SELECTED_PLAYER){
            TDC_IntVec v = FuncGetNewPos(cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY, wd, ht, cMan.rGrd.mAxLth);
            cMan.rRecs[cMan.ixRec].mIxX = v.x; cMan.rRecs[cMan.ixRec].mIxY = v.y;
            cMan.rRecs[cMan.ixRec].transform.position = cMan.rGrd.mSquares[cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY].transform.position;
        }else if(state == ED_RP_Man.STATE.S_SELECTED_HOOP){
            TDC_IntVec v = FuncGetNewPos(cMan.rHoops[cMan.ixHoop].mIxX, cMan.rHoops[cMan.ixHoop].mIxY, wd, ht, cMan.rGrd.mAxLth);
            cMan.rHoops[cMan.ixHoop].mIxX = v.x; cMan.rHoops[cMan.ixHoop].mIxY = v.y;
            cMan.rHoops[cMan.ixHoop].transform.position = cMan.rGrd.mSquares[cMan.rHoops[cMan.ixHoop].mIxX, cMan.rHoops[cMan.ixHoop].mIxY].transform.position;
        }
    }

    private TDC_IntVec FuncGetNewPos(int curX, int curY, int wd, int ht, int axLngth)
    {
        int newX = curX + wd;
        int newY = curY + ht;

        if(newX < 0){
            newX = 0;
        }
        if(newX >= axLngth){
            newX = axLngth-1;
        }
        if(newY < 0){
            newY = 0;
        }
        if(newY >= axLngth){
            newY = axLngth-1;
        }
        
        return new TDC_IntVec(newX, newY);
    }
}
