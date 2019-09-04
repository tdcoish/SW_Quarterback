/*************************************************************************************
Okay just copy the existing one instead, because inheritance is derping up.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PB_Def : MonoBehaviour
{
    public Text                                 mTXTPage;

    public int                                  mPage = 0;

    // This is now a little misleading. This is more like the slot, rather than the play art itself.
    public PRAC_PlayArt[]                       mPlayArts;

    void Start()
    {
        mPlayArts = GetComponentsInChildren<PRAC_PlayArt>();
    }

    // Basically we're just testing for if they are hitting the offensive plays.
    public virtual void FRunUpdate()
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
                    GetComponentInParent<PRAC_UI>().mDefensivePlayName.text = sName;

                    PRAC_Man pMan = FindObjectOfType<PRAC_Man>();
                    pMan.FPlayPicked(sName);
                }
            }else{
                Debug.Log("Hit nothing");
            }
        }
    }

    public virtual void FSetUpPlaybookImagery()
    {
        int ind = mPlayArts.Length * mPage;

        // If we have 14 plays, we should have 3 pages. Can't show a fourth page, obviously.
        if(ind > IO_DefPlays.mPlays.Length)
        {
            Debug.Log("Past the end of the playbook");
            mPage = 0;
        }

        for(int i = ind; i<mPlayArts.Length + ind; i++)
        {
            // Here we put in a blank sprite if there's no play to go into there.
            if(i+1 > IO_DefPlays.mPlays.Length){
                Debug.Log("Can't show past the end of the PB");
                break;
            }

            // We also want to put some text on them to show the play name.
            string path = "PlayArt/Defense/" + IO_DefPlays.mPlays[i].mName;
            Debug.Log("Play: " + path);

            int el = i - (mPlayArts.Length * mPage);
            Sprite spr = Resources.Load<Sprite>(path);
            mPlayArts[el].GetComponent<Image>().sprite = spr;
            mPlayArts[el].mName = IO_DefPlays.mPlays[i].mName;
            mPlayArts[el].GetComponentInChildren<Text>().text = IO_DefPlays.mPlays[i].mName;
        }
    }

    public void BT_NextPage()
    {
        mPage++;
        if(mPage * mPlayArts.Length > IO_DefPlays.mPlays.Length)
        {
            mPage = 0;
        }

        int numPages = IO_DefPlays.mPlays.Length / mPlayArts.Length;
        mTXTPage.text = ("Page: " + mPage + "/"+numPages);

        FSetUpPlaybookImagery();
    }
}
