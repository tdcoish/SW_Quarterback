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

    [SerializeField]
    private GameObject          mFootballField;

    [SerializeField]
    private RT_Player           PF_Player;

    [SerializeField]
    private string              mDefaultPlay = "DefaultPlay.txt";

    [SerializeField]
    private InputField          mPlayNameEnter;

    [SerializeField]
    private Text                mSelectedPlayerInfo;

    public List<RT_Player>      rPlayers;

    // Load in play, then shove some RT_Player's into the football field
    void Awake()
    {
        // gonna try and use a streamreader 
        StreamReader sReader = new StreamReader(Application.dataPath+"/Plays/"+mDefaultPlay);
        
        string sLine = string.Empty;
        while((sLine = sReader.ReadLine()) != null){
            // use this line to set up our players.
            Vector3 posOnField = mFootballField.transform.position;
            string sSpot = UT_Strings.StartAndEndString(sLine, '[', ']');
            string sSpotX = UT_Strings.StartAndEndString(sSpot, '[', ',');
            sSpotX = UT_Strings.DeleteMultipleChars(sSpotX, "[,");
            string sSpotZ = UT_Strings.StartAndEndString(sSpot, ',', ']');
            sSpotZ = UT_Strings.DeleteMultipleChars(sSpotZ, ",]");

            // now we place the player on the screen according to the width and height.
            float unitsToPixel = 0.01f;     // unity default
            float fieldYardsToPixels = 10f;
            posOnField.x += float.Parse(sSpotX)*unitsToPixel * fieldYardsToPixels;
            posOnField.y += float.Parse(sSpotZ) * unitsToPixel * fieldYardsToPixels;

            RT_Player ply = Instantiate(PF_Player, posOnField, mFootballField.transform.rotation);
            ply.mTag = sLine.Substring(0, sLine.IndexOf(':'));
        }

    }

    void Start()
    {
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
            mSelectedPlayerInfo.text = "TYPE: " + rPlayers[actInd].mTag;
        }


        // cut this out, just for demo
        if(Input.GetKeyDown(KeyCode.U)){
            SceneManager.LoadScene("ThrowTesting");
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
