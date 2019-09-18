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

    // Will return false if the write was a failure.
    public static bool FWRITE_ROUTE(DATA_Route route)
    {
        string sName =route.mName;
        StreamWriter sw = new StreamWriter(Application.dataPath+"/Plays/Routes/"+sName+".txt");
        sw.WriteLine(route.mName);
        sw.WriteLine(route.mSpots.Length);
        for(int j=0; j<route.mSpots.Length; j++){
            sw.WriteLine("("+route.mSpots[j].x+","+route.mSpots[j].y+")");
        }

        sw.Close();

        return true;
    }

    public static bool FWRITE_GRID_ROUTE(DATA_Route route)
    {
        string sName =route.mName;
        StreamWriter sw = new StreamWriter(Application.dataPath+"/FILE_IO/Routes/"+sName+".txt");
        sw.WriteLine(route.mName);
        sw.WriteLine(route.mSpots.Length);
        for(int j=0; j<route.mSpots.Length; j++){
            sw.WriteLine("("+route.mSpots[j].x+","+route.mSpots[j].y+")");
        }

        sw.Close();

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

        string sPath = Application.dataPath+"/Plays/Routes/";
        string[] fPathNames = Directory.GetFiles(sPath, "*.txt");
        mRoutes = new DATA_Route[fPathNames.Length];

        for(int j=0; j<mRoutes.Length; j++)
        {
            StreamReader sr = new StreamReader(fPathNames[j]);
            string sLine = string.Empty;
            DATA_Route rt = new DATA_Route();
            rt.mName = sr.ReadLine();
            rt.mSpots = new Vector2[int.Parse(sr.ReadLine())];
            for(int i=0; i<rt.mSpots.Length; i++)
            {
                sLine = sr.ReadLine();
                rt.mSpots[i] = UT_Strings.FGetVecFromString(sLine);
            }

            sr.Close();
            
            mRoutes[j] = rt;
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
        
        Debug.Log("Route not found");
        return null;
    }

    // Here we save the routes as individual files
    public static void FWRITE_ALL_ROUTES_AS_TEXT()
    {
        for(int i=0; i<mRoutes.Length; i++)
        {
            FWRITE_ROUTE(mRoutes[i]);
        }
    }

    // Here we take our already loaded binary files, and convert them to text files.
    public static void FCONVERT_TO_SINGLE_TEXT_FILE()
    {
        // need the num of routes? Sure.
        StreamWriter sw = new StreamWriter(Application.dataPath+"/RoutesText/routes.txt");
        sw.WriteLine(mRoutes.Length);
        for(int i=0; i<mRoutes.Length; i++)
        {
            sw.WriteLine(mRoutes[i].mName);
            sw.WriteLine(mRoutes[i].mSpots.Length);
            for(int j=0; j<mRoutes[i].mSpots.Length; j++)
            {
                sw.Write("("+mRoutes[i].mSpots[j].x+","+mRoutes[i].mSpots[j].y+")");
            }
            sw.Write("\n");
        }
        sw.Close();
    }

}
