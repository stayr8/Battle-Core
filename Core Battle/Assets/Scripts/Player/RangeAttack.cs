using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{

    [Header("Ranged Varialbes")]
    public GameObject projPrefab;
    public Transform projSpawnPoint;

    private PlayerCtr playerCtr;

    // Start is called before the first frame update
    void Start()
    {
        playerCtr = GetComponent<PlayerCtr>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RangedAttack()
    {
        Instantiate(projPrefab, projSpawnPoint.transform.position, projSpawnPoint.transform.rotation); 
    }

}
