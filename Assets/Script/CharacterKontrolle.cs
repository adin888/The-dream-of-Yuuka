using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterKontrolle : MonoBehaviour
{

    private float movementInputDirection;
    private float movementInput;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private float slowDownSpeed;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;
    private bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    private bool isDashing;

    private Vector2 ledgePosBot; //��⵽ǽ�ǵ�ʱ���õ�ǰwallCheck���ӵ�λ��
    private Vector2 ledgePos1;   //������ʼʱ�����λ��
    private Vector2 ledgePos2;    //��������ʱ�����λ��

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;

    public bool canWallSliding;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;             //ʹ��ɫ�ڿ���Ҳ�ܵ����������Ĵ�С��������������,������
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;            //��ǽ������ʱ���ķ���HopΪ�ɿ�ǽ�棬JumpΪ��ǽ���ٴ�������ǰ�߷���Ӧб���£�����б���ϣ�Hop������
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;    //����ʱ�䣬Ĭ�����������ǰ0.15s�ڰ�����ԾҲ����غ�ɹ�����
    public float turnTimerSet = 0.1f;     //ͬ�ϣ��ݲ�ʱ��Ĭ��0.1s
    public float wallJumpTimerSet = 0.5f;   //ʹ������Խ���һ��ǽ���ߣ���wallJump��0.5s�ڸı䷽������Զ��
    public float ledgeClimbXOffset1 = 0f;   //���￪ʼ����ʱ������ǽ������߽ǵ�ƫ����
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;   //�����������ʱ������ǽ������߽ǵ�ƫ����������һ�����Ĳ�ֵ�������Ϊ����ǰ���λ��
    public float ledgeClimbYOffset2 = 0f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;


    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
        CheckDash();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding() //����ɫ�Ƿ���ǽ���ϻ���
    {
        if (!canWallSliding)
            return;
        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0 && !canClimbLedge)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;
            //�·��ļ��㱾����ͨ�����ϻ�����ȡ����ü�⵽��ǽ������ı�Ե�㣬Ȼ�����Ԥ���ƫ����������ʱ�����ﶨλ
            if (isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else //��������������㷽ʽ�෴
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;

            anim.SetBool("canClimbLedge", canClimbLedge);
        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }
    //���������Ҫ�ڶ�����/�����н��е��ã�Ҳ���Ƕ������Ž���ʱ�Զ�ִ�У���Ϊһ֡
    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimbLedge", canClimbLedge);
    }
    private void CheckSurroundings() //����ɫ�Ƿ��ڵ��棬�Ƿ���ǽ��Ӵ�
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);
        //������������ߣ��·���wallcheck��⵽���Ϸ���ledgecheckû��⵽��˵��������ǽ��
        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void CheckIfCanJump() //����Ƿ������Ծ�������趨�����Ծ����
    {
        //�����Ӵ�ǽ��ʱ����
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }

    }

    private void CheckMovementDirection()
    {
        if (Mathf.Abs(movementInputDirection) > Mathf.Epsilon && Mathf.Sign(movementInputDirection) == facingDirection)
            isWalking = true;

        else
            isWalking = false;

        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        //if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        //{
        //    iswalking = true;
        //}
        //else
        //{
        //    iswalking = false;
        //}
    }

    private void UpdateAnimations()
    {
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("OnGround", isGrounded);
        anim.SetFloat("AirSpeedY", rb.velocity.y);
        //anim.SetBool("isWallSliding", isWallSliding);
    }

    private void CheckInput()

    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        movementInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            //�����ؾͿ�����������������ʼһ����ʱ����������ʱ��
            if (isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }
        //�����ǽ��һ���ݴ�ʱ�䣨ת��ʱ�䣩
        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCoolDown))
                AttemptToDash();
        }

    }
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, 0.0f);
                dashTimeLeft -= Time.deltaTime;
                //��Ӱ��������ͼ����µĲ�Ӱ
                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }

        }
    }
    private void CheckJump()
    {   
        if (jumpTimer > 0)
        {
            //WallJump
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
        }
        //��ǽ��ֱ������
        //else if (isWallSliding && movementInputDirection == 0 && canJump) //Wall hop
        //{
        //    isWallSliding = false;
        //    amountOfJumpsLeft--;
        //    Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
        //    rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        //}
        //��ǽ���ٴ�����
        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }
    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;

        }
    }
    private void ApplyMovement()
    {
        slowDownSpeed = isWalking ? 1.0f : 0.5f;        //����ת��ʱ���ٶȼ���

        //ģ���������
        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if(canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInput * slowDownSpeed, rb.velocity.y);
        }
        //������ã����ڿ��и������һ����ʹ���ƶ�����������������
        //else if (!isGrounded && !isWallSliding && movementInputDirection != 0)
        //{
        //    Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
        //    rb.AddForce(forceToAdd);

        //    if (Mathf.Abs(rb.velocity.x) > movementSpeed)
        //    {
        //        rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        //    }
        //}

        //��ǽ���ϻ���
        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }
    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }
    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
