/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PROFST_St : MonoBehaviour
{
    protected PRAC_Off_Man                      cMan;

    protected PROFST_Live                       cLive;
    protected PROFST_Pick                       cPick;
    protected PROFST_Pre                        cPre;
    protected PROFST_Post                       cPost;

    public virtual void Start()
    {
        cMan = GetComponent<PRAC_Off_Man>();    

        cPick = GetComponent<PROFST_Pick>();
        cPost = GetComponent<PROFST_Post>();
        cLive = GetComponent<PROFST_Live>();
        cPre = GetComponent<PROFST_Pre>();
    }

    public virtual void FEnter(){}
    public virtual void FExit(){}
    public virtual void FRun(){}
}
