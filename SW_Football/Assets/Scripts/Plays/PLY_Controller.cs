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
    public string               mPlay;

    // AI_Route now gets passed in a string that it converts to a route.
    // for now, set up the receivers in the gameworld first.
    public List<AI_Route>       mRoutes;

    void Start()
    {
        string sPlay = File.ReadAllText(Application.dataPath + "/Plays/" + mPlay);

        // now the difference is that we pass off lines to AI_Route, 
        // instead of having the individual route do everything.
        while(true)
        {
            if(!mPlay.Contains("---")){
                break;
            }
        }
    }

    void Update()
    {
        
    }
}
