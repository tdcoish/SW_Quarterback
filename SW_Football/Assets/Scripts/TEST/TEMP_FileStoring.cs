/*************************************************************************************
Exists just for storing some interesting code snippets.
*************************************************************************************/
using UnityEngine;
using System.IO;

public class TEMP_FileStoring : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Path: " + Application.dataPath);
        Debug.Log("Path: " + (Application.dataPath + "/Plays"));

        // huh, so apparently this actually works.
        string[] fileNames = Directory.GetFiles(Application.dataPath + "/Plays");
        for(int i=0; i<fileNames.Length; i++){
            Debug.Log("File: " + fileNames[i]);
        }
    }

    void Update()
    {
        
    }
}
