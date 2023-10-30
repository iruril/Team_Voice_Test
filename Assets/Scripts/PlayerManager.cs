using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject _avatar = null;

    void Start()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        //if (_avatar == null)
        {
            Debug.Log("asdasadasd");
            float x = Random.Range(-2, 2f);
            float y = Random.Range(-2, 2f);

            Vector3 spawn = new Vector3(x, y, 0);
            _avatar = PhotonNetwork.Instantiate("Player", spawn, Quaternion.identity); // 플레이어 생성
        }
    }
}
