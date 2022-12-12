using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Data;
using UnityEngine.Networking.PlayerConnection;


public class PhotonInit : MonoBehaviourPunCallbacks
{
    public static PhotonInit instance;

    public GameObject LobbyPanel;
    public GameObject MakeRoomPanel;
    public GameObject RoomPanel;
    public InputField PlayerInput;
    public InputField RoomInput;

    public string gameVersion = "CoreBattle 1.0";
    public string playerID = "";

    private bool isGameStart = false;
    private bool isLogin = false;
    private string connectionState = "";

    Text connectionInfo;

    public static PhotonInit Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

                if(instance == null)
                {
                    Debug.Log("no singleton object");
                }
            }
            return instance;
        }
    }

    public void SetPlayerName()
    {
        Debug.Log("�÷��̾� ID: " + PlayerInput.text);

        if (isGameStart == false && isLogin == false)
        {
            playerID = PlayerInput.text;
            PlayerInput.text = string.Empty;

            OnLogin();
        }
        else if(isGameStart == true && isLogin == true)
        {
            PlayerInput.text = string.Empty;
        }
    }
    public void CreatRoomBtnOnClick()
    {
        MakeRoomPanel.SetActive(true);
    }
    public void OKBtnOnClick()
    {
        MakeRoomPanel.SetActive(false);
    }
    public void CreatRoom()
    {
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 10 });
        LobbyPanel.SetActive(false);
    }
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();

        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);

        connectionState = "������ ���� ���� ��...";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        isGameStart = false;
        isLogin = false;
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        connectionState = "�κ� ���� �Ϸ�!";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        //PhotonNetwork.JoinRandomRoom();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        connectionState = "������ ���� ���� �Ϸ�!";
        if (connectionInfo)
            connectionInfo.text = connectionState;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        connectionState = "�� ����, �� ������...";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        //this.CreatRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        connectionState = "�� ���� �Ϸ�!";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        isLogin = true;
        PhotonNetwork.LoadLevel("SampleScene");
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        connectionState = "�� ���� �Ϸ�";
        if (connectionInfo)
            connectionInfo.text = connectionState;
    }

    private void OnLogin()
    {
        PhotonNetwork.NickName = this.playerID;

        if (PhotonNetwork.IsConnected)
        {
            connectionState = "�뿡 ���� ��...";
            if (connectionInfo)
                connectionInfo.text = connectionState;

            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);

            PhotonNetwork.JoinLobby();
        }
        else
        {
            connectionState = "������ ���� ���� �Ұ���, ������ ��...";
            if (connectionInfo)
                connectionInfo.text = connectionState;

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Awake()
    {
        PhotonNetwork.GameVersion = "CoreBattle 1.0";
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();

        if (GameObject.Find("ConnectionInfoText") != null)
        {
            connectionInfo = GameObject.Find("ConnectionInfoText").GetComponent<Text>();
        }

        connectionState = "������ ���� ���� ��...";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if(isGameStart == false && SceneManager.GetActiveScene().name == "SampleScene" && isLogin == true)
        {
            isGameStart = true;
        }
    }

}
