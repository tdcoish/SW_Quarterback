using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="GAME_EVENT", menuName="GE/GAME_EVENT")]
public class GE_Event : ScriptableObject{

	// readonly is const for c#
	private readonly List<GE_Listener> eventListeners = 
		new List<GE_Listener>();

	public void Raise(object data)
	{
		for(int i = eventListeners.Count -1; i >= 0; i--)
			eventListeners[i].OnEventRaised(data);
	}

	public void RegisterListener(GE_Listener listener)
	{
		if (!eventListeners.Contains(listener))
			eventListeners.Add(listener);
	}

	public void UnregisterListener(GE_Listener listener)
	{
		if (eventListeners.Contains(listener))
			eventListeners.Remove(listener);
	}
}