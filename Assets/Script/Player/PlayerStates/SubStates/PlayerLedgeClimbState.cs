
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
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

    private Vector2 detectedPos;
    private Vector2 corenerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 workspace;

    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;
    private bool isTouchingCeiling;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        M_Movement?.SetVelocityToZero();
        player.transform.position = detectedPos;
        corenerPos = DetermineCornerPosition();

        startPos.Set(corenerPos.x - (M_Movement.FacingDiraction * playerData.startOffset.x), corenerPos.y - playerData.startOffset.y);
        stopPos.Set(corenerPos.x + (M_Movement.FacingDiraction * playerData.startOffset.x), corenerPos.y + playerData.startOffset.y);

        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if(isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isTouchingCeiling)
            {
                //stateMachine.ChangeState(player.CrouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {
            xInput = player.M_InputHandler.NormInputX;
            yInput = player.M_InputHandler.NormInputY;
            jumpInput = player.M_InputHandler.JumpInput;

            M_Movement?.SetVelocityToZero();
            player.transform.position = startPos;

            if(xInput==M_Movement.FacingDiraction && isHanging && !isClimbing)
            {
                CheckForSpace();
                isClimbing = true;
                player.M_Anim.SetBool("climbLedge", true);
            }
            else if(yInput==-1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if(jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.M_Anim.SetBool("climbLedge", false);
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

    private void CheckForSpace()
    {
        isTouchingCeiling = Physics2D.Raycast(corenerPos + (Vector2.up * 0.015f) + (Vector2.right * M_Movement.FacingDiraction * 0.015f), 
            Vector2.up, playerData.standColliderHeight, M_CollisionSenses.WhatIsGround);
        player.M_Anim.SetBool("isTouchingCeiling", isTouchingCeiling);
    }
    private Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(M_CollisionSenses.WallCheck.position,
            Vector2.right * M_Movement.FacingDiraction, M_CollisionSenses.WallCheckDistance, M_CollisionSenses.WhatIsGround);

        float xDist = xHit.distance;
        workspace.Set((xDist + 0.015f) * M_Movement.FacingDiraction, 0f);

        RaycastHit2D yHit = Physics2D.Raycast(M_CollisionSenses.HorizontalLedgeCheck.position + (Vector3)workspace, Vector2.down,
            M_CollisionSenses.HorizontalLedgeCheck.position.y - M_CollisionSenses.WallCheck.position.y + 0.015f, 
            M_CollisionSenses.WhatIsGround);

        float yDist = yHit.distance;

        workspace.Set(M_CollisionSenses.WallCheck.position.x + 
            (xDist * M_Movement.FacingDiraction), M_CollisionSenses.HorizontalLedgeCheck.position.y - yDist);

        return workspace;
    }
}
