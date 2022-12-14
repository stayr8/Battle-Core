using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpwan : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;


    private void Start()
    {
        int randomNum = Random.Range(0, spawnPoints.Length);

        Transform spawnPoint = spawnPoints[randomNum];
        GameObject playerTospawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        PhotonNetwork.Instantiate(playerTospawn.name, spawnPoint.position, Quaternion.identity);
    }
}