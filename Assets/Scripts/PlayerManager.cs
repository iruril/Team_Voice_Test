using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _avatar = null;

    void Start()
    {
        StartCoroutine(CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => GameManager.instance.isConnect);

        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        float x = Random.Range(-2, 2f);
        float y = Random.Range(-2, 2f);

        Vector3 spawn = new Vector3(x, y, 0);
        GameManager.instance.MyAvatar =  PhotonNetwork.Instantiate(_avatar.name, spawn, Quaternion.identity); // 플레이어 생성
    }
}