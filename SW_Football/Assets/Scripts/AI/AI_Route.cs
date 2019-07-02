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

    public List<Vector3> mPath;

    void Start()
    {
        mPath = new List<Vector3>();

        string path = Application.dataPath + "/Plays/";

        // read in the route from File IO
        string file = File.ReadAllText(path + mNameOfFile);

        while(true){

            if(!file.Contains("(") || !file.Contains(")")){
                break;
            }
            
            // get the next spot
            string sSpot = StartAndEndString(file, '(', ')');

            // break the spot down into its two positions.
            string xSpot = StartAndEndString(sSpot, '(', ',');
            xSpot = xSpot.Replace("(", "");
            xSpot = xSpot.Replace(",", "");
            string ySpot = StartAndEndString(sSpot, ',', ')');
            ySpot = ySpot.Replace(",", "");
            ySpot = ySpot.Replace(")", "");
            //myString = Regex.Replace(myString, @"[;,\t\r ]|[\n]{2}", "\n");

            Vector3 vSpot = new Vector3();
            vSpot.x = float.Parse(xSpot);
            vSpot.y = float.Parse(ySpot);

            mPath.Add(vSpot);

            // skip ahead to the next position
            file = file.Substring(file.IndexOf(')')+1);
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
