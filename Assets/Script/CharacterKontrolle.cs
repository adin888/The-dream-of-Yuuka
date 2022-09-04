using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterKontrolle : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float m_Speed = 4.5f;
    [SerializeField] float m_jumpForce = 7.5f;

    Rigidbody2D m_rigidbody2d;
    Animator m_animator;
    GroundSensor m_groundSensor;
    float inputX;
    float inputRaw;
    float m_disableMovementTimer = 0.0f;
    int m_facingDirection = 1;
    bool isOnGround = true;
    bool m_moving;
    public bool onGround { get { return isOnGround; } }
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        m_disableMovementTimer -= Time.deltaTime;

        //Check if character just landed on the ground
        if (!isOnGround && m_groundSensor.State())
        {
            isOnGround = true;
            m_animator.SetBool("OnGround", isOnGround);
        }

        //Check if character just started falling
        if (isOnGround && !m_groundSensor.State())
        {
            isOnGround = false;
            m_animator.SetBool("OnGround", isOnGround);
        }

        if (m_disableMovementTimer < 0.0f)
            inputX = Input.GetAxis("Horizontal");

        inputRaw = Input.GetAxisRaw("Horizontal");

        // Check if current move input is larger than 0 and the move direction is equal to the characters facing direction
        if (Mathf.Abs(inputRaw) > Mathf.Epsilon && Mathf.Sign(inputRaw) == m_facingDirection)
            m_moving = true;

        else
            m_moving = false;

        // Swap direction of sprite depending on move direction
        if (inputRaw > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }

        else if (inputRaw < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // SlowDownSpeed helps decelerate the characters when stopping
        float SlowDownSpeed = m_moving ? 1.0f : 0.5f;
        // Set movement
        m_rigidbody2d.velocity = new Vector2(inputX * m_Speed * SlowDownSpeed, m_rigidbody2d.velocity.y);

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_rigidbody2d.velocity.y);

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            m_rigidbody2d.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
            m_animator.SetBool("OnGround", isOnGround);
            m_groundSensor.Disable(0.2f);
        }
        else if(m_moving)
        {
            m_animator.SetBool("Move", true);
        }
        else
        {
            m_animator.SetBool("Move", false);
        }

    }
}
