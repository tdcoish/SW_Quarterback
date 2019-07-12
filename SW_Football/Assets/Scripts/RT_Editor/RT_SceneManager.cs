/*************************************************************************************
Runs when the editor starts. Shoves default play onto the screen
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RT_SceneManager : MonoBehaviour
{

    public GameObject           mFootballField;

    public RT_Player            PF_Player;

    [SerializeField]
    private string              mDefaultPlay = "DefaultPlay.txt";

    [SerializeField]
    public InputField           mPlayNameEnter;

    [SerializeField]
    private Text                mPositionText;

    [SerializeField]
    private Text                mSelectedPlayerInfo;

    public List<RT_Player>      rPlayers;

    private RT_PlayReader       cPlayReader;

    // Load in play, then shove some RT_Player's into the football field
    void Awake()
    {
        cPlayReader = GetComponent<RT_PlayReader>();
    }

    void Start()
    {
        cPlayReader.ReadInPlay(mDefaultPlay);

        rPlayers = new List<RT_Player>();

        var objs = FindObjectsOfType<RT_Player>();
        foreach(RT_Player ply in objs){
            rPlayers.Add(ply);
        }
    }

    private Vector3 GetStartPos(string line)
    {
        return Vector3.zero;
    }


    // One of the things we do, is update the Type input field with the correct RT_Player struct
    void Update()
    {
        int actInd = GetActivePlayerIndex();

        // When the user presses the mouse, we handle making things active or not.
        // also change things based on whether we are placing route nodes.
        if(Input.GetMouseButtonDown(0)){

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<RT_Player>() != null){
                    hit.collider.GetComponent<RT_Player>().mIsChosen = true;
                    if(actInd != -1) rPlayers[actInd].mIsChosen = false;
                }
            }
        }

        actInd = GetActivePlayerIndex();
        if(actInd == -1){
            mSelectedPlayerInfo.text = "NO PLAYER CHOSEN";
        }else{
            mPositionText.text = "X: " + rPlayers[actInd].transform.position.x*10f + ", Y: " + rPlayers[actInd].transform.position.y*10f;
            mSelectedPlayerInfo.text = "TYPE: " + rPlayers[actInd].mTag;
        }

    }

    public int GetActivePlayerIndex()
    {
        for(int i=0; i<rPlayers.Count; i++){
            if(rPlayers[i].mIsChosen){
                return i;
            }
        }

        return -1;
    }

}
