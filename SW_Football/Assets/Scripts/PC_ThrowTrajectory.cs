/*************************************************************************************
Displays the trajectory of a throw.


Horizontal distance traveled is simple: d = v*t;
Vertical Distance traveled == 1/2 * g * t * t // for free fall
+ v*t in vertical axis for motion.

Total formula == d = (vInitial * t) - 1/2 *g*t*t

Shit, I need to know the original velocity in all axis. Let's just pretend that we start with
some weak y value.

Alright, first problem is we need to figure out what the real direction is, not just assume perfect 
x, y axis alignment.
Solved.
Now need to actually know how hard the throw is at any given moment in time.
*************************************************************************************/
using UnityEngine;

public class PC_ThrowTrajectory : MonoBehaviour
{
    public Vector3[]            mPoints;

    private bool                mRender = false;

    // a reference to how strong the current throw is.
    [SerializeField]
    private SO_Float            mThrowPower;
    [SerializeField]
    private SO_Vec3             mThrowAngle;

    [SerializeField]
    private SO_Transform        QBRef;

    void Start()
    {
        mPoints = new Vector3[10];
    }

    void Update()
    {
        if(mRender){

            // calculate the trajectory over x seconds.
            float timeToCalcFor = 2f;
            Vector3 spot = QBRef.Val.position;

            float yPow = mThrowPower.Val;
            float fwdPow = mThrowPower.Val;

            for(int i=0; i<10; i++){
               spot = QBRef.Val.position;
               spot += i/10f * (fwdPow * timeToCalcFor) * QBRef.Val.forward;
            //    spot.y += i/10f * ((4f*timeToCalcFor) - (Physics.gravity.magnitude * i/10f * i/10f));
               mPoints[i] = spot;
            }

            for(int i=0; i<9; i++){
                Debug.DrawLine(mPoints[i], mPoints[i+1], Color.green, 2f);
            }
        }
    }

    public void QB_Winding(){
        mRender = true;
    }
    public void QB_Thrown(){
        mRender = false;
    }
    public void QB_Clutch(){
        mRender = false;
    }

}
