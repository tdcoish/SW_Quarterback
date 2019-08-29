/*************************************************************************************
For now we just splat 11 players on the field when they hit new player.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class DPC_NewPlay : MonoBehaviour
{

    public GameObject                       rSnapSpot;

    public PE_Role                          PF_Defender;

    public InputField                       rPlayInputText;

    // Just shove 11 players on the field side by side.
    public void BT_NewPlay()
    {
        // make them start 2 yards apart.
        Vector2 vPos = rSnapSpot.transform.position;
        vPos.x -= 11f * 0.1f;
        for(int i=0; i<11; i++)
        {
            var clone = Instantiate(PF_Defender, vPos, transform.rotation);
            clone.mStartPos.x = (vPos.x * 10f) - rSnapSpot.transform.position.x;
            clone.mTag = "NA";
            clone.mRole = "NA";
            clone.mDetails = "NA";
            vPos.x += 2f * 0.1f;
        }
    }    

    public void BT_SavePlay()
    {
        if(rPlayInputText.text == string.Empty)
        {
            Debug.Log("Please enter a play name");
            return;
        }

        // gather up all the defenders in the scene.
        PE_Role[] roles = FindObjectsOfType<PE_Role>();
        DATA_Play ply = new DATA_Play();
        ply.mName = rPlayInputText.text;
        ply.mPlayerRoles = new DT_PlayerRole[roles.Length];     // Should always be 11.
        Debug.Log("Should be 11: " + ply.mPlayerRoles.Length);
        for(int i=0; i<ply.mPlayerRoles.Length; i++)
        {
            DT_PlayerRole dtRole = new DT_PlayerRole();
            dtRole.mTag = roles[i].mTag;
            dtRole.mRole = roles[i].mRole;
            dtRole.mDetail = roles[i].mDetails;
            dtRole.mStart = roles[i].mStartPos;
            ply.mPlayerRoles[i] = dtRole;
        }
        IO_DefPlays.FWRITE_PLAY(ply);
    }
}
