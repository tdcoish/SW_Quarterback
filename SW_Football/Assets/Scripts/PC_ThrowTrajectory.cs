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

    private PC_Controller       rQB;

    // a reference to how strong the current throw is.
    [SerializeField]
    private SO_Float            mThrowPower;
    [SerializeField]
    private SO_Vec3             mThrowAngle;

    [SerializeField]
    private GFX_UI_Traj         PF_PointSphere;
    private GFX_UI_Traj[]       mSpheres;

    void Start()
    {
        mPoints = new Vector3[10];
        for(int i=0; i<20; i++)
        {
            Instantiate(PF_PointSphere, transform.position, transform.rotation);
        }
        mSpheres = FindObjectsOfType<GFX_UI_Traj>();
        Debug.Log("Length: " + mSpheres.Length);

        rQB = FindObjectOfType<PC_Controller>();
    }

    void Update()
    {
        if(rQB.mThrowState == PC_Controller.PC_THROW_STATE.S_CHARGING || rQB.mThrowState == PC_Controller.PC_THROW_STATE.S_FULLYCHARGED){
            Vector3 spot = rQB.transform.position;

            float fThrowPowerMeters = IO_Settings.mSet.lPlayerData.mThrowSpd * mThrowPower.Val;

            float fwdSpd = Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(mThrowAngle.Val, rQB.transform.forward))) * fThrowPowerMeters;
            // this is the raw power multiplied by the angle in the y axis
            float ySpd = Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(mThrowAngle.Val, Vector3.up)) * fThrowPowerMeters;

            // calculate the time it takes for the y val to get to it's negative, so we only render an arc until roughly the ground.
            float tm = ySpd/Physics.gravity.magnitude * 2f;

            for(int i=0; i<10; i++){
               spot = rQB.transform.position;
               // okay this is a hack, I'm just tired of this spawning from the center of the body, not the camera.
               spot.y += 1f;

               float timeStep = i/10f * tm;
               spot += rQB.transform.forward * timeStep * fwdSpd;

               // calculating y needs two parts. initial velocity + time, minus gravity *time*time / 2.0f
               float y = timeStep * ySpd;
                y -= Physics.gravity.magnitude * timeStep * timeStep / 2.0f;
                spot.y += y;
               mPoints[i] = spot;
            }

            // Render lines to the right and left of the player.
            for(int i=0; i<9; i++){
                int ind = i*2;
                mSpheres[ind].transform.position = mPoints[i] + rQB.transform.right*0.1f;
                mSpheres[ind+1].transform.position = mPoints[i] - rQB.transform.right*0.1f;
            }
        }else
        {
            ShoveNodesOffScreen();
        }
    }

    private void ShoveNodesOffScreen()
    {
        Vector3 vPos = new Vector3(0, 1000f, 0f);
        for(int i=0; i<mSpheres.Length; i++)
        {
            mSpheres[i].transform.position = vPos;
        }
    }

}
