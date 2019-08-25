/*************************************************************************************
Trying to simultaneously learn binary files with this.

Alrighty, now it's just time to try to save a Vec2 and see what happens.

Woah. Crazy bug. Apparently if I open two BinaryWriters in the same context, everything gets 
super fucked up. Zero clue as to why. Hopefully future me can figure this one out.

Going to keep this class around as a reference.
*************************************************************************************/
using UnityEngine;
using System.IO;

public class TestClass
{
    public int whatevs = 4;
    public string muhString = "Timothy Is Cool";
    public float someFloat = 7.5f;
}

public class IO_BinaryTest : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            FTestWrite();
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            FTestRead();
        }
    }

    public void FTestWrite()
    {
        string path = Application.dataPath+"/Plays/Offence/test3.bin";
        BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));
        TestClass t = new TestClass();
        bw.Write(t.whatevs);
        bw.Write(t.muhString);
        bw.Write(t.someFloat);
        bw.Close();

        // string path = Application.dataPath+"/Plays/Offence/test3.bin";
        // BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));
        // Vector2 vVec2 = new Vector2();
        // vVec2.x = -30f; vVec2.y = 40f;
        // Debug.Log(vVec2);
        // bw.Write(200f);
        // bw.Write(vVec2.x);
        // bw.Write(vVec2.y);
        // bw.Close();
    }

    public void FTestRead()
    {
        string path = Application.dataPath+"/Plays/Offence/test3.bin";
        BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
        TestClass myT = new TestClass();
        myT.whatevs = -1;
        myT.whatevs = br.ReadInt32();
        myT.muhString = br.ReadString();
        myT.someFloat = br.ReadSingle();
        Debug.Log(myT.whatevs);
        Debug.Log(myT.muhString);
        Debug.Log(myT.someFloat);
        br.Close();
        
        // string path = Application.dataPath+"/Plays/Offence/test3.bin";
        // BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
        // Vector2 myVec2 = new Vector2();
        // float temp = br.ReadSingle();
        // myVec2.x = br.ReadSingle();
        // myVec2.y = br.ReadSingle();
        // Debug.Log(temp);
        // Debug.Log(myVec2);
        // br.Close();
    }

    public void FWriteRouteToDisk(DT_Route dt_route)
    {
        string path = Application.dataPath+"/Plays/Offence/"+dt_route.mName+".bin";
        BinaryWriter b = new BinaryWriter(File.Open(path, FileMode.Create));
        b.Write(dt_route.mName);
    }

    
}
