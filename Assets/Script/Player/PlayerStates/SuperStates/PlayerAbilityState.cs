using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;
    private bool isGrounded;

    protected Movement M_Movement { get => m_Movement ?? m_Core.GetCoreComponent(ref m_Movement); }
    private CollisionSenses M_CollisionSenses { get => m_CollisionSenses ?? m_Core.GetCoreComponent(ref m_CollisionSenses); }

    private Movement m_Movement;
    private CollisionSenses m_CollisionSenses;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAbilityDone)
        {
            if(isGrounded&&M_Movement?.CurrentVelocity.y<0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else 
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if(M_CollisionSenses)
        {
            isGrounded = M_CollisionSenses.Ground;     
        }
    }
}
