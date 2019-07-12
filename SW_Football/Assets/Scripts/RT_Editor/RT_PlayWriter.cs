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
                Vector3 nodePos = PixelsToYardsConversion(cRouteTool.rRoutes[i].mNodes[j].transform.position);

                fileContents += "(" + string.Format("{0:0.0}", nodePos.x) + "," + string.Format("{0:0.0}", nodePos.y) + ")";
            }

            // convert from where they are on the field to our coordinates, which unfortunately requires a lot of code
            Vector3 yrdPos = PixelsToYardsConversion(cRouteTool.rRoutes[i].mNodes[0].transform.position);
            
            fileContents += "\tLINEUP: [" + string.Format("{0:0.0}", yrdPos.x) + "," + string.Format("{0:0.0}", yrdPos.y)+ "]";
            fileContents += "\n";
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
