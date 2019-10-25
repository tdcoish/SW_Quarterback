/*************************************************************************************
The positioner.
*************************************************************************************/
using UnityEngine;

public struct TDC_IntVec{
    public TDC_IntVec(int inX, int inY){
        x = inX;
        y = inY;
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
        TDC_IntVec v = FuncGetNewPos(cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY, -5, 0, 50);
        cMan.rRecs[cMan.ixRec].mIxX = v.x; cMan.rRecs[cMan.ixRec].mIxY = v.y;
        cMan.rRecs[cMan.ixRec].transform.position = cMan.rGrd.mSquares[cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY].transform.position;
    }
    public void BT_Left()
    {
        TDC_IntVec v = FuncGetNewPos(cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY, -1, 0, cMan.rGrd.mAxLth);
        cMan.rRecs[cMan.ixRec].mIxX = v.x; cMan.rRecs[cMan.ixRec].mIxY = v.y;
        cMan.rRecs[cMan.ixRec].transform.position = cMan.rGrd.mSquares[cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY].transform.position;
    }
    public void BT_UpHard()
    {
        TDC_IntVec v = FuncGetNewPos(cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY, 0, 5, cMan.rGrd.mAxLth);
        cMan.rRecs[cMan.ixRec].mIxX = v.x; cMan.rRecs[cMan.ixRec].mIxY = v.y;
        cMan.rRecs[cMan.ixRec].transform.position = cMan.rGrd.mSquares[cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY].transform.position;
    }
    public void BT_Up()
    {
        TDC_IntVec v = FuncGetNewPos(cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY, 0, 1, cMan.rGrd.mAxLth);
        cMan.rRecs[cMan.ixRec].mIxX = v.x; cMan.rRecs[cMan.ixRec].mIxY = v.y;
        cMan.rRecs[cMan.ixRec].transform.position = cMan.rGrd.mSquares[cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY].transform.position;
    }
    public void BT_RightHard()
    {
        TDC_IntVec v = FuncGetNewPos(cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY, 5, 0, cMan.rGrd.mAxLth);
        cMan.rRecs[cMan.ixRec].mIxX = v.x; cMan.rRecs[cMan.ixRec].mIxY = v.y;
        cMan.rRecs[cMan.ixRec].transform.position = cMan.rGrd.mSquares[cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY].transform.position;
    }
    public void BT_Right()
    {
        TDC_IntVec v = FuncGetNewPos(cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY, 1, 0, cMan.rGrd.mAxLth);
        cMan.rRecs[cMan.ixRec].mIxX = v.x; cMan.rRecs[cMan.ixRec].mIxY = v.y;
        cMan.rRecs[cMan.ixRec].transform.position = cMan.rGrd.mSquares[cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY].transform.position;
    }
    public void BT_DownHard()
    {
        TDC_IntVec v = FuncGetNewPos(cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY, 0, -5, cMan.rGrd.mAxLth);
        cMan.rRecs[cMan.ixRec].mIxX = v.x; cMan.rRecs[cMan.ixRec].mIxY = v.y;
        cMan.rRecs[cMan.ixRec].transform.position = cMan.rGrd.mSquares[cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY].transform.position;
    }
    public void BT_Down()
    {
        TDC_IntVec v = FuncGetNewPos(cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY, 0, -1, cMan.rGrd.mAxLth);
        cMan.rRecs[cMan.ixRec].mIxX = v.x; cMan.rRecs[cMan.ixRec].mIxY = v.y;
        cMan.rRecs[cMan.ixRec].transform.position = cMan.rGrd.mSquares[cMan.rRecs[cMan.ixRec].mIxX, cMan.rRecs[cMan.ixRec].mIxY].transform.position;
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
