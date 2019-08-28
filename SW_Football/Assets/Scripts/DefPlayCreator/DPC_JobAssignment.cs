/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class DPC_JobAssignment : MonoBehaviour
{
    private DPC_Selector         cSelector;

    public Dropdown             DP_Tag;
    public Dropdown             DP_Role;
    public Dropdown             DP_Detail;

    public enum ASSIGNER_STATE
    {
        SNONE_SELECTED,
        SNEW_PLAYER_SELECTED,
        SDETAIL_VALUE_DUMMY,
        SDETAIL_VALUE_USEFUL
    }
    public ASSIGNER_STATE       mState;

    void Awake()
    {
        cSelector = GetComponentInParent<DPC_Selector>();

        mState = ASSIGNER_STATE.SNONE_SELECTED;
    }

    public void FRun_Update()
    {
        // switch the states.
        switch(mState)
        {
            case ASSIGNER_STATE.SNONE_SELECTED: RUN_Active(); break;
            case ASSIGNER_STATE.SNEW_PLAYER_SELECTED: RUN_Active(); break;
        }
    }

    private void RUN_Active()
    {

    }

    private void RUN_Inactive()
    {

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
        mState = ASSIGNER_STATE.SDETAIL_VALUE_DUMMY;
        DP_Detail.value = DP_Detail.options.Count;
    }

    public void OnDetailChanged()
    {
       if(!ErrorFree())
        {
            return;
        }

        if(mState == ASSIGNER_STATE.SDETAIL_VALUE_DUMMY)
        {
            mState = ASSIGNER_STATE.SDETAIL_VALUE_USEFUL;
            Debug.Log("Eating changed detail");
            return;
        }

        cSelector.rGuys[cSelector.mActivePlayer].GetComponent<PE_Role>().mDetails = DP_Detail.options[DP_Detail.value].text;

    }

    // For routes, display the routes, for blocking, display options. Whatever.
    private void SetDetailOptions()
    {
        DP_Detail.options.Clear();
        if(DP_Role.options[DP_Role.value].text == "Zone")
        {
            for(int i=0; i<IO_ZoneList.mZones.Length; i++)
            {
                DP_Detail.options.Add(new Dropdown.OptionData(IO_ZoneList.mZones[i].mName));
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
