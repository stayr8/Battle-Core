using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{

    [Header("Ranged Varialbes")]
    public GameObject projPrefab;
    public Transform projSpawnPoint;

    public GameObject slash;
    private GameObject Slashs;


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

    public void StartRangedSkill()
    {
        Slashs = Instantiate(slash, projSpawnPoint.transform.position, projSpawnPoint.rotation);
    }

    public void EndRangedSkill()
    {
        Destroy(Slashs);
    }
}
