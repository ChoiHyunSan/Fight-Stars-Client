using UnityEngine;

public class NetworkPlayerController : PlayerController
{
    private Vector2 _lastReceivedPosition;
    private Vector2 _lastReceivedVelocity;
    private float _lastSyncTime;

    protected override void Awake()
    {
        base.Awake();
        _lastSyncTime = Time.time;
    }

    public override void FixedUpdate()
    {
        if (!isServerUpdateReceived) return;

        // ������ ����Ͽ� �ε巯�� �̵� ó��
        float t = Mathf.Clamp01((Time.time - _lastSyncTime) * smoothingFactor);
        _rb.position = Vector2.Lerp(_rb.position, _lastReceivedPosition, t);
        _rb.velocity = _lastReceivedVelocity;
    }

    public override void Attack()
    {
        // �������� ���ĵ� ���� ��� ���� �� ȣ���
        _playerAnimationFSM?.PlayAttack();
    }
}
