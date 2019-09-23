/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    public ED_OP_Mark                               PF_Marker;
    public InputField                               mNameField;
    public Text                                     mCurSelectedTag;
    public Text                                     mNewTag;
    public Text                                     mLineOfScrim;
    public Button                                   mSaveBtn;
    public UI_SavedTxt                              mSavedText;
    public Dropdown                                 mDPFormations;

    public ED_FM_Grid                               rGrid;
    public Vector2                                  mSnapSpot;

    public List<ED_FM_Ply>                          mAths;
    public int                                      ixPly = -1;
    public List<string>                             mValidTags;
    public List<string>                             mTagsAvailable;
    private int                                     mTagInd = 0;

    public Canvas                                   rSelected;
    public GameObject                               rUITagging;
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
        ClearGridOfPlayers();
        LoadAndDisplayFormation("Default");
        PopulateDropdownList();

        ENTER_UNSELECTED();
    }

    private void PopulateDropdownList(){
        //------------------- Populate the dropdown list.
        mDPFormations.options = new List<Dropdown.OptionData>();
        string[] sFormationNames = IO_Formations.FReturnFormationNames();
        foreach(string s in sFormationNames){
            mDPFormations.options.Add(new Dropdown.OptionData(s));
        }
    }
    private void ClearGridOfPlayers()
    {
        ED_FM_Ply[] players = FindObjectsOfType<ED_FM_Ply>();
        foreach(ED_FM_Ply p in players){
            Destroy(p.gameObject);
        }

        mAths.Clear();
    }
    private void LoadAndDisplayFormation(string name){
        DATA_Formation f = IO_Formations.FLOAD_FORMATION(name);

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
    }

    private void ENTER_SELECTED()
    {
        mState = STATE.S_Selected;
        rSelected.gameObject.SetActive(true);
        Vector3 vPos = mAths[ixPly].transform.position;
        var clone = Instantiate(PF_Marker, vPos, transform.rotation);
        clone.GetComponent<Image>().rectTransform.SetParent(mAths[ixPly].transform);
        mLineOfScrim.gameObject.SetActive(true);
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
        ixPly = -1;
        rSelected.gameObject.SetActive(false);
        ED_OP_Mark[] marks = FindObjectsOfType<ED_OP_Mark>();
        foreach(ED_OP_Mark m in marks){
            Destroy(m.gameObject); 
        }
        mLineOfScrim.gameObject.SetActive(false);
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
                            ixPly = i;
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
        // --------------------- Line of Scrimmage
        int numOnLOS = 0;
        foreach(ED_FM_Ply p in mAths){
            if(p.y == 0){
                numOnLOS++;
            }
        }
        if(numOnLOS < 7){
            mLineOfScrim.gameObject.SetActive(true);
            mSaveBtn.gameObject.SetActive(false);
        }else{
            mLineOfScrim.gameObject.SetActive(false);
            mSaveBtn.gameObject.SetActive(true);
        }

        // ------------------------------ Tagging, not for OL and QB
        if(mAths[ixPly].mTag.Contains("OL") || mAths[ixPly].mTag.Contains("QB")){
            rUITagging.SetActive(false);
        }else{
            rUITagging.SetActive(true);
        }
        
        // -------------------------------

        mCurSelectedTag.text = mAths[ixPly].mTag;
        if(Input.GetMouseButtonDown(1)){
            EXIT_SELECTED();
            ENTER_UNSELECTED();
        }
    }

    // Pass in how far to the right, and up you want to move them.
    private void RepositionPlayer(ED_FM_Ply p, int x, int y)
    {
        int newX = p.x + x;
        int newY = p.y - y;

        foreach(ED_FM_Ply ply in mAths){
            if(ply.x == newX && ply.y == newY){
                Debug.Log("There's already a player on that spot");
                return;
            }
        }

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
        RepositionPlayer(mAths[ixPly], -5, 0);
    }
    public void BT_Left()
    {
        RepositionPlayer(mAths[ixPly], -1, 0);
    }
    public void BT_UpHard()
    {
        RepositionPlayer(mAths[ixPly], 0, 5);
    }
    public void BT_Up()
    {
        RepositionPlayer(mAths[ixPly], 0, 1);
    }
    public void BT_RightHard()
    {
        RepositionPlayer(mAths[ixPly], 5, 0);
    }
    public void BT_Right()
    {
        RepositionPlayer(mAths[ixPly], 1, 0);
    }
    public void BT_DownHard()
    {
        RepositionPlayer(mAths[ixPly], 0, -5);
    }
    public void BT_Down()
    {
        RepositionPlayer(mAths[ixPly], 0, -1);
    }

    public void BT_SaveFormation()
    {
        if(mNameField.text == ""){
            Debug.Log("ERROR. No Name");
            return;
        }
        // Formations are saved using the personnel data. RB->TE. So 2WR, 1RB, 2TE becomes 12
        DATA_Formation f = new DATA_Formation();
        int numRB = 0;
        int numTE = 0;
        foreach(ED_FM_Ply p in mAths){
            if(p.mTag.Contains("TE")){
                numTE++;
            }else if(p.mTag.Contains("RB")){
                numRB++;
            }
        }
        f.mName = numRB+"-"+numTE+"_"+mNameField.text;
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
        mSavedText.FSetVisible();

        PopulateDropdownList();
    }

    public void BT_TagSet()
    {
        if(mState != STATE.S_Selected){
            Debug.Log("Wrong state to set tag");
            return;
        }
        mAths[ixPly].mTag = mNewTag.text;
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

        // Here I want to remove the "extra" tags. Eg. no WR4 when WR3 is available.
        int wrNum = 10;
        int rbNum = 10;
        int teNum = 10;
        foreach(string t in tags){
            if(t.Contains("WR")){
                int n = int.Parse(t.Substring(2));
                if(n < wrNum){
                    wrNum = n;
                }
            }
            if(t.Contains("TE")){
                int n = int.Parse(t.Substring(2));
                if(n < teNum){
                    teNum = n;
                }
            }
            if(t.Contains("RB")){
                int n = int.Parse(t.Substring(2));
                if(n < rbNum){
                    rbNum = n;
                }
            }
        }

        for(int i=0; i<tags.Count; i++){
            if(tags[i].Contains("WR")){
                int n = int.Parse(tags[i].Substring(2));
                if(n > wrNum){
                    tags.RemoveAt(i);
                    i--;
                    continue;
                }
            }
            if(tags[i].Contains("TE")){
                int n = int.Parse(tags[i].Substring(2));
                if(n > teNum){
                    tags.RemoveAt(i);
                    i--;
                    continue;
                }
            }
            if(tags[i].Contains("RB")){
                int n = int.Parse(tags[i].Substring(2));
                if(n > rbNum){
                    tags.RemoveAt(i);
                    i--;
                    continue;
                }
            }
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

    public void BT_MainMenu(){
        SceneManager.LoadScene("SN_MN_Main");        
    }

    public void DP_ChangeFormation(){
        ClearGridOfPlayers();
        LoadAndDisplayFormation(mDPFormations.options[mDPFormations.value].text);
    }
}
