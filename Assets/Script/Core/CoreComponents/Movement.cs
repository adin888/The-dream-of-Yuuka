using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D M_RB { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        M_RB = GetComponent<Rigidbody2D>();
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = M_RB.velocity;
    }

}
