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
            Instantiate(RefPlayer, mFootballField.transform.position, mFootballField.transform.rotation);
        }
        

        // interesting, by the time we get to here, the streamreader is "finished"
        // lines 2-4 show how to get back to the beginning.
        Debug.Log(sReader.ReadToEnd());
        sReader.DiscardBufferedData();
        sReader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        Debug.Log(sReader.ReadToEnd());

    }

    private Vector3 GetStartPos(string line)
    {
        return Vector3.zero;
    }

    void Update()
    {
        
    }
}
