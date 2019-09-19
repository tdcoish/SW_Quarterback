/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class ED_FM_Grid : MonoBehaviour
{

    private ED_FM_Square[]                  mSquares;
    public int                              mAxisLength = 15;
    public int                              squarePixelSize = 20;

    void Start()
    {
        mSquares = GetComponentsInChildren<ED_FM_Square>();

        for(int i=0; i<mSquares.Length; i++)
        {
            Vector2 v = mSquares[i].transform.position;
            mSquares[i].x = i%mAxisLength;
            mSquares[i].y = i/mAxisLength;
            v.x += (i%mAxisLength)*squarePixelSize;
            v.y -= (i/mAxisLength)*squarePixelSize;
            mSquares[i].transform.position = v;
        }    
    }

    public Vector3 FGetPos(int x, int y){
        int el = y*mAxisLength + x;
        return mSquares[el].transform.position;
    }

    
}
