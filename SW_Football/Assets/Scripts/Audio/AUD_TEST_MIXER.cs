/*************************************************************************************
Take two states, transition on button press.
*************************************************************************************/
using UnityEngine;
using UnityEngine.Audio;

public class AUD_TEST_MIXER : MonoBehaviour
{
    public AudioMixerSnapshot               SNP_Normal;
    public AudioMixerSnapshot               SNP_Paused;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            SNP_Paused.TransitionTo(0.5f);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            SNP_Normal.TransitionTo(0.5f);
        }
    }
}
