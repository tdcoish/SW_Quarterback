/*************************************************************************************
Just a hack for now.
*************************************************************************************/
using UnityEngine;

public class HACK_PlyControl : MonoBehaviour
{

    public GE_Event         GE_Snap;
    public bool             mSnapped = false;
    void Start()
    {
        
    }

    void Update()
    {
        if(!mSnapped){
            if(Input.GetKeyDown(KeyCode.Space)){
                GE_Snap.Raise(null);
            }
        }
    }
}
