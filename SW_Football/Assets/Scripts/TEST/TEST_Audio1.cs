/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class TEST_Audio1 : MonoBehaviour
{

    public GameObject           PF_AudioSource;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Instantiate(PF_AudioSource);
        }    
    }
}
