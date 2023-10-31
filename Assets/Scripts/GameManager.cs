using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool isConnect = false;

    public GameObject MyAvatar = null;

    public Recorder MyRecorder { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        MyRecorder = GetComponent<Recorder>();
    }
}
