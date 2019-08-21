using UnityEngine;

/*******************************************************************************************
Right now this class works on a spawn system for the SFX. The SFX are game objects that get spawned
in. This could lead to the accidentaly spawning of way too many per frame, but it's fine. When switching
levels, just use a mixer to reduce SFX to zero.

Music is different, and will require a Singleton, or something else.
***************************************************************************************** */
public class AD_Manager : MonoBehaviour {

	public AudioSource			SFX_Button;

	public void OnMenuButtonPressed()
	{
		Instantiate(SFX_Button);
	}
}
