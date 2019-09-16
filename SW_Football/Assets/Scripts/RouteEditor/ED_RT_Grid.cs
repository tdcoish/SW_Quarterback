/*************************************************************************************
Should be able to access the grid through x,y coordinates.
*************************************************************************************/
using UnityEngine;

public class ED_RT_Grid : MonoBehaviour
{

    private ED_RT_Square[]              mSquares;
    public int                          mSqrLnth = 15;
    public int                          squarePixelSize = 20;

    void Start()
    {
        mSquares = GetComponentsInChildren<ED_RT_Square>();

        for(int i=0; i<mSquares.Length; i++)
        {
            Vector2 v = mSquares[i].transform.position;
            v.x += (i%mSqrLnth)*squarePixelSize;
            v.y -= (i/mSqrLnth)*squarePixelSize;
            mSquares[i].transform.position = v;
        }
    }

    void Update()
    {
        
    }

    public Vector3 FGetPos(int x, int y)
    {
        int el = y*mSqrLnth + x;
        return mSquares[el].transform.position;
    }
}
