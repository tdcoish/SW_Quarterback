/*************************************************************************************
User interface of the play selection screen.

Interesting learning experience for Unity Raycast2D. Since I have set the plays to be in 
a canvas that is already set to screen space, doing the old 
Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
is actually wrong. Instead we want
Physics2D.Raycast(Input.mousePosition, Vector2.zero);

Because we're already in pixels.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PB_UI : MonoBehaviour
{
    public Dropdown                 DP_Plays;

    public Image[]                  mOffPlayImgs;
    public Image[]                  mDefPlayImgs;

    // Basically we're just testing for if they are hitting the offensive plays.
    public void FRunUpdate()
    {
        // You know, what if we just straight up didn't do any ScreenToWorldPoitn conversion?
        // LOL, it worked.
        if(Input.GetMouseButtonDown(0)){
            // first, we raycast to make sure we're over the field. Because we can't spawn a player randomly off the field.
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Color.green, 10f);

            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<PRAC_PlayArt>() != null){
                    string sName = hit.collider.GetComponent<PRAC_PlayArt>().mName;
                    Debug.Log("Picked: " + sName);

                    // Get rid of this line after a while
                    GetComponentInParent<PRAC_UI>().mOffensivePlayName.text = sName;

                    PRAC_Man pMan = FindObjectOfType<PRAC_Man>();
                    pMan.FPlayPicked(sName);
                }
            }else{
                Debug.Log("Hit nothing");
            }
        }
    }

    public void BT_PlaySelected()
    {
        // Do something when they select a play.
        Debug.Log("Play selected: " + DP_Plays.options[DP_Plays.value].text);
    }

    public void FSetUpPlaybookImagery()
    {
        for(int i = 0; i<mOffPlayImgs.Length; i++)
        {
            if(i+1 > IO_PlayList.mPlays.Length){
                break;
            }

            // We also want to put some text on them to show the play name.
            string path = "PlayArt/Offense/" + IO_PlayList.mPlays[i].mName;
            Debug.Log("Play: " + path);
            Sprite spr = Resources.Load<Sprite>(path);
            mOffPlayImgs[i].sprite = spr;
            mOffPlayImgs[i].GetComponent<PRAC_PlayArt>().mName = IO_PlayList.mPlays[i].mName;
            mOffPlayImgs[i].GetComponentInChildren<Text>().text = IO_PlayList.mPlays[i].mName;
        }
    }
}
