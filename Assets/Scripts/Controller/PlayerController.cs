using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float _moveSpeed = 5.0f;

    [Header("Input Settings")]
    public Joystick movementJoystick;

    public Rigidbody2D rb;
    private Vector2 _movementInput;
    private Vector2 _serverPosition;
    private bool isPositionCorrected = false;

    private PlayerAnimationFSM playerAnimationFSM;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimationFSM = GetComponent<PlayerAnimationFSM>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerAnimationFSM.PlayAttack();
        }
#endif
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (isPositionCorrected)
        {
            ApplyServerCorrection();
        }
        else
        {
            Move();
        }
    }

    void HandleInput()
    {
        _movementInput = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);

        if (_movementInput.magnitude > 1)
            _movementInput = _movementInput.normalized;
    }

    void Move()
    {
        rb.velocity = _movementInput * _moveSpeed;
    }

    void ApplyServerCorrection()
    {
        rb.position = _serverPosition;
        rb.velocity = Vector2.zero;
        isPositionCorrected = false;
    }
}
