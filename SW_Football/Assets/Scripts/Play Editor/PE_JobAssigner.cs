/*************************************************************************************
Need a reference to the active player.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PE_JobAssigner : MonoBehaviour
{
    private PE_Editor           cEditor;
    private PE_Selector         cSelector;

    public Dropdown             DP_Tag;
    public Dropdown             DP_Role;
    public Dropdown             DP_Detail;

    void Awake()
    {
        cEditor = GetComponentInParent<PE_Editor>();
        cSelector = GetComponentInParent<PE_Selector>();

        Debug.Log("Assigner starting");
    }

    // When we select a player, set the values to be his values, tag, role, etcetera
    public void FSetDropdownValues()
    {
        if(!ErrorFree())
        {
            return;
        }

        PE_Role role = cSelector.rGuys[cSelector.mActivePlayer].GetComponent<PE_Role>();
        DP_Tag.captionText.text = role.mTag;
        DP_Role.captionText.text = role.mRole;
        DP_Detail.captionText.text = role.mDetails;
    }

    public void OnTagChanged()
    {
        if(!ErrorFree())
        {
            return;
        }

        // Alright, it's arcane but it makes sense.
        cSelector.rGuys[cSelector.mActivePlayer].GetComponent<PE_Role>().mTag = DP_Tag.options[DP_Tag.value].text;
    }

    public void OnRoleChanged()
    {
        if(!ErrorFree())
        {
            return;
        }

        cSelector.rGuys[cSelector.mActivePlayer].GetComponent<PE_Role>().mRole = DP_Role.options[DP_Role.value].text;
    }

    public void OnDetailChanged()
    {
       if(!ErrorFree())
        {
            return;
        }

        cSelector.rGuys[cSelector.mActivePlayer].GetComponent<PE_Role>().mDetails = DP_Detail.options[DP_Detail.value].text;
    }

    private bool ErrorFree()
    {
        if(!cSelector)
        {
            Debug.Log("For some reason no selector component");
            return false;
        }

        if(cSelector.mActivePlayer == -1)
        {
            Debug.Log("ERROR, no active player");
            return false;
        }

        if(cSelector.mActivePlayer > cSelector.rGuys.Length)
        {
            Debug.Log("ERROR, changing player after end of array");
            return false;
        }

        return true;
    }
}
