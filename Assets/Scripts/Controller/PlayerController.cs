using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Sync Settings")]
    public float smoothingFactor = 10f;

    protected Rigidbody2D _rb;
    protected PlayerAnimationFSM _playerAnimationFSM;

    protected Vector2 _targetPosition;
    protected Vector2 _currentVelocity;

    public bool isServerUpdateReceived = false;

    protected virtual void Awake()
    {
        // Physics/Rigidbody2D 찾기
        _rb = transform.Find("Physics")?.GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.LogError("Rigidbody2D not found under 'Physics'");

        // Visual/PlayerAnimationFSM 찾기
        _playerAnimationFSM = transform.Find("Visual")?.GetComponent<PlayerAnimationFSM>();
        if (_playerAnimationFSM == null) Debug.LogError("PlayerAnimationFSM not found under 'Visual'");
    }

    public virtual void FixedUpdate()
    {
        if (!isServerUpdateReceived) { 
            return;
        }

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * 10f);
    }

    public virtual void Attack()
    {
        _playerAnimationFSM?.PlayAttack();
    }

    public void SetPosition(Vector3 newPosition, Vector2 newVelocity)
    {
        _targetPosition = newPosition;
        Debug.Log($"SetPosition: {_targetPosition}");

        _playerAnimationFSM.SetVelocity(newVelocity);
    }

    public void OnDead()
    {
        PlayerInfo playerInfo = gameObject.GetComponent<PlayerInfo>();
        if (playerInfo != null)
        {
            playerInfo.UpdateHp(0);
        }

        // 투명하게 처리
        _playerAnimationFSM.SetAlpha(0.0f);
    }

    public void OnRespawn()
    {
        // 플레이어 체력 초기화
        PlayerInfo playerInfo = gameObject.GetComponent<PlayerInfo>();
        if (playerInfo != null)
        {
            playerInfo.UpdateHp(playerInfo.maxHp);
        }

        // 불투명하게 처리
        _playerAnimationFSM.SetAlpha(1.0f);
    }
}
