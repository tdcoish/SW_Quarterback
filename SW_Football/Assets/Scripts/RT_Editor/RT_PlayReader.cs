/*************************************************************************************
Reads in play and then sets it up on the "football field".
*************************************************************************************/
using UnityEngine;
using System.IO;

public class RT_PlayReader : MonoBehaviour
{
    private RT_SceneManager         cEdMan;

    void Awake()
    {
        cEdMan = GetComponent<RT_SceneManager>();
    }

    // Reads in the play and sets everything up.
    public void ReadInPlay(string sPlayName)
    {
        // gonna try and use a streamreader 
        StreamReader sReader = new StreamReader(Application.dataPath+"/Plays/"+sPlayName);
        
        string sLine = string.Empty;
        while((sLine = sReader.ReadLine()) != null){
            // use this line to set up our players.
            Vector3 posOnField = cEdMan.mFootballField.transform.position;
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
            posOnField.z = 0f;

            RT_Player ply = Instantiate(cEdMan.PF_Player, posOnField, cEdMan.mFootballField.transform.rotation);
            ply.mTag = sLine.Substring(0, sLine.IndexOf(':'));
        }
    }
}
