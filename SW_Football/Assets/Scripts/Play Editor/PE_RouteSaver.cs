/*************************************************************************************
Routes are saved as:

1) Name
2) Amount of spots
3) Spots, each x and y themselves.

*************************************************************************************/
using UnityEngine;
using System.IO;

public class PE_RouteSaver : MonoBehaviour
{
    
    public void FWriteRouteToDisk(DT_Route FL_Route)
    {
        string path = Application.dataPath+"/Plays/Routes/" + FL_Route.mName +".bin";
        BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));

        // first, write the name of the route.
        bw.Write(FL_Route.mName);
        bw.Write(FL_Route.mSpots.Count);
        foreach (Vector2 spot in FL_Route.mSpots)
        {
            bw.Write(spot.x);
            bw.Write(spot.y);
        }
    }
}
