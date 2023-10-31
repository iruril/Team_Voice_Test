using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class VoiceGroupManager : MonoBehaviourPunCallbacks
{
    public Dictionary<byte, List<string>> VoiceGroups = new();

    private const byte _clamp = 255;

    [PunRPC]
    public void CreateVoiceGroup(string[] users, int[] pvIDs)
    {
        byte groupID = 1;
        for(groupID = 1; groupID < _clamp; groupID++)
        {
            if (!VoiceGroups.ContainsKey(groupID)) break;
        }

        List<string> tempList = new();
        foreach(string item in users)
        {
            tempList.Add(item);
        }
        VoiceGroups.Add(groupID, tempList);

        Debugger(groupID);

        foreach (int item in pvIDs)
        {
            if (photonView.IsMine)
            {
                Debug.Log("VGM : " + item);
                PhotonView.Find(item).RPC("EnterVoiceGroup", RpcTarget.All, groupID, item);
            }
        }
    }

    [PunRPC]
    public void InviteSpecificUserIntoVoiceGroup(string user, int pvID, byte groupID)
    {
        List<string> tempList = VoiceGroups[groupID];

        tempList.Add(user);
        VoiceGroups[groupID] = tempList;
        Debugger(groupID);

        PhotonView.Find(pvID).RPC("EnterVoiceGroup", RpcTarget.All, groupID, pvID);
    }

    [PunRPC]
    public void ExitSpecificVoiceGroup(string user, byte groupID)
    {
        List<string> tempList = VoiceGroups[groupID];
        if (!tempList.Contains(user)) return;

        tempList.Remove(user);
        VoiceGroups[groupID] = tempList;
        if(tempList.Count == 0)
        {
            VoiceGroups.Remove(groupID);
        }

        Debugger(groupID);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        string userID = otherPlayer.NickName;
        Debug.Log("Exit : " + otherPlayer.NickName);

        if (otherPlayer.CustomProperties.ContainsKey("VoiceGroup"))
        {
            byte groupID = (byte)otherPlayer.CustomProperties["VoiceGroup"];
            photonView.RPC("ExitSpecificVoiceGroup", RpcTarget.AllBufferedViaServer, userID, groupID);

            Debugger(groupID);
        }
    }

    private void Debugger(byte groupID)
    {
        if (!VoiceGroups.ContainsKey(groupID))
        {
            Debug.Log("[WARNING] VoiceGroupID : " + groupID + " Has Destroyed !!!!!");
            return;
        }

        Debug.Log("VoiceGroupID : " + groupID + " ====== >");
        Debug.Log("-----------------------------------");
        foreach(string id in VoiceGroups[groupID])
        {
            Debug.Log("Participant : " + id);
        }
        Debug.Log("----------------END----------------");
    }
}
