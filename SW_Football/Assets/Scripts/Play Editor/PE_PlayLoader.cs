/*************************************************************************************
Loads in, and displays the play.

Having a problem finding the correct spot in units that the player should be instantiated at, that
makes them have the correct field position. So for now I'm just sort of getting a rough guide.

Lol I'm an idiot. Just divide the number of pixels by the units of the field. Since I made a field
that is 50 meters (I pretend), and the field image is 500x500, then I can just divide by 10.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PE_PlayLoader : MonoBehaviour
{
    private PE_Selector         cSelector;

    //public GameObject           rSnapSpot;
    public PE_Field             rField;

    public PE_Role              PF_PlayerRole;

    void Start()
    {
        cSelector = GetComponent<PE_Selector>();
    }

    // We also have to destroy the roles currently in the scene.
    public void FLoadPlay(DATA_Play play)
    {
        PE_Role[] roles = FindObjectsOfType<PE_Role>();
        for(int i=0; i<roles.Length; i++)
        {
            Destroy(roles[i].gameObject);
        }

        Debug.Log("Play: " + play.mName);
        for(int i=0; i<play.mPlayerRoles.Length; i++)
        {            
            PE_Role role = Instantiate(PF_PlayerRole, transform.position, transform.rotation);
            role.mTag = play.mPlayerRoles[i].mTag;
            role.mRole = play.mPlayerRoles[i].mRole;     // just a string now.
            role.mDetails = play.mPlayerRoles[i].mDetail;
            role.mStartPos.x = play.mPlayerRoles[i].mStart.x;
            role.mStartPos.y = play.mPlayerRoles[i].mStart.y;

            // Here I'm making sure that the players always start on whole numbers, even if the play is not saved that way.
            role.mStartPos.x = (float)System.Math.Round((double)role.mStartPos.x, 0);
            role.mStartPos.y = (float)System.Math.Round((double)role.mStartPos.y, 0);

            // okay, now actually make it spawn in the right spot.
            // update. I guess we need to work in pixels.
            float fMetersToPixels = 50f / rField.GetComponent<RectTransform>().rect.width;
            Vector3 vPos = new Vector3();
            vPos.x = rField.transform.position.x + role.mStartPos.x * fMetersToPixels;
            vPos.y = rField.transform.position.y + role.mStartPos.y * fMetersToPixels;
            // set the y to the 10 yard line, normalized. That is where I decided the play starts from.
            vPos.y -= 15f * fMetersToPixels;
            role.transform.position = vPos;
        }

        // Since the old ones are garbage now.
        cSelector.FGetNewReferences();
    }
}
