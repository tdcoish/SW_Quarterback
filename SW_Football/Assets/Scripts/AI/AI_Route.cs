/****************************************************************************************
Route gets loaded in from text file, 
************************************************************************************** */

using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AI_Route : MonoBehaviour
{
    public List<Vector3>    mPath;
    public Vector3          mStartPos;

    void Awake()
    {

    }

    /****************************************************************************************
    Takes a string with data for the route, converts it to actual positions.
    ************************************************************************************** */
    public void ReceiveRoute(string sRoute, Vector3 snapPoint)
    {
        mPath = new List<Vector3>();
        
        string sCopy = sRoute;
        while(true){

            if(!sRoute.Contains("(") || !sRoute.Contains(")")){
                break;
            }
            
            // get the next spot
            string sNode = UT_Strings.StartAndEndString(sRoute, '(', ')');

            // break the spot down into its two positions.
            string nodeX = UT_Strings.StartAndEndString(sNode, '(', ',');
            nodeX = nodeX.Replace("(", "");
            nodeX = nodeX.Replace(",", "");
            string nodeY = UT_Strings.StartAndEndString(sNode, ',', ')');
            nodeY = nodeY.Replace(",", "");
            nodeY = nodeY.Replace(")", "");
            //myString = Regex.Replace(myString, @"[;,\t\r ]|[\n]{2}", "\n");

            Vector3 vSpot = new Vector3();
            vSpot.x = float.Parse(nodeX);
            vSpot.z = float.Parse(nodeY);           // needs to be z or they'll try to move upwards.

            mPath.Add(vSpot);

            // skip ahead to the next position
            sRoute = sRoute.Substring(sRoute.IndexOf(')')+1);
        }

        // now we set the starting position, relative to the "snap point" passed in.
        if(!sCopy.Contains("[") || !sCopy.Contains("]")){
            Debug.Log("No start position found\n");
            return;
        }
        
        string sSpot = UT_Strings.StartAndEndString(sCopy, '[', ']');
        string xSpot = UT_Strings.StartAndEndString(sSpot, '[', ',');
        xSpot = xSpot.Replace("[", "");
        xSpot = xSpot.Replace(",", "");
        string ySpot = UT_Strings.StartAndEndString(sSpot, ',', ']');
        ySpot = ySpot.Replace(",", "");
        ySpot = ySpot.Replace("]", "");

        mStartPos = transform.position;
        mStartPos.x = float.Parse(xSpot) + snapPoint.x;
        mStartPos.z = float.Parse(ySpot) + snapPoint.z;
        transform.position = mStartPos;
    }

    void Update()
    {
        
    }
}
