using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Data;


public class PhotonInit : MonoBehaviourPunCallbacks
{
    public InputField playerInput;

    public static PhotonInit instance;



    private bool isGameStart = false;
    private bool isLogin = false;
    private string playerID = "";


    //게임 시작 버튼
    public void SetPlayerName()
    {
        Debug.Log("플레이어 ID: " + playerInput.text);

        if (isGameStart == false && isLogin == false)
        {
            playerID = playerInput.text;
            playerInput.text = string.Empty;

            Debug.Log("connect 시도" + isGameStart + ", " + isLogin);

            Connect();
        }
    }


    //로비 입장시 호출
    public override void OnJoinedLobby()
    {
        Debug.Log("Join Lobby");

        PhotonNetwork.JoinRandomRoom();
    }

    //룸 입장 실패시 호출
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No Room");
    }

    //룸 생성 완료시 호출
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("Finish make a room");
    }

    //룸 입장시 호출
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Joined Room");
        isLogin = true;

        PlayerPrefs.SetInt("LogIn", 1);

        PhotonNetwork.LoadLevel("demo");
    }

    public static PhotonInit Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

                if (instance == null)
                {
                    Debug.Log("No Singleton Obj");
                }
            }

            return instance;
        }
    }


    //룸 접속
    public void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }



    private void Awake()
    {
        PhotonNetwork.GameVersion = "CoreBattle 1.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        if(PlayerPrefs.GetInt("LogIn") == 1)
        {
            isLogin = true;
        }

        if (isGameStart == false && SceneManager.GetActiveScene().name == "demo" && isLogin == true)
        {
            isGameStart = true;
        }

        StartCoroutine(CreatPlayer());
    }


    IEnumerator CreatPlayer()
    {
        //PhotonNetwork.Instantiate("Player", new Vector3(0, 1, 0), Quaternion.identity, 0);

        yield return null;
    }
}
