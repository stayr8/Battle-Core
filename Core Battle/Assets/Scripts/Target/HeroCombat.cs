using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : MonoBehaviour
{
    public enum HeroAttackType { Melee, Ranged };
    public HeroAttackType heroAttackType;

    public GameObject targetedEnemy;
    public float attackRange;

    public GameObject targetedItem;
    public float itemRange;


    private PlayerCtr playerCtr;

    public bool basicAtkIdle = false;
    public bool isHeroAlive;
    public bool performMeleeAttack = true;
    public bool isMove = true;

    [Header("Ranged Varialbes")]
    public bool performRangeAttack = true;
    public GameObject projPrefab;
    public Transform projSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        playerCtr = GetComponent<PlayerCtr>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedEnemy != null)
        {
            if (Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position) > attackRange)
            {
                playerCtr.agent.SetDestination(targetedEnemy.transform.position);
                playerCtr.agent.stoppingDistance = attackRange;
                isMove = true;
            }
            else
            {
                // MELEE CHARACTER
                if (heroAttackType == HeroAttackType.Melee)
                {
                    // Character Dir get
                    var dir = new Vector3(playerCtr.agent.steeringTarget.x, playerCtr.transform.position.y, playerCtr.agent.steeringTarget.z) - playerCtr.transform.position;
                    var dirXZ = new Vector3(dir.x, 0f, dir.z);
                    Quaternion targetRot = Quaternion.LookRotation(dirXZ);
                    playerCtr.rigid.rotation = Quaternion.RotateTowards(playerCtr.transform.rotation, targetRot, 13.0f);
                    //playerCtr.agent.stoppingDistance = Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position);
                    //playerCtr.agent.SetDestination(transform.position)s;
                    isMove = true;

                    if (performMeleeAttack)
                    {
                        //Debug.Log("Attack The Minion");
                        //StartCoroutine(MeleeAttackInterval());
                    }
                }

                // RANGED CHARACTER
                if (heroAttackType == HeroAttackType.Ranged)
                {
                    var dir = new Vector3(playerCtr.agent.steeringTarget.x, playerCtr.transform.position.y, playerCtr.agent.steeringTarget.z) - playerCtr.transform.position;
                    var dirXZ = new Vector3(dir.x, 0f, dir.z);
                    Quaternion targetRot = Quaternion.LookRotation(dirXZ);
                    playerCtr.rigid.rotation = Quaternion.RotateTowards(playerCtr.transform.rotation, targetRot, 13.0f);
                    playerCtr.agent.stoppingDistance = Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position);
                    //playerCtr.agent.SetDestination(transform.position);
                    isMove = true;
                    if (performRangeAttack)
                    {
                        //Debug.Log("Attack The Minion");
                        //StartCoroutine(RangedAttackInterval());
                    }
                }
            }
        }
        else if (targetedItem != null)
        {
            if (Vector3.Distance(gameObject.transform.position, targetedItem.transform.position) > itemRange)
            {
                // Character Dir get
                var dir = new Vector3(playerCtr.agent.steeringTarget.x, playerCtr.transform.position.y, playerCtr.agent.steeringTarget.z) - playerCtr.transform.position;
                var dirXZ = new Vector3(dir.x, 0f, dir.z);
                Quaternion targetRot = Quaternion.LookRotation(dirXZ);
                playerCtr.rigid.rotation = Quaternion.RotateTowards(playerCtr.transform.rotation, targetRot, 13.0f);

                playerCtr.agent.SetDestination(targetedItem.transform.position);
                playerCtr.agent.stoppingDistance = itemRange;
                isMove = true;
            }
        }

        // Character move stop
        if (playerCtr.agent.velocity.magnitude == 0f)
        {
            isMove = false;
            return;
        }
       

    }
    
}
