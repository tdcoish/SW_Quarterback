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
    private AI_Runner               cRunner;

    public bool                     mInEndzone = false;

    private GameObject              rFootball;

    void Start()
    {
        cRunner = GetComponentInParent<AI_Runner>();
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

            if(cRunner != null){
                cRunner.mActivated = true;
                PROJ_Football fBallRef = FindObjectOfType<PROJ_Football>();
                cRunner.rFootball = fBallRef;
                Rigidbody rigid = fBallRef.GetComponent<Rigidbody>();
                rigid.velocity = Vector3.zero;
                rigid.useGravity = false;
                rigid.detectCollisions = false;
                fBallRef.transform.parent = transform.parent;
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
