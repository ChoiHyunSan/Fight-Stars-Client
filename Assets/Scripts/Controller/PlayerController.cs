using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Sync Settings")]
    public float smoothingFactor = 10f;

    protected Rigidbody2D _rb;
    protected PlayerAnimationFSM _playerAnimationFSM;

    protected Vector2 _targetPosition;
    protected Vector2 _currentVelocity;

    protected bool isServerUpdateReceived = false;

    protected virtual void Awake()
    {
        // Physics/Rigidbody2D 찾기
        _rb = transform.Find("Physics")?.GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.LogError("Rigidbody2D not found under 'Physics'");

        // Visual/PlayerAnimationFSM 찾기
        _playerAnimationFSM = transform.Find("Visual")?.GetComponent<PlayerAnimationFSM>();
        if (_playerAnimationFSM == null) Debug.LogError("PlayerAnimationFSM not found under 'Visual'");
    }

    protected virtual void FixedUpdate()
    {
        if (!isServerUpdateReceived) return;

        _rb.position = Vector2.Lerp(_rb.position, _targetPosition, Time.fixedDeltaTime * smoothingFactor);
        _rb.velocity = _currentVelocity;
    }

    public virtual void ApplyServerMovement(Vector2 newPosition, Vector2 newVelocity)
    {
        _targetPosition = newPosition;
        _currentVelocity = newVelocity;
        isServerUpdateReceived = true;
    }

    public virtual void Attack()
    {
        _playerAnimationFSM?.PlayAttack();
    }
}
