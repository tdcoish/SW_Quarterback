/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class AI_Target : MonoBehaviour
{
    [SerializeField]
    private GE_Event                HitTarget;
    [SerializeField]
    private GE_Event                GE_Touchdown;

    public bool                     mCaughtBall = false;

    private AI_Receiver             mOwner;

    public bool                     mInEndzone = false;

    void Start()
    {
        mOwner = GetComponentInParent<AI_Receiver>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GM_Endzone>() != null)
        {
            mInEndzone = true;
        }

        if(other.GetComponent<PROJ_Football>() != null){
            mCaughtBall = true;
            HitTarget.Raise(null);

            // but now we also need to check if we're within an endzone.
            if(mInEndzone){
                GE_Touchdown.Raise(null);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<GM_Endzone>()){
            mInEndzone = false;
        }
    }
}
