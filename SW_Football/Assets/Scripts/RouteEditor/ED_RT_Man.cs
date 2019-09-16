/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class ED_RT_Man : MonoBehaviour
{
    public enum STATE{
        S_BEGIN,
        S_PLACING,
        S_END
    }
    public STATE                                    mState;

    public Image                                    PF_Player;
    
    public ED_RT_Grid                               rGrid;

    void Update()
    {

        switch(mState){
            case STATE.S_BEGIN: RUN_BEGIN(); break;
            case STATE.S_PLACING: RUN_PLACING(); break;
        }

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
            
            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<ED_RT_Square>() != null)
                {
                    Debug.Log("Hit grid square");
                    Debug.Log(hit.transform.position);
                    var clone = Instantiate(PF_Player, hit.transform.position, transform.rotation);
                    clone.rectTransform.SetParent(rGrid.transform);
                }
            }

        }
    }
    
    // I first place a receiver in a certain spot, and that's it.
    private void RUN_BEGIN()
    {
        Vector3 vRecPos = rGrid.FGetPos(rGrid.mSqrLnth/2 + 1, rGrid.mSqrLnth-1);
        var clone = Instantiate(PF_Player, vRecPos, transform.rotation);
        clone.rectTransform.SetParent(rGrid.transform);

        mState = STATE.S_PLACING;
    }

    private void RUN_PLACING()
    {

    }

}
