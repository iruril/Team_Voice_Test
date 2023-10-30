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

    /// <summary>
    /// 포톤 마스터 연결 시 호출
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN OnConnectedToMaster " + PhotonNetwork.NetworkClientState.ToString());

        // 방이 있으면 입장, 없으면 생성
        //JoinOrCreateRoom("roomName", "RoomEdit");
    }

    /// <summary>
    /// 방이 있으면 입장, 없으면 생성
    /// </summary>
    public void JoinOrCreateRoom()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
        {
            PhotonNetwork.JoinOrCreateRoom("111", new RoomOptions { MaxPlayers = 0, EmptyRoomTtl = 0 }, Photon.Realtime.TypedLobby.Default);
        }
    }

    /// <summary>
    /// 방이 생성될 때 호출
    /// </summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    /// <summary>
    /// 방 생성에 실패 했을 때 호출
    /// </summary>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed " + returnCode.ToString() + " " + message.ToString());
    }

    /// <summary>
    /// 포톤 연결이 끊겼을 때 호출
    /// </summary>\
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");

        PhotonNetwork.Reconnect();
    }

    public void ConnectToChatRoom(string roomName)
    {
        // 챗룸 접속 
    }

    /// <summary>
    /// 방 접속 시 호출
    /// </summary>
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("KAI : " + _sceneName);
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();
            PhotonNetwork.LoadLevel(_sceneName);
        }
    }

    public override void OnLeftRoom()
    {
        //GameManager.Instance.SceneManager.ChangeScene("MainScene");
    }
}
