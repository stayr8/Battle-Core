using UnityEngine;

public class State
{
    public PlayerCtr playerCtr;
    public StateMachine stateMachine;

    protected Vector3 velocity;


    public State(PlayerCtr _playerCtr, StateMachine _stateMachine)
    {
        playerCtr = _playerCtr;
        stateMachine = _stateMachine;      

    }

    public virtual void Enter()
    {
        Debug.Log("enter state: " + this.ToString());
    }

    public virtual void HandleInput()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
