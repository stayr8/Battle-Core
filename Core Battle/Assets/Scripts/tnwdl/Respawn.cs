using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject[] charPrefabs;
    public GameObject player;

    private void Start()
    {
        player = Instantiate(charPrefabs[(int)DataManager.instance.currentChar]);
        player.transform.position = transform.position;
    }
}
