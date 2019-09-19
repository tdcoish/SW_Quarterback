/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ED_FM_Man : MonoBehaviour
{
    
    public enum STATE{
        S_BEGIN,
        S_NoneSelected,
        S_Selected,
        S_END
    }
    public STATE                                    mState;

    public Image                                    PF_Player;
    public InputField                               mNameField;
    public Text                                     mCurSelectedTag;
    public Text                                     mNewTag;

    public ED_FM_Grid                               rGrid;
    public Vector2                                  mSnapSpot;

    public List<ED_FM_Ply>                          mAths;
    public int                                      mActive = -1;
    public List<string>                             mValidTags;
    public List<string>                             mTagsAvailable;
    private int                                     mTagInd = 0;

    public Canvas                                   rSelected;
    public Canvas                                   rUnselected;

    void Start()
    {
        mSnapSpot.x = rGrid.mAxisLength / 2;
        mSnapSpot.y = rGrid.mAxisLength - 5;

        mAths = new List<ED_FM_Ply>();
        mValidTags = new List<string>();
        mTagsAvailable = new List<string>();
        PopulateValidTagsList();
    }

    void Update()
    {
        switch(mState)
        {
            case STATE.S_BEGIN: RUN_BEGIN(); break;
            case STATE.S_NoneSelected: RUN_NONE_SELECTED(); break;
            case STATE.S_Selected: RUN_SELECTED(); break;
        }
    }

    // Here we have to load in the default, and put the default there.
    private void RUN_BEGIN()
    {
        DATA_Formation f = IO_Formations.FLOAD_FORMATION("Default");

        Debug.Log("Num Players: " + f.mSpots.Length);
        for(int i=0; i<f.mSpots.Length; i++)
        {
            int x = (int)mSnapSpot.x;
            // int y = (int)mSnapSpot.y;
            int y = 0;
            x += (int)(f.mSpots[i].x / 2);
            y += (int)f.mSpots[i].y / 2;
            Vector3 vPos = rGrid.FGetPos(x, y);
            var clone = Instantiate(PF_Player, vPos, transform.rotation);
            clone.rectTransform.SetParent(rGrid.transform);

            ED_FM_Ply p = clone.GetComponent<ED_FM_Ply>();
            p.mTag = f.mTags[i];
            p.x = x;
            p.y = y;
            mAths.Add(p);
        }

        ENTER_UNSELECTED();
    }

    private void ENTER_SELECTED()
    {
        mState = STATE.S_Selected;
        rSelected.gameObject.SetActive(true);
    }
    private void EXIT_UNSELECTED()
    {
        rUnselected.gameObject.SetActive(false);
    }
    private void ENTER_UNSELECTED()
    {
        mState = STATE.S_NoneSelected;
        rUnselected.gameObject.SetActive(true);
    }
    private void EXIT_SELECTED()
    {
        mActive = -1;
        rSelected.gameObject.SetActive(false);
    }

    private void RUN_NONE_SELECTED()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
            
            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<ED_FM_Ply>() != null)
                {
                    Debug.Log("Hit grid square");
                    Debug.Log(hit.transform.position);
                    ED_FM_Ply p = hit.collider.GetComponent<ED_FM_Ply>();
                    for(int i=0; i<mAths.Count; i++){
                        if(mAths[i].mTag == p.mTag){
                            mActive = i;
                            EXIT_UNSELECTED();
                            ENTER_SELECTED();
                            break;
                        }
                    }
                }
            }

        }
    }

    private void RUN_SELECTED()
    {
        
        mCurSelectedTag.text = mAths[mActive].mTag;
        if(Input.GetMouseButtonDown(1)){
            EXIT_SELECTED();
            ENTER_UNSELECTED();
        }
    }

    // Pass in how far to the right, and up you want to move them.
    private void RepositionPlayer(ED_FM_Ply p, int x, int y)
    {
        p.x += x;
        if(p.x > rGrid.mAxisLength - 1){
            p.x = rGrid.mAxisLength - 1;
        }
        if(p.x < 0){
            p.x = 0;
        }
        p.y -= y;
        if(p.y > rGrid.mAxisLength - 1){
            p.y = rGrid.mAxisLength - 1;
        }
        if(p.y < 0){
            p.y = 0;
        }
        Vector3 v = rGrid.FGetPos(p.x, p.y);
        p.transform.position = v;
    }

    public void BT_LeftHard()
    {
        RepositionPlayer(mAths[mActive], -5, 0);
    }
    public void BT_Left()
    {
        RepositionPlayer(mAths[mActive], -1, 0);
    }
    public void BT_UpHard()
    {
        RepositionPlayer(mAths[mActive], 0, 5);
    }
    public void BT_Up()
    {
        RepositionPlayer(mAths[mActive], 0, 1);
    }
    public void BT_RightHard()
    {
        RepositionPlayer(mAths[mActive], 5, 0);
    }
    public void BT_Right()
    {
        RepositionPlayer(mAths[mActive], 1, 0);
    }
    public void BT_DownHard()
    {
        RepositionPlayer(mAths[mActive], 0, -5);
    }
    public void BT_Down()
    {
        RepositionPlayer(mAths[mActive], 0, -1);
    }

    public void BT_SaveFormation()
    {
        DATA_Formation f = new DATA_Formation();
        f.mName = mNameField.text;
        ED_FM_Ply[] players = FindObjectsOfType<ED_FM_Ply>();
        f.mSpots = new Vector2[players.Length];
        f.mTags = new string[players.Length];
        for(int i=0; i<players.Length; i++){
            Vector2 v = new Vector2();
            v.y = players[i].y * 2;
            v.x = (players[i].x - rGrid.mAxisLength/2) * 2;
            f.mSpots[i] = v;
            f.mTags[i] = players[i].mTag;
        }

        IO_Formations.FWRITE_FORMATION(f);
    }

    public void BT_TagSet()
    {
        if(mState != STATE.S_Selected){
            Debug.Log("Wrong state to set tag");
            return;
        }
        mAths[mActive].mTag = mNewTag.text;
    }

    public void BT_TagNext()
    {
        mTagInd++;
        HandleNextOrPreviousTag();
    }
    public void BT_TagPrev()
    {
        mTagInd--;
        HandleNextOrPreviousTag();
    }

    private void HandleNextOrPreviousTag()
    {
        mTagsAvailable = FindAvailableTags();
        Debug.Log("Num Available: " + mTagsAvailable.Count);
        if(mTagInd > mTagsAvailable.Count - 1){
            mTagInd = 0;
        }
        if(mTagInd < 0){
            mTagInd = mTagsAvailable.Count - 1;
        }

        mNewTag.text = mTagsAvailable[mTagInd];
    }

    private List<string> FindAvailableTags()
    {
        List<string> tags = new List<string>();
        foreach(string t in mValidTags){
            tags.Add(t);
        }
        Debug.Log("num tags");
        
        foreach(ED_FM_Ply p in mAths){
            tags.Remove(p.mTag);
        }

        return tags;
    }
    private void PopulateValidTagsList()
    {
        mValidTags.Add("QB");
        mValidTags.Add("WR1");
        mValidTags.Add("WR2");
        mValidTags.Add("WR3");
        mValidTags.Add("WR4");
        mValidTags.Add("WR5");
        mValidTags.Add("TE1");
        mValidTags.Add("TE2");
        mValidTags.Add("TE3");
        mValidTags.Add("RB1");
        mValidTags.Add("RB2");
        mValidTags.Add("RB3");
        mValidTags.Add("OL1");
        mValidTags.Add("OL2");
        mValidTags.Add("OL3");
        mValidTags.Add("OL4");
        mValidTags.Add("OL5");
    }
}
