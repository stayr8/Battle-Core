using UnityEngine.EventSystems;
using UnityEngine;

public class StandingState : State
{

    bool isMove;
    public bool attack;
    public bool skillCheck;
    public bool skillCheck2;
    public bool skillCheck3;
    public bool skillCheck4;
    public bool allskillcheck;


    public Vector3 Dir;

    public StandingState(PlayerCtr _playerCtr, StateMachine _stateMachine) : base(_playerCtr, _stateMachine)
    {
        playerCtr = _playerCtr;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        isMove = false;
        attack = false;
        playerCtr.skill_1 = false;
        playerCtr.skill_2 = false;
        playerCtr.skill_3 = false;
        playerCtr.skill_4 = false;
        allskillcheck = false;

        skillCheck = false;
        // 플레이어 속도 받아옴.
        playerCtr.agent.speed = playerCtr.playerSpeed;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // 우 클릭시 위치
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            int layerMask = 1 << 7;

            if (Physics.Raycast(playerCtr.camera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
            {
                if (hit.transform.gameObject.tag != "UI")
                {
                    SetDestination(hit.point);
                    playerCtr.agent.stoppingDistance = 0;
                }
            }
        }

       

        // 좌 클릭시 공격
        if (Input.GetMouseButtonDown(0))
        {
            if(!allskillcheck)
                attack = true;

            RaycastHit hits;
            if (Physics.Raycast(playerCtr.camera.ScreenPointToRay(Input.mousePosition), out hits))
            {
                if (hits.transform.gameObject.tag != "UI")
                {
                    Dir = hits.point;
                }
            }
        }



        // Q 클릭
        if (Input.GetButtonDown("Skill_1"))
        {
            if (!playerCtr.skill_1)
            {
                skillCheck = true;
                allskillcheck = true;
                //skill = true;
            }

        }
        else if (Input.GetButtonDown("Skill_2"))
        {
            if (!playerCtr.skill_2)
            {
                skillCheck2 = true;
                allskillcheck = true;
                //skill = true;
            }

        }
        else if (Input.GetButtonDown("Skill_3"))
        {
            if (!playerCtr.skill_3)
            {
                skillCheck3 = true;
                allskillcheck = true;
                //skill = true;
            }

        }
        else if (Input.GetButtonDown("Skill_4"))
        {
            if (!playerCtr.skill_4)
            {
                skillCheck4 = true;
                allskillcheck = true;
                //skill = true;
            }

        }

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (playerCtr.heroCombat.targetedEnemy != null)
        {
            if (playerCtr.heroCombat.targetedEnemy.GetComponent<HeroCombat>() != null)
            {
                if (!playerCtr.heroCombat.targetedEnemy.GetComponent<HeroCombat>().isHeroAlive)
                {
                    playerCtr.heroCombat.targetedEnemy = null;
                }
            }
        }
        else if (playerCtr.heroCombat.targetedItem != null)
        {
            if (playerCtr.heroCombat.targetedItem.GetComponent<HeroCombat>() != null)
            {
                if (!playerCtr.heroCombat.targetedItem.GetComponent<HeroCombat>().isHeroAlive)
                {
                    playerCtr.heroCombat.targetedItem = null;
                }
            }
        }



        playerCtr.animator.SetFloat("speed", playerCtr.agent.velocity.magnitude);
        // Attack In
        if (attack)
        {
            playerCtr.agent.enabled = false;
            playerCtr.animator.SetTrigger("attack");
            stateMachine.ChangeState(playerCtr.attacking);
        }
        else if (playerCtr.skill_1)
        {
            playerCtr.agent.enabled = false;
            playerCtr.animator.SetTrigger("skill_1");
            skillCheck = false;
            allskillcheck = false;
            stateMachine.ChangeState(playerCtr.skilling);
        }
        else if (playerCtr.skill_2)
        {
            playerCtr.agent.enabled = false;
            playerCtr.animator.SetTrigger("skill_2");
            skillCheck2 = false;
            allskillcheck = false;
            stateMachine.ChangeState(playerCtr.skilling);
        }
        else if (playerCtr.skill_3)
        {
            playerCtr.agent.enabled = false;
            playerCtr.animator.SetTrigger("skill_3");
            skillCheck3 = false;
            allskillcheck = false;
            stateMachine.ChangeState(playerCtr.skilling);
        }
        else if (playerCtr.skill_4)
        {
            playerCtr.agent.enabled = false;
            playerCtr.animator.SetTrigger("skill_4");
            skillCheck4 = false;
            allskillcheck = false;
            stateMachine.ChangeState(playerCtr.skilling);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


        // Character move stop
        if (playerCtr.agent.velocity.magnitude == 0f)
        {
            isMove = false;
            return;
        }
        // Character Dir get
        var dir = new Vector3(playerCtr.agent.steeringTarget.x, playerCtr.transform.position.y, playerCtr.agent.steeringTarget.z) - playerCtr.transform.position;
        var dirXZ = new Vector3(dir.x, 0f, dir.z);
        Quaternion targetRot = Quaternion.LookRotation(dirXZ);
        playerCtr.rigid.rotation = Quaternion.RotateTowards(playerCtr.transform.rotation, targetRot, 13.0f);



    }

    // Character Nev Move
    private void SetDestination(Vector3 dest)
    {
        playerCtr.agent.SetDestination(dest);
        isMove = true;
    }


    public override void Exit()
    {
        base.Exit();
    }

}
