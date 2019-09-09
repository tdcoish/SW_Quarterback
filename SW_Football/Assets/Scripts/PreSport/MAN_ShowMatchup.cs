/*************************************************************************************
This is just the rip-off scene of the players rising out of the ice from Wayne 
Gretzky 3D hockey.

The players need to slowly go up, while they slowly rotate around.
So if I make them 2 units down, they move up at 2 units/per second. They also rotate 180 
in two seconds.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MAN_ShowMatchup : MonoBehaviour
{
    public enum State{
        S_Raising,
        S_Standing,
        S_Posing,
        S_Done
    }

    private State               mState;

    // They finish at +1y.
    public float                mYDis = 2f;
    public float                mRaisingTime = 2f;
    private float               mTime;
    private float               mStartTime;
    public float                mUprightTime = 1f;      // The time they chill before they go into their "stance"
    public float                mStanceTime = 2f;       // The time it takes before the scene loads.

    public GameObject           mLeftPlayer;
    public GameObject           mRightPlayer;

    public Vector3              mCamStartPos;
    public Vector3              mCamFinishPos;
    public Vector3              mRotCamStart;
    public Vector3              mRotCamFinish;

    public Camera               rCam;

    private void Start()
    {
        mState = State.S_Raising;
        mTime = Time.time;
        mStartTime = Time.time;

        rCam = Camera.main;
    }

    void Update()
    {
        switch(mState)
        {
            case State.S_Raising: RUN_Raising(); break;
            case State.S_Standing: RUN_Standing(); break;
            case State.S_Posing: RUN_Posing(); break;
        }

        TransitionCamera();
    }

    void RUN_Raising()
    {
        if(Time.time - mTime >= mRaisingTime)
        {
            mState = State.S_Standing;
            mTime = Time.time;
            return;
        }
        RaisePlayers();
    }

    void RUN_Standing()
    {
        if(Time.time - mTime > mUprightTime){
            mTime = Time.time;
            mState = State.S_Posing;
        }
    }

    // For now the players just tilt down. In reality, I would set an animation for them.
    void RUN_Posing()
    {
        if(Time.time - mTime > mStanceTime)
        {
            SceneManager.LoadScene("SN_Play");
        }

        Vector3 vTilt = mLeftPlayer.transform.rotation.eulerAngles;
        vTilt.x = -45f;
        mLeftPlayer.transform.rotation = Quaternion.Euler(vTilt);
        vTilt = mRightPlayer.transform.rotation.eulerAngles;
        vTilt.x = 45f;
        mRightPlayer.transform.rotation = Quaternion.Euler(vTilt);

    }

    private void RaisePlayers()
    {
        Vector3 vPos = mLeftPlayer.transform.position;
        vPos.y += Time.deltaTime * (mYDis/mRaisingTime);
        mLeftPlayer.transform.position = vPos;

        vPos = mRightPlayer.transform.position;
        vPos.y += Time.deltaTime * (mYDis/mRaisingTime);
        mRightPlayer.transform.position = vPos;

        Vector3 vRot = mLeftPlayer.transform.rotation.eulerAngles;
        vRot.y += 180f/mRaisingTime * Time.deltaTime;
        mLeftPlayer.transform.rotation = Quaternion.Euler(vRot);
        
        vRot = mRightPlayer.transform.rotation.eulerAngles;
        vRot.y += 180f/mRaisingTime * Time.deltaTime;
        mRightPlayer.transform.rotation = Quaternion.Euler(vRot);
    }

    // Camera goes from it's starting position and rotation to it's final spot in the cumulative time for raising and standing.
    private void TransitionCamera()
    {
        float timePercent = (Time.time - mStartTime) / (mRaisingTime + mUprightTime);
        if(timePercent > 1.0f) timePercent = 1.0f;
        Vector3 vRot = Vector3.Lerp(mRotCamStart, mRotCamFinish, timePercent);
        Vector3 vSpot = Vector3.Lerp(mCamStartPos, mCamFinishPos, timePercent);

        rCam.transform.position = vSpot;
        rCam.transform.rotation = Quaternion.Euler(vRot);
    }

}
