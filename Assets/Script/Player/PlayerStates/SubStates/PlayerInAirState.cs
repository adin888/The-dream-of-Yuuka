using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    protected Movement M_Movement
    {
        get => m_Movement ?? m_Core.GetCoreComponent(ref m_Movement);
    }
    protected CollisionSenses M_CollisionSenses
   {
        get => m_CollisionSenses ?? m_Core.GetCoreComponent(ref m_CollisionSenses);
    }

    private Movement m_Movement;
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

  
}
