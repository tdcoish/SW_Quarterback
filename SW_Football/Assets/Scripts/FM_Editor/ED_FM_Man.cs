/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class ED_FM_Man : MonoBehaviour
{
    void Start()
    {

        DATA_Formation f = IO_Formations.FLOAD_FORMATION("TEST");
        Debug.Log("The name of the formation: " + f.mName);
        f.mName = "TEST2";
        IO_Formations.FWRITE_FORMATION(f);
    }

    void Update()
    {
        
    }
}
