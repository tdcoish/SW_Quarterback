/*************************************************************************************
In keeping with my new pattern, I'm having this guy being called by an owner, depending 
on the appropriate state.
*************************************************************************************/
using UnityEngine;

public class DPC_Selector : MonoBehaviour
{
    public int                      mActivePlayer;

    public PE_Choosable[]           rGuys;             // terrible name for the spawned athlete objects.

    void Start()
    {
        mActivePlayer = -1;
    }

    public void FRun_Update()
    {
        rGuys = FindObjectsOfType<PE_Choosable>();

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<PE_Choosable>() != null){
                    hit.collider.GetComponent<PE_Choosable>().mChosen = true;
                    if(mActivePlayer != -1)
                    {
                        rGuys[mActivePlayer].mChosen = false;
                    }
                    mActivePlayer = FGetActivePlayerIndex();
                }
                else 
                {
                    if(mActivePlayer != -1)
                    {
                        rGuys[mActivePlayer].mChosen = false;
                    }
                    mActivePlayer = -1;
                }
            }

            mActivePlayer = FGetActivePlayerIndex();
        }

        SetChoosablesBars();
    }

    // So only the truly active one is 
    private void SetChoosablesBars()
    {
        for(int i=0; i<rGuys.Length; i++)
        {
            if(i != mActivePlayer)
                rGuys[i].mBar.SetActive(false);
            else
                rGuys[i].mBar.SetActive(true);
        }

    }

    public int FGetActivePlayerIndex()
    {
        for(int i=0; i<rGuys.Length; i++){
            if(rGuys[i].mChosen){
                return i;
            }
        }

        return -1;
    }

    // Call this whenever we load a new play, or whatever.
    public void FGetNewReferences()
    {
        mActivePlayer = -1;

        rGuys = FindObjectsOfType<PE_Choosable>();
    }
}
