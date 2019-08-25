/*************************************************************************************
Need a reference to the active player.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PE_JobAssigner : MonoBehaviour
{
    private PE_Editor           cEditor;
    private PE_Selector         cSelector;
    private PE_RouteTool        cRouteTool;

    public Dropdown             DP_Tag;
    public Dropdown             DP_Role;
    public Dropdown             DP_Detail;

    void Awake()
    {
        cEditor = GetComponentInParent<PE_Editor>();
        cSelector = GetComponentInParent<PE_Selector>();
        cRouteTool = GetComponentInParent<PE_RouteTool>();

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
        // Gotta also change the detail menu.
        SetDetailOptions();
    }

    public void OnDetailChanged()
    {
       if(!ErrorFree())
        {
            return;
        }

        cSelector.rGuys[cSelector.mActivePlayer].GetComponent<PE_Role>().mDetails = DP_Detail.options[DP_Detail.value].text;

        // now if it's new, then make a new route?
        if(DP_Detail.options[DP_Detail.value].text == "New...")
        {
            Debug.Log("Make a new route");
            cRouteTool.BT_NewRoute();
        }
    }

    // For routes, display the routes, for blocking, display options. Whatever.
    private void SetDetailOptions()
    {

        DP_Detail.options.Clear();
        if(DP_Role.options[DP_Role.value].text == "Route")
        {
            DP_Detail.options.Add(new Dropdown.OptionData("New..."));
            for(int i=0; i<IO_RouteList.mRoutes.Length; i++)
            {
                DP_Detail.options.Add(new Dropdown.OptionData(IO_RouteList.mRoutes[i].mName));
            }

            // here's where I would have a binary file with all the options for routes, and a for loop through them.
            // DP_Detail.options.Add(new Dropdown.OptionData("New..."));
            // DP_Detail.options.Add(new Dropdown.OptionData("Curl"));
            // DP_Detail.options.Add(new Dropdown.OptionData("Hook"));
        }
        else
        {
            DP_Detail.options.Add(new Dropdown.OptionData("Standard"));
        }
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
