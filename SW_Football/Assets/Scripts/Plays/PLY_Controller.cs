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
    public string               mOffPlayName = "DefaultPlay";
    public string               mDefPlayName = "Cover2";

    // AI_Route now gets passed in a string that it converts to a route.
    // for now, set up the receivers in the gameworld first.
    public GameObject           RefAthlete;
    public List<AI_Route>       mRoutes;
    public GameObject           PF_Defender;
    public List<AI_ZoneDefence> mDefenders;

    [SerializeField]
    private PLY_SnapSpot        mSnapSpot;

    [SerializeField]
    private GE_Event            GE_PLY_Restart;

    void Awake()
    {
        SetUpPlay();
    }

    private void SetUpPlay()
    {
        GE_PLY_Restart.Raise(null);

        //if we've already instantiated players, delete them
        for(int i=0; i<mRoutes.Count; i++){
            Destroy(mRoutes[i].gameObject);
        }
        for(int i=0; i<mDefenders.Count; i++){
            Destroy(mDefenders[i].gameObject);
        }
        mRoutes = new List<AI_Route>();
         
        StreamReader sReader = new StreamReader(Application.dataPath +"/Plays/"+mOffPlayName+".txt");
        string sLine = string.Empty;
        while((sLine = sReader.ReadLine()) != null)
        {
            var clone = Instantiate(RefAthlete, transform.position, transform.rotation);
            clone.GetComponent<AI_Route>().ReceiveRoute(sLine, mSnapSpot.transform.position);
            clone.GetComponent<AI_Athlete>().mTag = UT_Strings.StartAndEndString(sLine, ':');
            // Debug.Log("Tag: " + clone.GetComponent<AI_Athlete>().mTag);
            if(clone.GetComponent<AI_Athlete>().mTag.Contains("G")){
                Destroy(clone);
                continue;
            }
            mRoutes.Add(clone.GetComponent<AI_Route>());
        }

        // Now we set up the defenders.
        mDefenders = new List<AI_ZoneDefence>();
        sReader = new StreamReader(Application.dataPath+"/Plays/Defense/"+mDefPlayName+".txt");
        sLine = string.Empty;
        while((sLine = sReader.ReadLine()) != null)
        {
            var clone = Instantiate(PF_Defender, transform.position, transform.rotation);
            clone.GetComponent<AI_ZoneDefence>().ReceivePlayAssignment(sLine, mSnapSpot.transform.position);
            mDefenders.Add(clone.GetComponent<AI_ZoneDefence>());
        }
    }

    // If they press i, restart the play.
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)){
            SetUpPlay();
        }
    }
}
