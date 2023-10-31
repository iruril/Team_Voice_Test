using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class VoiceManager : MonoBehaviourPun
{
    [SerializeField]
    private Recorder _voiceRecorder;

    [SerializeField]
    private Button _joinBlueTeamButton;
    [SerializeField]
    private Button _joinRedTeamButton;

    private const byte BLUE_TEAM_GROUP = 1;
    private const byte RED_TEAM_GROUP = 2;

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
        GameObject canvas = GameObject.Find("Canvas");
        _voiceRecorder = GameManager.instance.MyRecorder;
        _joinBlueTeamButton = canvas.transform.GetChild(0).GetComponent<Button>();
        _joinRedTeamButton = canvas.transform.GetChild(1).GetComponent<Button>();
    }

    void Start()
    {
        if (!photonView.IsMine) return;
        _joinBlueTeamButton.onClick.AddListener(JoinBlueTeam);
        _joinRedTeamButton.onClick.AddListener(JoinRedTeam);
    }

    private void JoinBlueTeam()
    {
        if (!photonView.IsMine) return;
        playerTeam = Team.BlueTeam;
        
        SetupVoiceGroup();
        UpdateUI();
        Debug.Log(PhotonNetwork.LocalPlayer.UserId + " " + (Team)_voiceRecorder.InterestGroup);
    }

    private void JoinRedTeam()
    {         
        if (!photonView.IsMine) return;
        playerTeam = Team.RedTeam;
        SetupVoiceGroup();
        UpdateUI();
        Debug.Log(PhotonNetwork.LocalPlayer.UserId + " " + (Team)_voiceRecorder.InterestGroup);
    }

    private void SetupVoiceGroup()
    {
        if (playerTeam == Team.BlueTeam)
        {
            PunVoiceClient.Instance.Client.OpChangeGroups(new byte[0], new byte[1] { BLUE_TEAM_GROUP });
            _voiceRecorder.InterestGroup = BLUE_TEAM_GROUP;
        }
        else if (playerTeam == Team.RedTeam)
        {
            PunVoiceClient.Instance.Client.OpChangeGroups(new byte[0], new byte[1] { RED_TEAM_GROUP });
            _voiceRecorder.InterestGroup = RED_TEAM_GROUP;
        }
    }

    private void UpdateUI()
    {
        _joinBlueTeamButton.interactable = (playerTeam != Team.BlueTeam);
        _joinRedTeamButton.interactable = (playerTeam != Team.RedTeam);
    }
}
