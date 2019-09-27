/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class AN_OffPlayer : MonoBehaviour
{

    private PRAC_Off_Ply                    cPlayer;
    private Animator                        cAnimator;

    void Start()
    {
        cPlayer = GetComponent<PRAC_Off_Ply>(); 
        cAnimator = GetComponentInChildren<Animator>();   
    }

    // Switch based on the state the player is in?
    void Update()
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
        Debug.Log("Setting all bools to false");
        cAnimator.SetBool("isActive", false);
        cAnimator.SetBool("isRunning", false);
        cAnimator.SetBool("isPreSnap", false);
        cAnimator.SetBool("isStrafing", false);
        cAnimator.SetBool("isDoneRoute", false);
    }
}
