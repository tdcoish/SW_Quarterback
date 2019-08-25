﻿/*************************************************************************************
Gets all the DT_PlayerRoles in the scene and saves them all to a new DT_OffencePlay.

We listen to a button in the scene.

It's now time to find the actual position.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PE_PlaySaver : MonoBehaviour
{
    public PE_Field             rField;

    // Fun fact, scriptable objects created at runtime will not be kept.

    // So what this was doing was writing the existing play to FL_Play
    public void FSavePlay(string sName, DT_OffencePlay FL_Play)
    {
        FL_Play.mName = sName;
        
        // get array of all ED_Roles? Then save them.
        PE_Role[] roles = FindObjectsOfType<PE_Role>();
        FL_Play.mPlayerRoles = new List<DT_PlayerRole>();
        for(int i=0; i<roles.Length; i++){
            DT_PlayerRole temp = new DT_PlayerRole();
            temp.mRole = roles[i].mRole;
            // temp.mStart = roles[i].mStartPos;
            temp.mTag = roles[i].mTag;

            // find the position from the field position.
            float fMetersToPixels = 50f / rField.GetComponent<RectTransform>().rect.width;      // field is 50 meters
            temp.mStart.x = (roles[i].transform.position.x - rField.transform.position.x) / fMetersToPixels;
            // adjust for starting on 10.
            temp.mStart.y = (roles[i].transform.position.y - rField.transform.position.y) / fMetersToPixels + 15f;

            FL_Play.mPlayerRoles.Add(temp);
        }
    }

    public DATA_Play FConvertPlayToDATA(string sName)
    {
        DATA_Play play = new DATA_Play();
        play.mName = sName;
        
        // get array of all ED_Roles? Then save them.
        PE_Role[] roles = FindObjectsOfType<PE_Role>();
        play.mPlayerRoles = new DT_PlayerRole[roles.Length];
        for(int i=0; i<roles.Length; i++){
            DT_PlayerRole temp = new DT_PlayerRole();
            temp.mRole = roles[i].mRole;
            // temp.mStart = roles[i].mStartPos;
            temp.mTag = roles[i].mTag;

            // find the position from the field position.
            float fMetersToPixels = 50f / rField.GetComponent<RectTransform>().rect.width;      // field is 50 meters
            temp.mStart.x = (roles[i].transform.position.x - rField.transform.position.x) / fMetersToPixels;
            // adjust for starting on 10.
            temp.mStart.y = (roles[i].transform.position.y - rField.transform.position.y) / fMetersToPixels + 15f;

            // oh lol, forgot to add detail.
            temp.mDetail = roles[i].mDetails;

            play.mPlayerRoles[i] = temp;
        }

        return play;
    }



}
