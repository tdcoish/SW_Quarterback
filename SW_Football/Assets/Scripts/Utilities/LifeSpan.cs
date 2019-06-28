using UnityEngine;

public class LifeSpan : MonoBehaviour {

	[SerializeField]
	private float lifeTime = 3f;
	
	void Awake() 
	{
		lifeTime = Mathf.Abs(lifeTime);
		Destroy(gameObject, lifeTime);
	}
}
