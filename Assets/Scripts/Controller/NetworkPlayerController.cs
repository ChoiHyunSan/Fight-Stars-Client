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

    public override void ApplyServerMovement(Vector2 newPosition, Vector2 newVelocity)
    {
        base.ApplyServerMovement(newPosition, newVelocity);

        _lastReceivedPosition = newPosition;
        _lastReceivedVelocity = newVelocity;
        _lastSyncTime = Time.time;
    }

    protected override void FixedUpdate()
    {
        if (!isServerUpdateReceived) return;

        // 보간을 사용하여 부드러운 이동 처리
        float t = Mathf.Clamp01((Time.time - _lastSyncTime) * smoothingFactor);
        _rb.position = Vector2.Lerp(_rb.position, _lastReceivedPosition, t);
        _rb.velocity = _lastReceivedVelocity;
    }

    public override void Attack()
    {
        // 서버에서 전파된 공격 명령 수신 후 호출됨
        _playerAnimationFSM?.PlayAttack();
    }
}
