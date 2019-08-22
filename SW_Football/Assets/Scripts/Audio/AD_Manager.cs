/*******************************************************************************************
Right now this class works on a spawn system for the SFX. The SFX are game objects that get spawned
in. This could lead to the accidentaly spawning of way too many per frame, but it's fine. When switching
levels, just use a mixer to reduce SFX to zero.

Music is different, and will require a Singleton, or something else.

Huh, since timescale is set to 0 when we pause, we actually need to do this using coroutines.
NVM. Can transition using unscaled time.
***************************************************************************************** */

using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AD_Manager : MonoBehaviour {

	//public SO_Float				SET_MasterVolume;

	public AudioMixer						refAudioMixer;
	public AudioMixerSnapshot               SNP_Normal;
    public AudioMixerSnapshot               SNP_Paused;

	public AudioSource						SFX_Button;

	public void OnBackToNormal()
	{
		SNP_Normal.TransitionTo(0.1f);
	}

	public void OnPauseMenuOpened()
	{
		SNP_Paused.TransitionTo(0.5f);
	}

	public void OnPauseMenuClosed()
	{
		SNP_Normal.TransitionTo(0.5f);
	}

	public void OnMenuButtonPressed()
	{
		Instantiate(SFX_Button);
	}
}
