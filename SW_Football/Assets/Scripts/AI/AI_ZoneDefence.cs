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

        Debug.Log("setting vel");

        // calculate the straight distance.
        Vector3 dis = rValidFootball.Val - transform.position;
        Debug.Log("Distance: " + dis);
        dis = Vector3.Normalize(dis);
        cRigid.velocity = dis * mMaxVel;
        Debug.Log("Vel: " + cRigid.velocity);
    }

    // triggered by ball thrown event
    public void BreakOnBall()
    {
        Debug.Log("Breaking on ball");
        mBreakOnBall = true;
    }
}
