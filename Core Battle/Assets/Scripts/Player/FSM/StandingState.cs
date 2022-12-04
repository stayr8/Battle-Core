using UnityEngine.EventSystems;
using UnityEngine;

public class StandingState : State
{
    
    bool isMove;
    public bool attack;
    public bool skill;
    

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
        skill = false;
        // 플레이어 속도 받아옴.
        playerCtr.agent.speed = playerCtr.playerSpeed;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if(EventSystem.current.IsPointerOverGameObject())
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
                if (hit.transform.gameObject.tag != "UI" || hit.transform.gameObject.tag != "Enemy")
                {
                    SetDestination(hit.point);
                }
            }
            if(Physics.Raycast(playerCtr.camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if(interactable != null)
                {
                    SetFocus(interactable, hit.point);
                }
            }
        }
        // 좌 클릭시 공격
        if (Input.GetMouseButtonDown(0))
        {
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
        if(Input.GetButtonDown("Skill_1"))
        {
            if(!skill)
            {
                skill = true;
            }
        }

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        playerCtr.animator.SetFloat("speed", playerCtr.agent.velocity.magnitude);
        // Attack In
        if(attack)
        {
            playerCtr.agent.enabled = false;
            playerCtr.animator.SetTrigger("attack");
            stateMachine.ChangeState(playerCtr.attacking);
        }
        else if(skill)
        {
            playerCtr.agent.enabled = false;
            playerCtr.animator.SetTrigger("skill_1");
            stateMachine.ChangeState(playerCtr.skilling);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isMove)
        {
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

    }

    // Character Nev Move
    private void SetDestination(Vector3 dest)
    {
        playerCtr.agent.SetDestination(dest);
        isMove = true;
    }


    void SetFocus(Interactable newFocus, Vector3 dest)
    {
        if(newFocus != playerCtr.focus)
        {
            if (playerCtr.focus != null)
            {
                playerCtr.focus.OnDefocused();
            }
            playerCtr.focus = newFocus;
            FollowTarget(newFocus);
            //playerCtr.agent.SetDestination(dest);
        }

        
        newFocus.OnFocused(playerCtr.transform);
        
    }

    void RemoveFocus()
    {
        if (playerCtr.focus != null)
        {
            playerCtr.focus.OnDefocused();
        }
        playerCtr.focus = null;
        StopFollowingTarget();

    }

    public void FollowTarget(Interactable newTarget)
    {
        playerCtr.agent.stoppingDistance = newTarget.radius * 0.6f;


        playerCtr.target = newTarget.interactionTransform;
        RemoveFocus();
    }

    public void StopFollowingTarget()
    {
        playerCtr.agent.stoppingDistance = 0f;


        playerCtr.target = null;
    }

    public override void Exit()
    {
        base.Exit();
    }

}
