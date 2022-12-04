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


    //���� ���� ��ư
    public void SetPlayerName()
    {
        Debug.Log("�÷��̾� ID: " + playerInput.text);

        if (isGameStart == false && isLogin == false)
        {
            playerID = playerInput.text;
            playerInput.text = string.Empty;

            Debug.Log("connect �õ�" + isGameStart + ", " + isLogin);

            Connect();
        }
    }


    //�κ� ����� ȣ��
    public override void OnJoinedLobby()
    {
        Debug.Log("Join Lobby");

        PhotonNetwork.JoinRandomRoom();
    }

    //�� ���� ���н� ȣ��
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No Room");
    }

    //�� ���� �Ϸ�� ȣ��
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("Finish make a room");
    }

    //�� ����� ȣ��
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


    //�� ����
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
