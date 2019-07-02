/****************************************************************************************
Route gets loaded in from text file, 
************************************************************************************** */

using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AI_Route : MonoBehaviour
{

    // gonna have other things, like which receiver we are, etcetera.
    public string               mNameOfFile = "FakePlay.txt";

    void Start()
    {
        Debug.Log("Path: " + Application.dataPath);
        Debug.Log("Path: " + (Application.dataPath + "/Plays"));
        string path = Application.dataPath + "/Plays/";

        // huh, so apparently this actually works.
        string[] fileNames = Directory.GetFiles(Application.dataPath + "/Plays");
        for(int i=0; i<fileNames.Length; i++){
            Debug.Log("File: " + fileNames[i]);
        }

        // alright here's where we read in the route.
        string file = File.ReadAllText(path + mNameOfFile);
        Debug.Log("File Insides: " + file);

        // now we make a list? of all the positions to go to.
        List<Vector3> mPath = new List<Vector3>();
        // for the file, when we get to the (, the contents are x, then y)
        int pos = 0;
        int breaker = 0;
        while(true){

            if(!file.Contains("(") || !file.Contains(")")){
                break;
            }
            
            // get the first spot
            string sSpot = StartAndEndString(file, '(', ')');

            // break the spot down into its two positions.
            string xSpot = StartAndEndString(sSpot, '(', ',');
            xSpot = xSpot.Replace("(", "");
            xSpot = xSpot.Replace(",", "");

            string ySpot = StartAndEndString(sSpot, ',', ')');
            ySpot = ySpot.Replace(",", "");
            ySpot = ySpot.Replace(")", "");

            Vector3 vSpot = new Vector3();
            vSpot.x = float.Parse(xSpot);
            vSpot.y = float.Parse(ySpot);
            Debug.Log("Spot0: " + vSpot.x + "," + vSpot.y);

            mPath.Add(vSpot);
            foreach(var spot in mPath){
                Debug.Log("PathSpot: " + spot);
            }

            //myString = Regex.Replace(myString, @"[;,\t\r ]|[\n]{2}", "\n");


            file = file.Substring(file.IndexOf(')')+1);
            Debug.Log("File Now: " + file);
            
            breaker++;
            if(breaker > 3)
                break;
        }
    }

    // returns the first substring that starts with first char, and ends with last char.
    // eg. "Help(0,5)(10,5)" Would return (0,5) if we passed in (s, '(',')')
    private string StartAndEndString(string s, char cStart, char cEnd)
    {
        int startPos = -1;
        for(int i=0; i<s.Length; i++)
        {
            if(s[i] == cStart){
                startPos = i;
                break;
            }
        }
        if(startPos == -1){
            return "START CHAR NOT FOUND\n";
        }

        bool foundEnd = false;
        int endPos = startPos+1;
        for(int i=endPos; i<s.Length; i++){
            if(s[i] == cEnd){
                endPos = i;
                foundEnd = true;
                break;
            }
        }
        if(!foundEnd){
            return "END CHAR NOT FOUND\n";
        }

        return s.Substring(startPos, (endPos-startPos + 1));
    }

    void Update()
    {
        
    }
}
