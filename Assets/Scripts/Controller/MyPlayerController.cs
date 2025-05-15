using Google.Protobuf.Protocol;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyPlayerController : PlayerController
{
    [Header("Input Settings")]
    public Joystick movementJoystick;
    public Joystick attackJoystick;

    private Vector2 _previousInput;

    private bool _controllFlag = false;
    private bool _attackFlag = false;

    public GameObject attackGuide;

    protected override void Awake()
    {
        base.Awake();
        movementJoystick = GameObject.Find("JoyStick_Move")?.GetComponent<Joystick>();
        if (movementJoystick == null)
        {
            Debug.LogError("Joystick_Move not found");
        }

        attackJoystick = GameObject.Find("JoyStick_Attack")?.GetComponent<AttackJoystick>();
        if (attackJoystick == null)
        {
            Debug.LogError("Joystick_Attack not found");
        }
        attackJoystick.GetComponent<AttackJoystick>().SetPlayerController(this);
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
            base.Attack();
        }
#endif

        HandleInput();
        HandleAttackGuide();
    }

    private void HandleAttackGuide()
    {
        if(_attackFlag == false || attackJoystick == null)
        {
            return;
        }

        // TODO : ����ü ������ ���̽�ƽ �������� ���̵尡 ���̰� ����
        Vector2 attackDirection = attackJoystick.Direction;

        // ���� ���
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        attackGuide.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
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

    public void SendAttackCommandToServer(PointerEventData eventData)
    {
        // TODO: ��Ʈ��ũ ��⿡ ���� ��� ����

    }

    public void DisplayGuide(bool v)
    {
        // ���� ���̵� ����
#if UNITY_EDITOR
        Debug.Log($"DisplayGuide: {v}");
#endif
        _attackFlag = v;
        attackGuide.SetActive(v); 
    }

    public void Attack(PointerEventData eventData)
    {
#if UNITY_EDITOR
        Debug.Log("Attack called");
#endif
        base.Attack();
    }
}
