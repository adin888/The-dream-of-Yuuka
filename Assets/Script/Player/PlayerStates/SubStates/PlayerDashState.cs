using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    private bool isHolding;
    private bool dashInputStop;

    private float lastDashTime;

    private Vector2 dashDirection;
    private Vector2 dashDirectionInput;
    private Vector2 lastAIPos;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        CanDash = false;
        player.M_InputHandler.UseDashInput();

        isHolding = true;
        dashDirection = Vector2.right * M_Movement.FacingDiraction;

        Time.timeScale = playerData.holdTimeScale;
        startTime = Time.unscaledTime;

        player.DashDirectionIndicator.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        if(M_Movement?.CurrentVelocity.y > 0)
        {
            M_Movement?.SetVelocityY(M_Movement.CurrentVelocity.y * playerData.dashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isExitingState)
        {
            player.M_Anim.SetFloat("yVelocity", M_Movement.CurrentVelocity.y);
            player.M_Anim.SetFloat("xVelocity", Mathf.Abs(M_Movement.CurrentVelocity.x));

            if(isHolding)
            {
                dashDirectionInput = player.M_InputHandler.DashDirectionInput;
                dashInputStop = player.M_InputHandler.DashInputStop;

                if(dashDirectionInput!=Vector2.zero)
                {
                    dashDirection = dashDirectionInput;
                    dashDirection.Normalize();
                }

                float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
                player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

                if(dashInputStop || Time.unscaledTime >= startTime+playerData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;
                    M_Movement?.FlipCheck(Mathf.RoundToInt(dashDirection.x));
                    player.M_RB.drag = playerData.drag;
                    M_Movement?.SetVelocity(playerData.dashVelocity, dashDirection);
                    player.DashDirectionIndicator.gameObject.SetActive(false);
                    PlaceAfterImage();
                }
                else
                {
                    M_Movement?.SetVelocity(playerData.dashVelocity, dashDirection);
                    CheckIfShouldPlaceAfterImage();
                }

                if(Time.time >= startTime + playerData.dashTime)
                {
                    player.M_RB.drag = 0f;
                    isAbilityDone = true;
                    lastDashTime = Time.time;
                }
            }
        }
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if(Vector2.Distance(player.transform.position,lastAIPos)>=playerData.distBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        lastAIPos = player.transform.position;
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void ResetCanDash() => CanDash = true; 
}
