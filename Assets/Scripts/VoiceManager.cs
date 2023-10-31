using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    private VoiceGroupManager _voiceGroupManager;

    [SerializeField]
    private Recorder _voiceRecorder;

    [SerializeField]
    private Button _createGroupButton;
    [SerializeField]
    private Button _exitGroupButton;

    private Hashtable _customProperty;

    public enum Team
    {
        None,
        BlueTeam,
        RedTeam
    }

    public Team playerTeam = Team.None;

    private void Awake()
    {
        if (!photonView.IsMine) return;
        _voiceGroupManager = GameObject.FindObjectOfType<VoiceGroupManager>().transform.GetComponent<VoiceGroupManager>();
        GameObject canvas = GameObject.Find("Canvas");
        _voiceRecorder = GameManager.instance.MyRecorder;
        _customProperty = PhotonNetwork.LocalPlayer.CustomProperties;

        _createGroupButton = canvas.transform.GetChild(0).GetComponent<Button>();
        _exitGroupButton = canvas.transform.GetChild(1).GetComponent<Button>();
    }

    void Start()
    {
        if (!photonView.IsMine) return;
        _createGroupButton.onClick.AddListener(CreateVoiceGroup);
        _exitGroupButton.onClick.AddListener(ExitCurrentVoiceGroup);
    }

    public void CreateVoiceGroup() //Test
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("VoiceGroup")) return;

        string[] users = { PhotonNetwork.LocalPlayer.NickName };
        int[] pvIDs = { photonView.ViewID };

        _voiceGroupManager.photonView.RPC("CreateVoiceGroup", RpcTarget.AllBufferedViaServer, users, pvIDs);
    }

    public void ExitCurrentVoiceGroup()
    {
        if (_voiceRecorder.InterestGroup == 0) return;

        string user = PhotonNetwork.LocalPlayer.NickName;
        byte groupID = (byte)PhotonNetwork.LocalPlayer.CustomProperties["VoiceGroup"];
        _voiceGroupManager.photonView.RPC("ExitSpecificVoiceGroup", RpcTarget.AllBufferedViaServer, user, groupID);

        PunVoiceClient.Instance.Client.OpChangeGroups(new byte[0], null);
        _voiceRecorder.InterestGroup = 0;

        _customProperty.Remove("VoiceGroup");
        PhotonNetwork.LocalPlayer.SetCustomProperties(_customProperty);
    }

    [PunRPC]
    public void EnterVoiceGroup(byte groupID, int pvID)
    {
        if (photonView.ViewID != pvID || !photonView.IsMine) return;
        Debug.Log("VM : " + photonView.ViewID);

        PunVoiceClient.Instance.Client.OpChangeGroups(new byte[0], new byte[1] { groupID });
        _voiceRecorder.InterestGroup = groupID;

        _customProperty.Add("VoiceGroup", groupID);
        PhotonNetwork.LocalPlayer.SetCustomProperties(_customProperty);
    }
}
