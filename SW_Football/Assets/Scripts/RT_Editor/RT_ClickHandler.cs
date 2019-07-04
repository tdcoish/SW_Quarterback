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

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if(hit.collider != null)
            {
                // Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
                if(hit.collider.GetComponent<RT_Player>() != null){
                    hit.collider.GetComponent<RT_Player>().mIsChosen = true;
                }
            }

        }

    }
}
