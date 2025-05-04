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
        // 서버에서 전파된 공격 명령 수신 후 호출됨
        _playerAnimationFSM?.PlayAttack();
    }
}
