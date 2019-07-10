/*************************************************************************************
Writes the play that the gamer is creating
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
        for(int i=0; i<cRouteTool.rRoutes.Count; i++){
            fileContents += cRouteTool.rRoutes[i].mPlayerTag + ": ";
            // now we get the route.
            for(int j=0; j<cRouteTool.rRoutes[i].mNodes.Count; j++){
                Vector3 nodePos = cRouteTool.rRoutes[i].mNodes[j].transform.position;
                fileContents += "(" + cRouteTool.rRoutes[i].mNodes[j].transform.position.x + "," + cRouteTool.rRoutes[i].mNodes[j].transform.position.y + ")";
            }
            fileContents += "\tLINEUP: [" + cRouteTool.rRoutes[i].mNodes[0].transform.position.x + "," + cRouteTool.rRoutes[i].mNodes[0].transform.position.y + "]";

            fileContents += "\n";
        }

        File.WriteAllText(path, fileContents);
    }
}
