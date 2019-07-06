﻿/*************************************************************************************
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

    [SerializeField]
    private GameObject          PointSphere;

    void Start()
    {
        mPoints = new Vector3[10];
    }

    void Update()
    {
        if(mRender){
            Vector3 spot = QBRef.Val.position;

            float fwdSpd = Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(mThrowAngle.Val, QBRef.Val.forward))) * mThrowPower.Val;
            // this is the raw power multiplied by the angle in the y axis
            float ySpd = Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(mThrowAngle.Val, Vector3.up)) * mThrowPower.Val;

            // calculate the time it takes for the y val to get to it's negative, so we only render an arc until roughly the ground.
            float tm = ySpd/Physics.gravity.magnitude * 2f;

            for(int i=0; i<10; i++){
               spot = QBRef.Val.position;

               float timeStep = i/10f * tm;
               spot += QBRef.Val.forward * timeStep * fwdSpd;

               // calculating y needs two parts. initial velocity + time, minus gravity *time*time / 2.0f
               float y = timeStep * ySpd;
                y -= Physics.gravity.magnitude * timeStep * timeStep / 2.0f;
                spot.y += y;
               mPoints[i] = spot;
            }

            for(int i=0; i<9; i++){
                Debug.DrawLine(mPoints[i], mPoints[i+1], Color.green, 0.1f);
                Instantiate(PointSphere, mPoints[i], transform.rotation);
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
