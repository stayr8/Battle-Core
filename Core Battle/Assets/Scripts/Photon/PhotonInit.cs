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

    [Header("GameObject")]
    public GameObject LobbyPanel;
    public GameObject MakeRoomPanel;
    public GameObject RoomPanel;
    public GameObject PwPanel;
    public GameObject PwErrorLog;
    public GameObject PwConfirmBtn;
    public GameObject PWPanelCloseBtn;
    public GameObject SelectChar;
    public GameObject playBtn;
    public GameObject readyPtn;

    public GameObject ResultPanel;

    public Text roomName;

    [Header("InputField")]
    public InputField PlayerInput;
    public InputField RoomInput;
    public InputField RoomPwInput;
    public InputField PwCheckIF;

    [Header("Toggle")]
    public Toggle PwToggle;

    [Header("Button")]
    public Button[] CellBtn;
    public Button CreateRoomBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("var")]
    public string gameVersion = "CoreBattle 1.0";
    public string playerID = "";
    public string privateRoom;
    public int hashtablecount;
    public int readyCheck;

    private bool isGameStart = false;
    private bool isLogin = false;
    private bool isReady = false;
    private string connectionState = "";

    [Header("SelectChar")]
    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    List<RoomInfo> myList = new List<RoomInfo>();
    private int curPage = 1, maxPage, multiple, roomNum;

    Text resultInfo;
    Text connectionInfo;

    HealthSystem healthSystem;
    PhotonView pv;

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

        if (isGameStart == false && isLogin == false && isReady == false)
        {
            playerID = PlayerInput.text;
            PlayerInput.text = string.Empty;

            OnLogin();
        }
        else
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
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 3, BroadcastPropsChangeToAll = true });
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

        OnLogin();

        isGameStart = false;
        isLogin = false;
        isReady = false;
    }
    public void CreatNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 3;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"password", RoomPwInput.text }
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        if (PwToggle.isOn)
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : "*" + RoomInput.text, roomOptions);
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 3 });
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
    public void OnLogin()
    {
        PhotonNetwork.NickName = this.playerID;

        if (PhotonNetwork.IsConnected)
        {
            connectionState = "룸에 접속 중...";
            if (connectionInfo)
                connectionInfo.text = connectionState;

            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);

            PhotonNetwork.AutomaticallySyncScene = true;    //?
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

    public void GameExitOnClick()
    {
        ResultPanel.SetActive(false);

        Application.Quit();
    }
    public void ReturnLobbyOnClick()
    {
        ResultPanel.SetActive(false);

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("LoginScene");
    }
    public void gameResult()
    {
        ResultPanel.SetActive(true);

        resultInfo.text = PhotonNetwork.CurrentRoom.Name + "님의 등수는" + PhotonNetwork.PlayerList.Length.ToString() + "위 입니다!";
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        connectionState = "로비 접속 완료!";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        myList.Clear();
        roomName.text = PhotonNetwork.CurrentRoom.Name;

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

        LobbyPanel.SetActive(false);
        SelectChar.SetActive(true);

        connectionState = "룸 접속 완료!";
        if (connectionInfo)
            connectionInfo.text = connectionState;

        isLogin = true;
        PlayerPrefs.SetInt("LogIn", 1);

        UpdatePLayerList();

        //PhotonNetwork.LoadLevel("SampleScene");
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
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePLayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePLayerList();
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
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }
    private void OnApplicationQuit()
    {
        readyCheck = 0;
        isReady = false;
        PlayerPrefs.SetInt("LogIn", 0);
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

        //DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("LogIn") == 1)
        {
            isLogin = true;
        }

        if (isGameStart == false && SceneManager.GetActiveScene().name == "SampleScene" && isLogin == true)
        {
            isGameStart = true;

            StartCoroutine(CreatPlayer());
        }

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1 && isReady == true)
        {
            playBtn.SetActive(true);
        }
        else
        {
           // playBtn.SetActive(false);
        }

        //if(healthSystem.isDie == true)
        //{
        //    gameResult();
        //}
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
    IEnumerator CreatPlayer()
    {
        while (!isGameStart)
        {
            yield return new WaitForSeconds(0.5f);
        }

        GameObject tempPlayer = PhotonNetwork.Instantiate("Sowrd", new Vector3(0, 0, 0), Quaternion.identity, 0);
        //tempPlayer.GetComponent<PlayerCtrl>().SetPlayerName(playerID);
        pv = GetComponent<PhotonView>();

        yield return null;
    }

    //캐릭터 선택
    private void UpdatePLayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);
        }
    }
    public void OnClickStartBtn()
    {
        PhotonNetwork.LoadLevel("demo");
    }
    public void OnClickReadyBtn()
    {
        readyCheck++;
        Debug.Log(readyCheck);

        if(readyCheck >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            isReady = true;
        }
    }
}
