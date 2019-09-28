/*************************************************************************************
An important note for animations. It's very important to disable the "can transition to self"
option in the animator tab. I don't know why that's enabled by default. However, it means that 
we keep transitioning to the same state over and over instead of just staying in it. Frankly,
I don't know why we would ever want that. I guess maybe for triggers like if some enemy 
has a "get shot" reaction.
*************************************************************************************/
using UnityEngine;

public class AN_OffPlayer : MonoBehaviour
{

    private PRAC_Off_Ply                    cAthlete;
    private Animator                        cAnimator;

    void Start()
    {
        cAthlete = GetComponent<PRAC_Off_Ply>(); 
        cAnimator = GetComponentInChildren<Animator>();   
    }

    // Switch based on the state the player is in?
    // Might have to just go in bool by bool.
    void Update()
    {
        SetAllBoolsToFalse();

        switch(cAthlete.mState)
        {
            case PRAC_Ath.PRAC_ATH_STATE.SPRE_SNAP: AnimatePreSnap(); break;
            case PRAC_Ath.PRAC_ATH_STATE.SDOING_JOB: AnimateDoingJob(); break;
            case PRAC_Ath.PRAC_ATH_STATE.SPOST_PLAY: AnimatePostPlay(); break;      // alright do something else.
        }

    }

    private void AnimatePreSnap()
    {
        cAnimator.SetBool("isPreSnap", true);
    }

    private void AnimateDoingJob()
    {
        switch(cAthlete.mRole)
        {
            case "ROUTE": if(cAthlete.GetComponent<OFF_RouteLog>().mState == OFF_RouteLog.STATE.S_BLIND){
                                cAnimator.SetBool("isRunning", true);
                            }else if(cAthlete.GetComponent<OFF_RouteLog>().mState == OFF_RouteLog.STATE.S_GET_OPEN){
                                cAnimator.SetBool("isDoneRoute", true);
                            } else if(cAthlete.GetComponent<OFF_RouteLog>().mState == OFF_RouteLog.STATE.S_LOOKING_FOR_BALL){
                                cAnimator.SetBool("isLookingForBall", true);
                            }
                            break;
            case "BLOCK": cAnimator.SetBool("isStrafing", true); break;
        }

    }

    private void AnimatePostPlay()
    {
        cAnimator.SetBool("isStrafing", true);
    }

    // Just for testing animations.
    void DoManualKeyTesting()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            SetAllBoolsToFalse();
            cAnimator.SetBool("isStrafing", true);
        }
        if(Input.GetKeyDown(KeyCode.O)){
            SetAllBoolsToFalse();
            cAnimator.SetBool("isPreSnap", true);
        }
        if(Input.GetKeyDown(KeyCode.U)){
            SetAllBoolsToFalse();
            cAnimator.SetBool("isRunning", true);
        }
    }

    void SetAllBoolsToFalse()
    {
        cAnimator.SetBool("isActive", false);
        cAnimator.SetBool("isRunning", false);
        cAnimator.SetBool("isPreSnap", false);
        cAnimator.SetBool("isStrafing", false);
        cAnimator.SetBool("isDoneRoute", false);
        cAnimator.SetBool("isLookingForBall", false);
    }
}
