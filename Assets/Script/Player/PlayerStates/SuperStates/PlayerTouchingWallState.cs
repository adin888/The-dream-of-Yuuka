using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected Movement M_Movement { get => m_Movement ?? m_Core.GetCoreComponent(ref m_Movement); }
    private CollisionSenses M_CollisionSenses { get => m_CollisionSenses ?? m_Core.GetCoreComponent(ref m_CollisionSenses); }

    private Movement m_Movement;
    private CollisionSenses m_CollisionSenses;

    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool grabInput;
    protected bool jumpInput;
    protected bool isTouchingLedge;
    protected int xInput;
    protected int yInput;
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if(M_CollisionSenses)
        {
            isGrounded = M_CollisionSenses.Ground;
            isTouchingWall = M_CollisionSenses.WallFront;
            isTouchingLedge = M_CollisionSenses.HorizontalLedge;
        }

        if(isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
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

        if(jumpInput)
        {
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if(isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if(!isTouchingWall || (xInput != M_Movement?.FacingDiraction && !grabInput))
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if(isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
