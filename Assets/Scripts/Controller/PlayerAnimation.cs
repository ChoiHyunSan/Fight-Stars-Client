using UnityEngine;
using Spine.Unity;
using Spine;

public class PlayerAnimationFSM : MonoBehaviour
{
    public GameObject attackEffectPrefab;

    private Rigidbody2D _rb;
    private SkeletonAnimation _skeletonAnimation;

    private PlayerState _currentState = PlayerState.Idle;
    private bool _isAttacking = false;

    private void Awake()
    {
        // 부모인 Physics에서 Rigidbody2D 찾아오기
        _rb = transform.parent.Find("Physics")?.GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.LogError("Rigidbody2D not found under 'Physics'");

        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void Update()
    {
        if (_isAttacking) return;

        Vector2 velocity = _rb.velocity;

        if (velocity.magnitude > 0.05f)
        {
            ChangeState(PlayerState.Move);

            if (velocity.x > 0)
                _skeletonAnimation.Skeleton.ScaleX = 1f;
            else if (velocity.x < 0)
                _skeletonAnimation.Skeleton.ScaleX = -1f;
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }

    private void ChangeState(PlayerState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                _skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                break;
            case PlayerState.Move:
                _skeletonAnimation.AnimationState.SetAnimation(0, "Run", true);
                break;
            case PlayerState.Attack:
                _skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
                _skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 0f);
                break;
        }
    }

    public void PlayAttack()
    {
        if (_isAttacking) return;

        _isAttacking = true;
        _skeletonAnimation.AnimationState.SetAnimation(1, "Attack", false);

        TrackEntry attackEntry = _skeletonAnimation.AnimationState.GetCurrent(1);
        if (attackEntry != null)
        {
            attackEntry.Complete += _ =>
            {
                _skeletonAnimation.AnimationState.ClearTrack(1);
                _isAttacking = false;
            };
        }

    }
}