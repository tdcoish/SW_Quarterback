using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Response : UnityEvent<object> {} // Leave it empty

public class GE_Listener : MonoBehaviour{
    public GE_Event Event;
    public Response response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(object data)
    {
        response.Invoke(data);
    }
}
