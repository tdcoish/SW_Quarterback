/*************************************************************************************
For now another test.
*************************************************************************************/
using UnityEngine;
using System.IO;

public class PE_RouteLoader : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            FLoadRoute("Slant");
        }
    }

    // Also displays
    void FLoadRoute(string sName)
    {
        string path = Application.dataPath+"/Plays/Offence/"+sName+".bin";
        BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
        string lName = br.ReadString();
        Debug.Log("Name of Route: " + lName);
        Vector2[] vSpots = new Vector2[br.ReadInt32()];
        for(int i=0; i<vSpots.Length; i++)
        {
            vSpots[i].x = br.ReadSingle();
            vSpots[i].y = br.ReadSingle();
            Debug.Log("Spot: " + vSpots[i]);
        }
    }
}
