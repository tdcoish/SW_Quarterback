/*************************************************************************************
Everything that I chop out of other places goes into here.
*************************************************************************************/
using UnityEngine;

public class IO_ExampleCode : MonoBehaviour
{
    // ----------------------------- GETTING ALL THE FILES IN A DIRECTORY
        // // first check if that route already exists.
    // string[] fPathNames = Directory.GetFiles(path, "*.bin");
    // path += route.mName+".bin";
    // foreach(string sName in fPathNames)
    // {
    //     if(sName == path)
    //     {
    //         Debug.Log("Error. Route name conflict.");
    //         return;
    //     }
    // }

    // -------------------- And the text version
        //     string[] fPathNames = Directory.GetFiles(Application.dataPath+"/Plays/Routes/", "*.txt");
        // string fName = Application.dataPath+"/Plays/Routes/"+route.mName+".txt";
        // foreach(string sName in fPathNames)
        // {
        //     if(sName == fName)
        //     {
        //         Debug.Log("Error. Route name conflict.");
        //         return false;
        //     }
        // }

    /*
        // Will return false if the write was a failure.
    public static bool FWRITE_ROUTE(DATA_Route route)
    {
        if(route.mName == string.Empty)
        {
            Debug.Log("Can't save un-named route");
            return false;
        }

        string path = Application.dataPath+"/Plays/Routes/"+route.mName+".bin";

        BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));
        bw.Write(route.mName);
        bw.Write(route.mSpots.Length);
        foreach (Vector2 vec2 in route.mSpots)
        {
            bw.Write(vec2.x);
            bw.Write(vec2.y);
        }

        bw.Close();

        // You know, we really might as well just always reload
        FLOAD_ROUTES();

        return true;
    }

    public static bool FCHECK_ROUTE_EXISTS(string sName)
    {
        foreach (DATA_Route rt in mRoutes)
        {
            if(rt.mName == sName)
            {
                return true;
            }
        }

        return false;
    }

    public static void FLOAD_ROUTES()
    {
        Debug.Log("Loading/Reloading routes");
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
     */
}
