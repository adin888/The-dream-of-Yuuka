using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core m_Core;
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isExitingState;
    protected bool isAnimationFinished;

    protected float startTime;

    private string animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
        m_Core = player.M_Core;
    }
    public virtual void Enter()
    {
        DoChecks();
        player.M_Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }
    public virtual void Exit()
    {
        player.M_Anim.SetBool(animBoolName, false);
        isExitingState = true;

    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks(){ }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
