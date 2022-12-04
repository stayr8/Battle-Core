using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : MonoBehaviour
{
    public enum HeroAttackType { Melee, Ranged};
    public HeroAttackType heroAttackType;

    public GameObject targetedEnemy;
    public float attackRange;
    public float rotateSpeedForAttack;

    private PlayerCtr playerCtr;

    public bool basicAtkIdle = false;
    public bool isHeroAlive;
    public bool performMeleeAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        playerCtr = GetComponent<PlayerCtr>();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetedEnemy != null)
        {
            if(Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position) > attackRange)
            {
                playerCtr.agent.SetDestination(targetedEnemy.transform.position);
                playerCtr.agent.stoppingDistance = attackRange;

                // Character Dir get
                var dir = new Vector3(playerCtr.agent.steeringTarget.x, playerCtr.transform.position.y, playerCtr.agent.steeringTarget.z) - playerCtr.transform.position;
                var dirXZ = new Vector3(dir.x, 0f, dir.z);
                Quaternion targetRot = Quaternion.LookRotation(targetedEnemy.transform.position - dirXZ);
                playerCtr.rigid.rotation = Quaternion.RotateTowards(playerCtr.transform.rotation, targetRot, 13.0f);

            }
            else
            {
                if(heroAttackType == HeroAttackType.Melee)
                {
                    if(performMeleeAttack)
                    {
                        Debug.Log("Attack The Minion");
                        // Start Coroutine To Attack
                    }
                }
            }
        }
    }
}
