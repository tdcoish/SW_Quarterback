using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UT_FindComponent {

	// First checks component in self.
	// Then checks if in parent.
	// Then checks if in child.
	// Then finally, goes to the parent, and searches all of its children.
	public static T FindComponent<T>(GameObject go){
		if(go.GetComponent<T>() != null){
			return go.GetComponent<T>();
		}

		if(go.GetComponentInParent<T>() != null){
			return go.GetComponentInParent<T>();
		}

		if(go.GetComponentInChildren<T>() != null){
			return go.GetComponentInChildren<T>();
		}

		if(go.transform.parent){
			if(go.transform.parent.GetComponentInChildren<T>() != null){
				return go.transform.parent.GetComponentInChildren<T>();
			}
		}

		return default(T);
	}

}
