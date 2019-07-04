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
    public string               mPlayName = "DefaultPlay.txt";

    // AI_Route now gets passed in a string that it converts to a route.
    // for now, set up the receivers in the gameworld first.
    public GameObject           RefAthlete;
    public List<AI_Route>       mRoutes;

    public GameObject           RefSnapSpot;

    [SerializeField]
    private PLY_SnapSpot        mSnapSpot;

    void Awake()
    {
        SetUpPlay();
    }

    private void SetUpPlay()
    {
        //if we've already instantiated players, delete them
        for(int i=0; i<mRoutes.Count; i++){
            Destroy(mRoutes[i].gameObject);
        }
        mRoutes = new List<AI_Route>();
         
        StreamReader sReader = new StreamReader(Application.dataPath +"/Plays/"+mPlayName);
        string sLine = string.Empty;
        while((sLine = sReader.ReadLine()) != null)
        {
            var clone = Instantiate(RefAthlete, transform.position, transform.rotation);
            clone.GetComponent<AI_Route>().ReceiveRoute(sLine, mSnapSpot.transform.position);
            mRoutes.Add(clone.GetComponent<AI_Route>());
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
