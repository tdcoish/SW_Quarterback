/*************************************************************************************
For now, just waits for the ball to be thrown, then breaks on the ball.

if the ball is already below our height, then we care if it's going up or down.
if it's going up and below, we'll just do some quick and dirty calculation of if we're
3m within the ball, and then we move towards the ball.
if further, then we move to the point where it has come down enough on the other side.
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
    
    [Tooltip("Max height the ball can be for us to still tip")]
    public float                mHeightOfInfluence = 2f;

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
    void InterceptBallAssumingNoHeight()
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

    void InterceptBall()
    {
        PROJ_Football fBallRef = FindObjectOfType<PROJ_Football>();
        Vector3 spotToMoveTo = new Vector3();

        Vector3 dis = fBallRef.transform.position - transform.position;
        dis = Vector3.Normalize(dis);

        // we need to find the spot when the ball will be low enough for us to affect.
        // since it's a parabola, we should get two points representing that height.
        Vector3 ballvel = fBallRef.GetComponent<Rigidbody>().velocity;
        float mag = Vector3.Magnitude(ballvel);
        
        // if it's beneath our max height
        if(fBallRef.transform.position.y < mHeightOfInfluence)
        {
            // if the ball is going down, just move to the ball
            // or if we're really close (DLine) move directly to the ball.
            if(ballvel.y <= 0f || Vector3.Distance(transform.position, fBallRef.transform.position) < 1f){
                spotToMoveTo = fBallRef.transform.position;
            }else{
                // theoretically I should be figuring out where the ball lands and then going there, but I just don't want to.
                float yDisToGo = fBallRef.transform.position.y - mHeightOfInfluence;
                float finalVel = Mathf.Sqrt(Mathf.Abs(ballvel.y*ballvel.y + 2*Physics.gravity.magnitude*yDisToGo)) * -1f;      // kind of cheating, since we have a negative y vel, which Sqrt ruins
                float tm = Mathf.Abs((finalVel - ballvel.y))/Physics.gravity.magnitude; 

                // now we calc the spot in that forward vector.
                // lol, just chop out the y comp
                Vector3 fwdComp = ballvel;
                fwdComp.y = 0f;
                
                spotToMoveTo = fBallRef.transform.position + fwdComp*tm;
                spotToMoveTo.y = 0f;
            }
        }else{
            // now we just calc the spot when it will be within reach.
            // start by calculating the time it needs.
            // omfg this took forever.
            float yDisToGo = fBallRef.transform.position.y - mHeightOfInfluence;
            float finalVel = Mathf.Sqrt(Mathf.Abs(ballvel.y*ballvel.y + 2*Physics.gravity.magnitude*yDisToGo)) * -1f;      // kind of cheating, since we have a negative y vel, which Sqrt ruins
            float tm = Mathf.Abs((finalVel - ballvel.y))/Physics.gravity.magnitude; 

            // now we calc the spot in that forward vector.
            // lol, just chop out the y comp
            Vector3 fwdComp = ballvel;
            fwdComp.y = 0f;
            
            spotToMoveTo = fBallRef.transform.position + fwdComp*tm;
            spotToMoveTo.y = 0f;
        }

        // now we just move to the spot.
        cRigid.velocity = mMaxVel * Vector3.Normalize(spotToMoveTo - transform.position);
    }

    public void OnBallHitsGround()
    {
        mBreakOnBall = false;
        cRigid.velocity = Vector3.zero;
    }

    public void CalcInterceptSpot()
    {
        
    }
}
