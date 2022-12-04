using UnityEngine;

public class SkillState : State
{
    float timePassed;
    float clipLength;
    float clipSpeed;

    public SkillState(PlayerCtr _playerCtr, StateMachine _stateMachine) : base(_playerCtr, _stateMachine)
    {
        playerCtr = _playerCtr;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        timePassed = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timePassed += Time.deltaTime;
        clipLength = playerCtr.animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
        clipSpeed = playerCtr.animator.GetCurrentAnimatorStateInfo(1).speed;

        // 애니메이션 시간 끝나면 다시 Standing
        if (timePassed >= clipLength / clipSpeed)
        {
            playerCtr.agent.enabled = true;
            stateMachine.ChangeState(playerCtr.standing);
            playerCtr.animator.SetTrigger("move");
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
