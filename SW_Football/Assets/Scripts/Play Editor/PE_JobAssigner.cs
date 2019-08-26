/*************************************************************************************
Need a reference to the active player.

Boy, DropDown sucks balls. Here's a copy-paste from a google doc I was working in.

I’ve finally figured out the source of these bugs. Dropdown only has a way of telling if the value has changed. 
What this means is that you could never assign the same route to two different receivers without going the Rec2, 
switching to a random route, then switching back. This is because the stupid thing wouldn’t do anything because 
when you click the same route you’re not “changing” the value. 
A temporary solution is just to shove some garbage values in there, like NA, or whatever, 
switch to that each time, and then go from there.

OMFG, this is comedy. Unity is so fucking incompetent, that if you manually change the value,
they just go ahead and call OnValueChanged for you, making you really fuck up. Jesus Christ,
the level of asshatery here.

I just can't. Fuck Unity. This Dropdown shit is awful.
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
        OnRoleChanged();
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

        // now we set the role to NOTSET, the last one.
        DP_Role.value = DP_Role.options.Count;
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

        if(DP_Role.options[DP_Role.value].text == "Route")
        {
            // now if it's new, then make a new route?
            if(DP_Detail.options[DP_Detail.value].text == "New...")
            {
                Debug.Log("Make a new route");
                cRouteTool.BT_NewRoute();
            }
            else 
            {
                cEditor.GetComponent<PE_DisplayPlayJobs>().FDisplayJobs();
            }

        }
        else
        {
            cEditor.GetComponent<PE_DisplayPlayJobs>().FDisplayJobs();
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
        }
        else
        {
            DP_Detail.options.Add(new Dropdown.OptionData("Standard"));
        }

        // We always add, NOTSET, to make the stupid dropdown menu work without dumbass workarounds
        // Even though this IS a dumbass workaround. #FUCK UNITY.
        DP_Detail.options.Add(new Dropdown.OptionData("NOTSET"));
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
