/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ED_OP_Grid : MonoBehaviour
{
    private ED_OP_Square[]                  mSquares;
    public int                              mAxLth = 15;
    public int                              mSqrPixSz = 20;

    void Start()
    {
        mSquares = GetComponentsInChildren<ED_OP_Square>();
        
        for(int i=0; i<mSquares.Length; i++)
        {
            Vector2 v = mSquares[i].transform.position;
            mSquares[i].x = i%mAxLth;
            mSquares[i].y = i/mAxLth;
            v.x += (i%mAxLth)*mSqrPixSz;
            v.y -= (i/mAxLth)*mSqrPixSz;
            mSquares[i].transform.position = v;
        }  
    }

    public Vector3 FGetPos(int x, int y){
        int el = y*mAxLth + x;
        return mSquares[el].transform.position;
    }
}
