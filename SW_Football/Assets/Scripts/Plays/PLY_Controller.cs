/*************************************************************************************
Has a play, sets up players according to text file. Then makes those players lined up
properly. Then those players follow their routes as given to them by the playbook, but 
only when we have hit space to hike the ball.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PLY_Controller : MonoBehaviour
{
    public string               mPlayName = "FakePlay.txt";

    // AI_Route now gets passed in a string that it converts to a route.
    // for now, set up the receivers in the gameworld first.
    public List<AI_Route>       mRoutes;

    void Awake()
    {
        string sPlay = File.ReadAllText(Application.dataPath + "/Plays/" + mPlayName);

        // now the difference is that we pass off lines to AI_Route, 
        // instead of having the individual route do everything.
        int plyInd = 0;
        int strInd = 0;
        while(true)
        {
            if(strInd == sPlay.Length){
                return;
            }

            int lBreak = sPlay.IndexOf('\n', strInd);
            string sLine;
            if(lBreak != -1){
                sLine = sPlay.Substring(0, sPlay.IndexOf('\n'));
                strInd = sPlay.IndexOf('\n');
            }else{
                sLine = sPlay;
                strInd = sPlay.Length;      // will throw error.
            }

            // now pass off the line, to the AI_Route
            mRoutes[plyInd++].ReceiveRoute(sLine);

            if(plyInd >= mRoutes.Count){
                return;
            }
        }
    }

    void Update()
    {
        
    }
}
