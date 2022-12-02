using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewPlayerData", menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 15f;

    [Header("Jump State")]

    [Header("Wall Jump State")]

    [Header("In Air State")]

    [Header("Wall Slide State")]
    public float wallFlideVelocity = 3f;
}
