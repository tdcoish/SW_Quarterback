/*************************************************************************************
Create a new route. Opens up the route node page.
*************************************************************************************/
using UnityEngine;

public class RT_NewRoute : MonoBehaviour
{
    [SerializeField]
    private GameObject          mNewRoutePage;

    void Start()
    {
        
    }

    void Update()
    {
    }

    // Open up the new route page, have the player dragging route nodes in.
    public void ClickedNewRoute(){
        Debug.Log("Create new route");

        mNewRoutePage.SetActive(true);
    }
}
