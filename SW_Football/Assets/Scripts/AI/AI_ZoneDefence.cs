/*************************************************************************************
For now, just waits for the ball to be thrown, then breaks on the ball.
*************************************************************************************/
using UnityEngine;

public class AI_ZoneDefence : MonoBehaviour
{

    private Rigidbody           cRigid;

    public float                mMaxVel = 3f;

    [SerializeField]
    private bool                mBreakOnBall;

    [SerializeField]
    private SO_Vec3             rValidFootball;

    void Start()
    {
        cRigid = GetComponent<Rigidbody>();
        mBreakOnBall = false;
    }

    // for now, they just chill, until the ball is thrown.
    void Update()
    {
        if(!mBreakOnBall) return;

        // GoDirectlyToBall();
        InterceptBall();
    }

    // triggered by ball thrown event
    public void BreakOnBall()
    {
        Debug.Log("Breaking on ball");
        mBreakOnBall = true;
    }

    // stupidest one, they just run straight to the ball. Leads to them always moving forwards
    void GoDirectlyToBall()
    {
        // calculate the straight distance.
        Vector3 dis = rValidFootball.Val - transform.position;
        dis = Vector3.Normalize(dis);
        cRigid.velocity = dis * mMaxVel;
    }

    // Ignores other players. They find the shortest path based on the football velocity and move there.
    void InterceptBall()
    {
        // This is slow, but it's decent for debugging.
        // for now we can just hack it with the football.transform.right vector, but it will get more complicated later.
        PROJ_Football fBallRef = FindObjectOfType<PROJ_Football>();

        Vector3 dis = fBallRef.transform.position - transform.position;
        dis = Vector3.Normalize(dis);

        // go to the right.
        if(Vector3.Dot(fBallRef.transform.right, dis) >= 0f){
            cRigid.velocity = mMaxVel * fBallRef.transform.right;
        }else{
            cRigid.velocity = mMaxVel * -fBallRef.transform.right;
        }
    }
}
