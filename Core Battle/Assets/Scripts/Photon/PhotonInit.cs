using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Data;
using UnityEngine.Networking.PlayerConnection;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PhotonInit : MonoBehaviourPunCallbacks
{
    public static PhotonInit instance;

    public GameObject LobbyPanel;
    public GameObject MakeRoomPanel;
    public GameObject RoomPanel;
    public GameObject PwPanel;
    public GameObject PwErrorLog;

    public InputField PlayerInput;
    public InputField RoomInput;
    public InputField RoomPwInput;
    public InputField PwCheckIF;

    public Toggle PwToggle;

    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public Button CreateRoomBtn;

    public string gameVersion = "CoreBattle 1.0";
    public string playerID = "";
    public int hashtablecount;

    private bool isGameStart = false;
    private bool isLogin = false;
    private string connectionState = "";

    List<RoomInfo> myList = new List<RoomInfo>();
    private int curPage = 1, maxPage, multiple, roomNum;

    Text connectionInfo;

    public static PhotonInit Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

                if (instance == null)
                {
                    Debug.Log("no singleton object");
                }
            }
            return instance;
        }
    }

    public void SetPlayerName()
    {
        Debug.Log("플레이어 ID: " + PlayerInput.text);

        if (isGameStart == false && isLogin == false)
        {
            playerID = PlayerInput.text;
            PlayerInput.text = string.Empty;

            OnLogin();
        }
        else if (isGameStart == true && isLogin == true)
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

        connectionState = "마스터 서버 접속 중...";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        isGameStart = false;
        isLogin = false;
    }
    public void CreatNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"password", RoomPwInput.text }
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        if (PwToggle.isOn)
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, roomOptions);
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 10 });
        }

        MakeRoomPanel.SetActive(false);
    }
    public void MyListClick(int num)
    {
        if (num == -2)
        {
            --curPage;
            MyListRenewal();
        }
        else if (num == -1)
        {
            ++curPage;
            MyListRenewal();
        }
        else if (myList[multiple + num].CustomProperties["password"] != null)
        {
            PwPanel.SetActive(true);
        }
        else
        {
            PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            MyListRenewal();
        }
    }
    public void RoomPw(int num)
    {
        switch (num)
        {
            case 0:
                roomNum = 0;
                break;
            case 1:
                roomNum = 1;
                break;
            case 2:
                roomNum = 2;
                break;
            case 3:
                roomNum = 3;
                break;

            default:
                break;
        }
    }
    public void EnterRoomWithPW()
    {
        if ((string)myList[multiple + roomNum].CustomProperties["password"] == PwCheckIF.text)
        {
            PhotonNetwork.JoinRoom(myList[multiple + roomNum].Name);
            MyListRenewal();
            PwPanel.SetActive(false);
        }
        else
        {
            StartCoroutine("ShowPwWrongMsg");
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        connectionState = "로비 접속 완료!";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        //PhotonNetwork.JoinRandomRoom();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        connectionState = "마스터 서버 연결 완료!";
        if (connectionInfo)
            connectionInfo.text = connectionState;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        connectionState = "룸 없음, 룸 생성중...";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        //this.CreatRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        connectionState = "룸 접속 완료!";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        isLogin = true;
        
        PhotonNetwork.LoadLevel("SampleScene");
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        connectionState = "룸 생성 완료";
        if (connectionInfo)
            connectionInfo.text = connectionState;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        Debug.Log("OnRoomListUpdate:" + roomList.Count);

        int roomCount = roomList.Count;

        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i]))
                {
                    myList.Add(roomList[i]);
                }
                else
                {
                    myList[myList.IndexOf(roomList[i])] = roomList[i];
                }
            }
            else if (myList.IndexOf(roomList[i]) != -1)
            {
                myList.RemoveAt(myList.IndexOf(roomList[i]));
            }
        }

        MyListRenewal();
    }

    private void OnLogin()
    {
        PhotonNetwork.NickName = this.playerID;

        if (PhotonNetwork.IsConnected)
        {
            connectionState = "룸에 접속 중...";
            if (connectionInfo)
                connectionInfo.text = connectionState;

            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);

            PhotonNetwork.JoinLobby();
        }
        else
        {
            connectionState = "마스터 서버 연결 불가능, 재접속 중...";
            if (connectionInfo)
                connectionInfo.text = connectionState;

            PhotonNetwork.ConnectUsingSettings();
        }
    }
    private void MyListRenewal()
    {
        maxPage = (myList.Count % CellBtn.Length == 0)
            ? myList.Count / CellBtn.Length
            : myList.Count / CellBtn.Length + 1;

        PreviousBtn.interactable = (curPage <= 1) ? false : true;
        NextBtn.interactable = (curPage >= maxPage) ? false : true;

        multiple = (curPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count)
             ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
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

        connectionState = "마스터 서버 접속 중...";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (isGameStart == false && SceneManager.GetActiveScene().name == "SampleScene" && isLogin == true)
        {
            isGameStart = true;
        }
    }

    IEnumerator ShowPwWrongMsg()
    {
        if (!PwErrorLog.activeSelf)
        {
            PwErrorLog.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            PwErrorLog.SetActive(false);
        }
    }
}
