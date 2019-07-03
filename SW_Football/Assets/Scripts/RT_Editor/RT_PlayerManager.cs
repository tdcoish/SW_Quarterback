/*************************************************************************************
Holds ref to all players, handles which ones active, etcetera.
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class RT_PlayerManager : MonoBehaviour
{
    public List<RT_Player> mPlayers = new List<RT_Player>();

    void Start()
    {
        var players = FindObjectsOfType<RT_Player>();
        foreach(RT_Player ply in players){
            mPlayers.Add(ply);
        }
    }

    // it needs to know which player is active.
    void Update()
    {
        
    }
}
