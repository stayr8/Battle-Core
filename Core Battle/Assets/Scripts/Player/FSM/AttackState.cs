using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    float timePassed;
    float clipLength;
    float clipSpeed;
    bool attack;

    private Vector3 AttackDir;


    public AttackState(PlayerCtr _playerCtr, StateMachine _stateMachine) : base(_playerCtr, _stateMachine)
    {
        playerCtr = _playerCtr;
        stateMachine = _stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

        attack = false;
        timePassed = 0f;
        
        // 공격 시 이동 중지
        playerCtr.animator.SetFloat("speed", 0);
        // 공격 방향 위치 값 get
        if (playerCtr.standing.attack)
        {
            AttackDir = playerCtr.standing.Dir;
            playerCtr.standing.attack = false;
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            int layerMask = 1 << 7;

            if (Physics.Raycast(playerCtr.camera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
            {
                if (hit.transform.gameObject.tag != "UI" && hit.transform.gameObject.tag != "Enemy")
                {
                    playerCtr.agent.SetDestination(hit.point);
                    playerCtr.agent.stoppingDistance = 0;
                }
            }
        }
        // 좌 클릭시 다음 공격
        if (Input.GetMouseButtonDown(0) && 0.3f <= timePassed)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCtr.camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                AttackDir = hit.point; 
            }
            attack = true;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timePassed += Time.deltaTime;
        clipLength = playerCtr.animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
        clipSpeed = playerCtr.animator.GetCurrentAnimatorStateInfo(1).speed;
        // 애니메이션 시간동안 좌클릭시 실행
        if(timePassed >= clipLength / clipSpeed && attack)
        {
            playerCtr.animator.SetTrigger("attack");
            stateMachine.ChangeState(playerCtr.attacking);
        }
        // 애니메이션 시간 끝나면 다시 Standing
        if(timePassed >= clipLength / clipSpeed)
        {
            playerCtr.agent.enabled = true;
            stateMachine.ChangeState(playerCtr.standing);
            playerCtr.animator.SetTrigger("move");
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // 방향 계속 최신화
        var dir = AttackDir - playerCtr.transform.position;
        var dirXZ = new Vector3(dir.x, 0f, dir.z);
        Quaternion targetRot = Quaternion.LookRotation(dirXZ);
        playerCtr.rigid.rotation = Quaternion.RotateTowards(playerCtr.transform.rotation, targetRot, 13.0f);
        
    }
}
