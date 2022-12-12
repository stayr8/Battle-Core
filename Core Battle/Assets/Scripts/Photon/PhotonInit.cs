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

    public string gameVersion = "CoreBattle 1.0";
    public string playerID = "";

    //인게임
    public void SetPlayer()
    {
        OnLogin();
    }
    //인게임



    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected");
    }

    private void OnLogin()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = this.playerID;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }


}
