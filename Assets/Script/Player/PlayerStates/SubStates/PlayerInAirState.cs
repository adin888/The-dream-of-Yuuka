using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    protected Movement M_Movement
    {
        get => m_Movement ?? m_Core.GetCoreComponent(ref m_Movement);
    }

    private Movement m_Movement;
    private CollisionSenses M_CollisionSenses
   {
        get => m_CollisionSenses ?? m_Core.GetCoreComponent(ref m_CollisionSenses);
    }

    private CollisionSenses m_CollisionSenses;
    #region Input
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;
    #endregion

    #region Check
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;

    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool isJumping;
    #endregion

    private float startWallJumpCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        xInput = player.M_InputHandler.NormInputX;
        jumpInput = player.M_InputHandler.JumpInput;
        jumpInputStop = player.M_InputHandler.JumpInputStop;
        grabInput = player.M_InputHandler.GrabInput;
        dashInput = player.M_InputHandler.DashInput;

        CheckJumpMultiplier();

        if(isGrounded && M_Movement?.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if(isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            //stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = M_CollisionSenses.WallFront;
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(isTouchingWall && grabInput && isTouchingLedge)
        {
            //stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && xInput == M_Movement?.FacingDiraction && M_Movement?.CurrentVelocity.y <= 0)
        {
            //stateMachine.ChangeState(player.WallSlideState);
        }
        //else if (dashInput && player.DashState.CheckIfCanDash())
        else
        {
            M_Movement?.FlipCheck(xInput);
            M_Movement?.SetVelocityX(playerData.movementVelocity * xInput);

            player.M_Anim.SetFloat("yVelocity", M_Movement.CurrentVelocity.y);
            player.M_Anim.SetFloat("xVelocity", Mathf.Abs(M_Movement.CurrentVelocity.x));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        if(M_CollisionSenses)
        {
            isGrounded = M_CollisionSenses.Ground;
            isTouchingWall = M_CollisionSenses.WallFront;
            isTouchingWallBack = M_CollisionSenses.WallBack;
            isTouchingLedge = M_CollisionSenses.HorizontalLedge;
        }

        if(isTouchingWall && isTouchingLedge)
        {
            //player.LedgeClimbState
        }

        if(!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }
    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime+playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }

    private void CheckJumpMultiplier()
    {
        if(isJumping)
        {
            if(jumpInputStop)
            {
                M_Movement?.SetVelocityY(M_Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
            }
            else if(M_Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;
}
