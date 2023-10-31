using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private string _sceneName = "Game";
    public string RoomID;

    public DefaultPool DefaultPool;
    public List<string> ObjectAddresses = new();

    private void Start()
    {
        PhotonNetwork.GameVersion = "1.0.0";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        int randID = Random.Range(0, 1000);
        PhotonNetwork.LocalPlayer.NickName = randID.ToString();
        Debug.Log("PUN OnConnectedToMaster " + PhotonNetwork.NetworkClientState.ToString() + ", Nickname : " + PhotonNetwork.LocalPlayer.NickName);
    }

    public void JoinOrCreateRoom()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
        {
            PhotonNetwork.JoinOrCreateRoom("111", new RoomOptions { MaxPlayers = 10, EmptyRoomTtl = 0}, Photon.Realtime.TypedLobby.Default);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed " + returnCode.ToString() + " " + message.ToString());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");

        PhotonNetwork.Reconnect();
    }

    public override void OnJoinedRoom()
    {
        GameManager.instance.isConnect = true;
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("KAI : " + _sceneName);
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();
            PhotonNetwork.LoadLevel(_sceneName);
        }
    }

    public override void OnLeftRoom()
    {
        GameManager.instance.isConnect = false;
    }
}
