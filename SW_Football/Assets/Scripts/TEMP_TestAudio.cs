/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class TEMP_TestAudio : MonoBehaviour
{
    private AudioSource          mAudio;

    void Start()
    {
        mAudio = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mAudio.Play();
        }
    }
}
