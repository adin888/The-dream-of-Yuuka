using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerAbilityState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
}
