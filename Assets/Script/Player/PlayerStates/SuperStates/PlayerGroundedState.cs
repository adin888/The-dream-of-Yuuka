using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected Movement M_Movement
    {
        get => m_Movement ?? m_Core.GetCoreComponent(ref m_Movement);
    }
    public Movement m_Movement;

    protected CollisionSenses M_CollisionSenses
    {
        get => m_CollisionSenses ?? m_Core.GetCoreComponent(ref m_CollisionSenses);
    }
    public CollisionSenses m_CollisionSenses;

    private bool jumpInput;
    private bool grabInput;
    private bool dashInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;

    protected bool isTouchingCeiling;
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, 
        string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.M_InputHandler.NormInputX;
        yInput = player.M_InputHandler.NormInputY;
        jumpInput = player.M_InputHandler.JumpInput;
        grabInput = player.M_InputHandler.GrabInput;
        dashInput = player.M_InputHandler.DashInput;

        if(jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if(isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if(dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
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
            isTouchingWall = M_CollisionSenses.WallFront;
            isTouchingLedge = M_CollisionSenses.HorizontalLedge;
            isTouchingCeiling = M_CollisionSenses.Ceiling;
        }
    }
}
