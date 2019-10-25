/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class ED_RP_Grd : MonoBehaviour
{
    public ED_RP_Sqr                        PF_Square;

    public ED_RP_Sqr[,]                     mSquares;
    public int                              mAxLth = 50;
    public int                              mSqrPixSz = 10;
    public GameObject                       rGridStart;

    void Start()
    {
        // now, we're actually spawning them all in instead.
        mSquares = new ED_RP_Sqr[mAxLth,mAxLth];

        for(int x=0; x<mAxLth; x++){
            for(int y=0; y<mAxLth; y++){
                Vector2 v = rGridStart.transform.position;
                v.x += mSqrPixSz * x;
                v.y += mSqrPixSz * y;           // so I never have to flip the x,y.

                mSquares[x,y] = Instantiate(PF_Square, v, transform.rotation);
                mSquares[x,y].transform.SetParent(transform);
                mSquares[x,y].x = x; mSquares[x,y].y = y;
            }
        }  

        Debug.Log("Done");
    }
}
