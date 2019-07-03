/*************************************************************************************
Runs when the editor starts. Shoves default play onto the screen
*************************************************************************************/
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class RT_SceneManager : MonoBehaviour
{

    [SerializeField]
    private GameObject          mFootballField;

    [SerializeField]
    private RT_Player           RefPlayer;

    [SerializeField]
    private string              mDefaultPlay = "DefaultPlay.txt";

    // Load in play, then shove some RT_Player's into the football field
    void Start()
    {
        // gonna try and use a streamreader 
        StreamReader sReader = new StreamReader(Application.dataPath+"/Plays/"+mDefaultPlay);
        
        string sLine = string.Empty;
        while((sLine = sReader.ReadLine()) != null){
            Debug.Log("Line: " + sLine);
            // use this line to set up our players.
            Vector3 posOnField = mFootballField.transform.position;
            string sSpot = UT_Strings.StartAndEndString(sLine, '[', ']');
            string sSpotX = UT_Strings.StartAndEndString(sSpot, '[', ',');
            sSpotX = UT_Strings.DeleteMultipleChars(sSpotX, "[,");
            string sSpotZ = UT_Strings.StartAndEndString(sSpot, ',', ']');
            sSpotZ = UT_Strings.DeleteMultipleChars(sSpotZ, ",]");
            Debug.Log("X: " + sSpotX + "\nY: " + sSpotZ);

            // now we place the player on the screen according to the width and height.
            float unitsToPixel = 0.01f;     // unity default
            float fieldYardsToPixels = 10f;
            posOnField.x += float.Parse(sSpotX)*unitsToPixel * fieldYardsToPixels;
            posOnField.y += float.Parse(sSpotZ) * unitsToPixel * fieldYardsToPixels;

            Instantiate(RefPlayer, posOnField, mFootballField.transform.rotation);
        }

    }

    private Vector3 GetStartPos(string line)
    {
        return Vector3.zero;
    }

    void Update()
    {
        
    }
}
