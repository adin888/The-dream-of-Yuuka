using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.M_InputHandler.UseJumpInput();
        player.JumpState.ResetAmountOfJumpsLeft();
        M_Movement?.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        M_Movement?.FlipCheck(wallJumpDirection);
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.M_Anim.SetFloat("yVelocity", M_Movement.CurrentVelocity.y);
        player.M_Anim.SetFloat("xVelocity", Mathf.Abs(M_Movement.CurrentVelocity.x));

        if(Time.time>=startTime+playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if(isTouchingWall)
        {
            wallJumpDirection = -M_Movement.FacingDiraction;
        }
        else
        {
            wallJumpDirection = M_Movement.FacingDiraction;
        }
    }
}
