using UnityEngine;

public class MyPlayerController : PlayerController
{
    [Header("Input Settings")]
    public Joystick movementJoystick;

    private Vector2 _previousInput;

    private bool _controllFlag = false;

    protected override void Awake()
    {
        base.Awake();
        movementJoystick = GameObject.Find("JoyStick_Move")?.GetComponent<Joystick>();
    }

    void Update()
    {
        if (!_controllFlag)
        { 
            return;
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.A))
        {
            SendAttackCommandToServer();
            Attack();
        }
#endif

        HandleInput();
    }

    public void ActiveControll()
    {
        _controllFlag = true;
    }

    void HandleInput()
    {
        Vector2 currentInput = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);

        if ((currentInput - _previousInput).sqrMagnitude > 0.01f)
        {
            SendMoveCommandToServer(currentInput.normalized);
            Debug.Log($"Input: {currentInput}");
            _previousInput = currentInput;
        }
    }

    void SendMoveCommandToServer(Vector2 dir)
    {
        // TODO: 네트워크 모듈에 입력 방향 전송
    }

    void SendAttackCommandToServer()
    {
        // TODO: 네트워크 모듈에 공격 명령 전송
    }
}
