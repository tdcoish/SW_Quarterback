/*************************************************************************************
When the player clicks down, we want them to select a player, if they click on one.
De-select if they click the field.

Note, when the player loads in a new play, we need to get a reference to all those players.
*************************************************************************************/
using UnityEngine;

public class PE_Selector : MonoBehaviour
{
    public int                  mActivePlayer;

    public PE_Choosable[]            rGuys;             // terrible name for the spawned athlete objects.

    private PE_Editor               cEditor;

    void Start()
    {
        cEditor = GetComponentInParent<PE_Editor>();
    }


    void Update()
    {
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
                    mActivePlayer = GetActivePlayerIndex();
                    cEditor.OnPlayerSelected();
                }
                else 
                {
                    if(mActivePlayer != -1)
                    {
                        rGuys[mActivePlayer].mChosen = false;
                        cEditor.OnPlayerDeselected();
                    }
                    mActivePlayer = -1;
                }
            }

            mActivePlayer = GetActivePlayerIndex();
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

    private int GetActivePlayerIndex()
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
