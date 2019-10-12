/*************************************************************************************
Could probably be used for offensive and defensive players.
*************************************************************************************/
using UnityEngine;

public class RP_CatchLog : MonoBehaviour
{
    public GameObject                   PF_SpotGFX;

    public Vector3 FCalcInterceptSpot(float fHeightOfInfluence = 1f)
    {
        PROJ_Football fBallRef = FindObjectOfType<PROJ_Football>();
        if(fBallRef == null){
            // Debug.Log("No football in scene. Intercept will have issues.");
            return Vector3.zero;
        }

        Vector3 vSpotToMoveTo = new Vector3();

        Vector3 vBallVel = fBallRef.GetComponent<Rigidbody>().velocity;
        float mag = Vector3.Magnitude(vBallVel);

        // ----------------------------------- First if statement is if the ball is already below our hands, and is going down, or it's right there already.
        if(fBallRef.transform.position.y < fHeightOfInfluence)
        {
            if(vBallVel.y <= 0f || Vector3.Distance(transform.position, fBallRef.transform.position) < 1f){
                return fBallRef.transform.position;
            }
        }
        else
        {
            // ----------- Here we have to calculate the spot, since the ball is unfortunately not going down already and is not close to us.
            float yDisToGo = fBallRef.transform.position.y - fHeightOfInfluence;
            float fFinalVel = Mathf.Sqrt(Mathf.Abs(vBallVel.y*vBallVel.y + 2*Physics.gravity.magnitude * yDisToGo)) * -1f;
            float fTm = Mathf.Abs((fFinalVel - vBallVel.y))/Physics.gravity.magnitude;

            Vector3 vFwdComp = vBallVel;
            vFwdComp.y = 0f;

            vSpotToMoveTo = fBallRef.transform.position + vFwdComp*fTm;
            vSpotToMoveTo.y = 0f;

            Instantiate(PF_SpotGFX, vSpotToMoveTo, transform.rotation);
            return vSpotToMoveTo;
        }

        return Vector3.zero;
    }

    // Now we calculate how long it's going to take to get to that spot.
    public float FCalcInterceptTime(float fHeightOfInfluence = 1f)
    {
        PROJ_Football fBallRef = FindObjectOfType<PROJ_Football>();
        if(fBallRef == null){
            // Debug.Log("No football in scene. Time is infinite.");
            return 0f;
        }
        
        Vector3 vBallVel = fBallRef.GetComponent<Rigidbody>().velocity;
        float mag = Vector3.Magnitude(vBallVel);

        // ----------------------------------- First if statement is if the ball is already below our hands, and is going down, or it's right there already.
        if(fBallRef.transform.position.y < fHeightOfInfluence)
        {
            return 0f;
        }
        else
        {
            // ----------- Here we have to calculate the spot, since the ball is unfortunately not going down already and is not close to us.
            float yDisToGo = fBallRef.transform.position.y - fHeightOfInfluence;
            float fFinalVel = Mathf.Sqrt(Mathf.Abs(vBallVel.y*vBallVel.y + 2*Physics.gravity.magnitude * yDisToGo)) * -1f;
            float fTm = Mathf.Abs((fFinalVel - vBallVel.y))/Physics.gravity.magnitude;

            return fTm;
        }
    }
}
