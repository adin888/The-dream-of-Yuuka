using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;

    protected Movement M_Movement
    {
        get => m_Movement ?? m_Core.GetCoreComponent(ref m_Movement);
    }
    protected Movement m_Movement;

    private bool jumpInput;
    private bool grabInput;
    private bool dashInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
