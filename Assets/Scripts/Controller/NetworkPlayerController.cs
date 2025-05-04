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
        base.FixedUpdate();
    }

    public override void Attack()
    {
        // �������� ���ĵ� ���� ��� ���� �� ȣ���
        _playerAnimationFSM?.PlayAttack();
    }
}
