using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Animator M_Anim { get; private set; }
    public Rigidbody2D M_RB { get; private set; }
    public BoxCollider2D M_Collider { get; private set; }
    public InputHandler M_InputHandler { get; private set; }
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
    }
    private void Start()
    {
        M_Anim = GetComponent<Animator>();
        M_RB = GetComponent<Rigidbody2D>();
        M_Collider = GetComponent<BoxCollider2D>();
        M_InputHandler = GetComponent<InputHandler>();


        StateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion
}
