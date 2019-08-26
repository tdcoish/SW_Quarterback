/*************************************************************************************
On start, loads in all the routes, and keeps them as a static list.

Directory.GetFiles is also a candidate. Indeed. In fact it turns out it gives you the 
entire file path, which is fine. So basically it returns filepath names, not file names.


Routes are saved:
1) Name
2) Count of spots
3) Spot data (as individual x and y floats)

Update: Working as planned.
*************************************************************************************/
using UnityEngine;
using System.IO;

// needs an array of DATA_Routes
public static class IO_RouteList
{
    public static DATA_Route[]          mRoutes;

    public static void FWRITE_ROUTE(DATA_Route route)
    {
        string path = Application.dataPath+"/Plays/Routes/";

        // first check if that route already exists.
        string[] fPathNames = Directory.GetFiles(path, "*.bin");
        path += route.mName+".bin";
        foreach(string sName in fPathNames)
        {
            if(sName == path)
            {
                Debug.Log("Error. Route name conflict.");
                return;
            }
        }

        BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));
        bw.Write(route.mName);
        bw.Write(route.mSpots.Length);
        foreach (Vector2 vec2 in route.mSpots)
        {
            bw.Write(vec2.x);
            bw.Write(vec2.y);
        }

        bw.Close();
    }

    public static void FLOAD_ROUTES()
    {
        string path = Application.dataPath+"/Plays/Routes/";

        string[] fPathNames = Directory.GetFiles(path, "*.bin");

        mRoutes = new DATA_Route[fPathNames.Length];
        for(int i=0; i<fPathNames.Length; i++)
        {
            BinaryReader br = new BinaryReader(new FileStream(fPathNames[i], FileMode.Open));
            mRoutes[i] = new DATA_Route();
            mRoutes[i].mName = br.ReadString();
            mRoutes[i].mSpots = new Vector2[br.ReadInt32()];
            for(int j=0; j<mRoutes[i].mSpots.Length; j++)
            {
                mRoutes[i].mSpots[j].x = br.ReadSingle();
                mRoutes[i].mSpots[j].y = br.ReadSingle();
            }

            br.Close();
        }
    }

    // assumes that the routes have all been loaded already
    public static DATA_Route FLOAD_ROUTE_BY_NAME(string sName)
    {
        DATA_Route route = new DATA_Route();
        
        for(int i=0; i<mRoutes.Length; i++)
        {
            if(mRoutes[i].mName == sName)
            {
                route = mRoutes[i];
                return route;
            }
        }
        
        Debug.Log("Play not found");
        return null;
    }

}
