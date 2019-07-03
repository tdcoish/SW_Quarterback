/*************************************************************************************
Exists to manage all clicks with the mouse, we raycast and find which oject the player 
clicked on.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class RT_ClickHandler : MonoBehaviour
{
    public List<RT_Player> rPlayers;

    void Start()
    {
        rPlayers = new List<RT_Player>();

        var objs = FindObjectsOfType<RT_Player>();
        foreach(RT_Player ply in objs){
            rPlayers.Add(ply);
        }
    }

    // so now what we do is raycast everytime they click, to see if we've hit a player.
    // It does not intuitively make sense to me why the forward vector2 is (0f,0f).
    // I guess it assumes that if forward is 0,0, then it must be 0,0,1
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            
            // first we de-activate all the rt_Player scripts.
            for(int i=0; i<rPlayers.Count; i++){
                rPlayers[i].mIsChosen = false;
            }

            Debug.Log("here");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if(hit.collider != null)
            {
                Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
                if(hit.collider.GetComponent<RT_Player>() != null){
                    Debug.Log("Hit RT_Player");
                    hit.collider.GetComponent<RT_Player>().mIsChosen = true;
                }
            }

        }

    }

    /// <summary>
    /// Cast a ray from the mouse to the target object
    /// Then sets the target position of the ability to that object.
    /// </summary>
    public void ScreenMouseRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;
 
        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);
 
        Collider2D[] col = Physics2D.OverlapPointAll(v);
    }
}
