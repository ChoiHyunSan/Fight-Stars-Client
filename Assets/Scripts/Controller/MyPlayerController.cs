using Google.Protobuf.Protocol;
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

    public override void FixedUpdate()
    {
        base.FixedUpdate();
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
            // �������� �ٲ�
            SendMoveCommandToServer(currentInput.normalized);
            Debug.Log($"Input: {currentInput}");
            _previousInput = currentInput;
        }
        else if (currentInput.sqrMagnitude <= 0.01f && _previousInput.sqrMagnitude > 0.01f)
        {
            // �Է��� �����(����) -> ���� ��Ŷ ����
            SendMoveCommandToServer(Vector2.zero);
            Debug.Log("Stop Input sent");
            _previousInput = Vector2.zero;
        }
    }

    void SendMoveCommandToServer(Vector2 dir)
    {
        C_Move movePacket = new C_Move
        {
            UserId = UserDataManager.Instance._userInfo.userId,
            Dx = dir.x,
            Dy = dir.y
        };
        NetworkManager.Instance.Send(movePacket);
    }

    void SendAttackCommandToServer()
    {
        // TODO: ��Ʈ��ũ ��⿡ ���� ��� ����
    }
}
