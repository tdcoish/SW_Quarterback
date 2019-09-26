/*************************************************************************************
Holy shit I can actually procedurally create and load in the plays I want. Holy shit.
I am a god. I have overdelivered.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PRAC_PB_UI : MonoBehaviour
{
    public Text                                 mOffensivePlayName;
    public Text                                 mTXTPage;
    public int                                  mPage = 0;

    public PRAC_PlayArt[]                       mPlayArts;

    // Basically we're just testing for if they are hitting the offensive plays.
    public virtual void FRunUpdate()
    {
        if(Input.GetMouseButtonDown(0)){
            // first, we raycast to make sure we're over the field. Because we can't spawn a player randomly off the field.
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<PRAC_PlayArt>() != null){
                    string sName = hit.collider.GetComponent<PRAC_PlayArt>().mName;
                    Debug.Log("Picked: " + sName);

                    PRAC_Off_Man pMan = FindObjectOfType<PRAC_Off_Man>();
                    pMan.FOffPlayPicked(sName);
                }
            }else{
                Debug.Log("Hit nothing");
            }
        }
    }

    // public virtual void FSetUpPlaybookImagery()
    // {
    //     string[] offPlayNames = IO_OffensivePlays.FReturnPlayNames();
    //     for(int i = 0; i<mPlayArts.Length; i++)
    //     {
    //         if(i+1 > offPlayNames.Length){
    //             break;
    //         }

    //         // We also want to put some text on them to show the play name.
    //         string path = "PlayArt/Offense/" + offPlayNames[i];
    //         Debug.Log("Play: " + path);
    //         Sprite spr = Resources.Load<Sprite>(path);
    //         mPlayArts[i].GetComponent<Image>().sprite = spr;
    //         mPlayArts[i].mName = offPlayNames[i];
    //         mPlayArts[i].GetComponentInChildren<Text>().text = offPlayNames[i];
    //     }
    // }

    public virtual void FSetUpPlaybookImagery()
    {
        int ind = mPlayArts.Length * mPage;

        string[] offPlayNames = IO_OffensivePlays.FReturnPlayNames();
        // If we have 14 plays, we should have 3 pages. Can't show a fourth page, obviously.
        if(ind > offPlayNames.Length)
        {
            Debug.Log("Past the end of the playbook");
            mPage = 0;
        }

        for(int i = ind; i<mPlayArts.Length + ind; i++)
        {
            // Here we put in a blank sprite if there's no play to go into there.
            if(i+1 > offPlayNames.Length){
                Debug.Log("Can't show past the end of the PB");
                break;
            }

            // We also want to put some text on them to show the play name.
            string path = "PlayArt/Offense/" + offPlayNames[i];
            Debug.Log("Play: " + path);

            int el = i - (mPlayArts.Length * mPage);
            Texture2D tex = Resources.Load<Texture2D>(path);
            Rect r = new Rect(0, 0, 256, 256);
            Sprite spr = Sprite.Create(tex, r, new Vector2(1f, 1f));
            mPlayArts[el].GetComponent<Image>().sprite = spr;
            mPlayArts[el].mName = offPlayNames[i];
            mPlayArts[el].GetComponentInChildren<Text>().text = offPlayNames[i];
        }
    }

    public void BT_NextPage()
    {
        mPage++;
        HandlePageTurned();
    }

    public void BT_PrevPage()
    {
        mPage--;
        HandlePageTurned();
    }

    private void HandlePageTurned()
    {
        string[] offPlayNames = IO_OffensivePlays.FReturnPlayNames();
        int numPages = offPlayNames.Length / mPlayArts.Length;
        if(offPlayNames.Length % mPlayArts.Length > 0){
            numPages++;
        }
        if(mPage < 0){
            // calc the maximum number of pages we can have, then minus one 
            mPage = numPages-1;
        }else if(mPage > numPages - 1)
        {
            mPage = 0;
        }


        mTXTPage.text = ("Page: " + (mPage+1) + "/"+numPages);

        FSetUpPlaybookImagery();
    }
}
