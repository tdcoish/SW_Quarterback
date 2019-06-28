using UnityEngine;

public class AD_Manager : MonoBehaviour {

    // copied in as an example from previous project.

	[SerializeField]
	private AudioSource 		mHealthPickup;

	public void OnHealthPickup(){
		mHealthPickup.Play();
	}
}
