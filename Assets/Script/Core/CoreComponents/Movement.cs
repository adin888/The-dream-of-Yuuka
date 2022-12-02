using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D M_RB { get; private set; }

    public int FacingDiraction { get; private set; }

    public bool CanSetVelocity { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        M_RB = GetComponentInParent<Rigidbody2D>();

        FacingDiraction = 1;
        CanSetVelocity = true;
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = M_RB.velocity;
    }
    #region Funktions
    private void SetFinalVelocity()
    {
        if(CanSetVelocity)
        {
            M_RB.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }

    public void SetVelocityToZero()
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetFinalVelocity();
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        SetFinalVelocity();
    }

    public void Flip()
    {
        FacingDiraction *= -1;
        M_RB.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    public void FlipCheck(int xInput)
    {
        if(xInput!=0&&xInput!=FacingDiraction)
        {
            Flip();
        }
    }
    #endregion
}
