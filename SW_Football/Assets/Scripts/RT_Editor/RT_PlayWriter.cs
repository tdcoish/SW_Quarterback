/*************************************************************************************
Writes the play that the gamer is creating

We already have tags both in the route and the player. So we can reference that, to write
the player.
*************************************************************************************/
using UnityEngine;
using System.IO;

public class RT_PlayWriter : MonoBehaviour
{

    private RT_RouteTool            cRouteTool;
    private RT_SceneManager         cEdManager;

    void Start()
    {
        cRouteTool = GetComponent<RT_RouteTool>();
        cEdManager = GetComponent<RT_SceneManager>();
    }

    // Triggered by button
    public void SavePlay()
    {
        // for now, let's just shove all the text into some text file and see what we get.
        // StreamReader sReader = new StreamReader(Application.dataPath+"/Plays/"+mDefaultPlay);
        string path = Application.dataPath+"/Plays/EditorCreatedPlays/"+cEdManager.mPlayNameEnter.text+".txt";
        
        string fileContents = string.Empty;
        // new way, iterate through every player in the scene, get the tag, find out if the tag exists in a route,
        // if so, write route first, then lineup.
        RT_Player[] rPlayers = FindObjectsOfType<RT_Player>();
        for(int i=0; i<rPlayers.Length; i++){

            fileContents += rPlayers[i].mTag + ": ";

            // get the tag and find if there are any routes with that tag.
            for(int j=0; j<cRouteTool.rRoutes.Count; j++){
                // if tags match, write all the nodes
                if(cRouteTool.rRoutes[j].mPlayerTag == rPlayers[i].mTag){
                    for(int k=0; k<cRouteTool.rRoutes[j].mNodes.Count; k++){
                        Vector3 nodePos = PixelsToYardsConversion(cRouteTool.rRoutes[j].mNodes[k].transform.position);
                        fileContents += "(" + string.Format("{0:0.0}", nodePos.x) + "," + string.Format("{0:0.0}", nodePos.y) + ")";
                    }
                }
            }
            // now write the LINEUP
            Vector3 yrdPos = PixelsToYardsConversion(rPlayers[i].transform.position);
            fileContents += "\tLINEUP: [" + string.Format("{0:0.0}", yrdPos.x) + "," + string.Format("{0:0.0}", yrdPos.y)+ "]" + "\n";
        }

        File.WriteAllText(path, fileContents);
    }

    Vector2 PixelsToYardsConversion(Vector3 pos)
    {
        Vector3 yrdPos = pos;
        yrdPos -= cEdManager.mFootballField.transform.position;
        float pxToYrd = 53.34f / cEdManager.mFootballField.GetComponent<RectTransform>().rect.width;
        yrdPos.x /= pxToYrd;
        yrdPos.y /= pxToYrd;
        yrdPos.z = 0f;

        return yrdPos;
    }
}
