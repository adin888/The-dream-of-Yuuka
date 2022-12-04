using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    private Movement M_Movement { get => m_Movement ?? m_Core.GetCoreComponent(ref m_Movement); }
    private Movement m_Movement;

    #region Check Transforms

    public Transform GroundCheck
    {
        get => GenericNotImplementedErro<Transform>.TryGet(groundCheck, m_Core.transform.parent.name);
        private set => groundCheck = value;
    }
    public Transform WallCheck
    {
        get => GenericNotImplementedErro<Transform>.TryGet(wallCheck, m_Core.transform.parent.name);
        private set => wallCheck = value;
    }
    public Transform HorizontalLedgeCheck
    {
        get => GenericNotImplementedErro<Transform>.TryGet(HorizontalLedgeCheck, m_Core.transform.parent.name);
        private set => horizontalLedgeCheck = value;
    }
    public Transform VerticalLedgeCheck
    {
        get => GenericNotImplementedErro<Transform>.TryGet(verticalLedgeCheck, m_Core.transform.parent.name);
        private set => verticalLedgeCheck = value;
    }
    public Transform CeilingCheck
    {
        get => GenericNotImplementedErro<Transform>.TryGet(ceilingCheck, m_Core.transform.parent.name);
        private set => ceilingCheck = value;
    }

    public float GroundCheckRadius { get => groundCheckRadius; private set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; private set => wallCheckDistance = value; }

    public LayerMask WhatIsGround { get => whatIsGround; private set => whatIsGround = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform horizontalLedgeCheck;
    [SerializeField] private Transform verticalLedgeCheck;
    [SerializeField] private Transform ceilingCheck;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    #endregion

    public bool Ceiling
    {
        get => Physics2D.OverlapCircle(CeilingCheck.position, GroundCheckRadius, WhatIsGround);
    }
    public bool Ground
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatIsGround);
    }
    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * M_Movement.FacingDiraction, WallCheckDistance, WhatIsGround);
    }
    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -M_Movement.FacingDiraction, WallCheckDistance, WhatIsGround);
    }
    public bool HorizontalLedge
    {
        get => Physics2D.Raycast(HorizontalLedgeCheck.position, Vector2.right * M_Movement.FacingDiraction, WallCheckDistance, WhatIsGround);
    }
    public bool VerticalLedge
    {
        get => Physics2D.Raycast(VerticalLedgeCheck.position, Vector2.down, WallCheckDistance, WhatIsGround);
    }
}
