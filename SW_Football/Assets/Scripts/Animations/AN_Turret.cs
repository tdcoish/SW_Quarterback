/*************************************************************************************
Right now all it needs to do is just play the shot fire.
*************************************************************************************/
using UnityEngine;

public class AN_Turret : MonoBehaviour
{
    private Animator                    animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FPlayFireAnim()
    {
        Debug.Log("Playin anim");
        // animator.Play("Fired");
        animator.SetTrigger("shotFire");
    }
}
