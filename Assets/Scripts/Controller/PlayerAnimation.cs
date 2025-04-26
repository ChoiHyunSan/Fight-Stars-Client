using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAnimationFSM : MonoBehaviour
{
    private Rigidbody2D rb;
    private SkeletonAnimation skeletonAnimation;
    private PlayerState currentState = PlayerState.Idle;
    private bool isAttacking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void Update()
    {
        if (!isAttacking) // 공격 중에는 이동 상태 업데이트를 잠깐 멈춤
        {
            Vector2 velocity = rb.velocity;
            if (velocity.magnitude > 0.01f)
            {
                ChangeState(PlayerState.Move);

                // 이동 방향 반전
                if (velocity.x > 0)
                    skeletonAnimation.Skeleton.ScaleX = 1f;
                else if (velocity.x < 0)
                    skeletonAnimation.Skeleton.ScaleX = -1f;
            }
            else
            {
                ChangeState(PlayerState.Idle);
            }
        }
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                break;
            case PlayerState.Move:
                skeletonAnimation.AnimationState.SetAnimation(0, "Run", true);
                break;
            case PlayerState.Attack:
                skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
                skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 0f); // 공격 끝나면 Idle 복귀
                break;
        }
    }

    public void PlayAttack()
    {
        Debug.Log("Attack Animation Triggered!");

        if (isAttacking)
            return;

        isAttacking = true;

        // 상체(Track 1)에 Attack 애니메이션만 재생 (Loop = false)
        skeletonAnimation.AnimationState.SetAnimation(1, "Attack", false);

        // 상체 공격 애니 끝나면 Track 1 비워서 걷기만 남게 함
        TrackEntry attackEntry = skeletonAnimation.AnimationState.GetCurrent(1);
        if (attackEntry != null)
        {
            attackEntry.Complete += entry =>
            {
                skeletonAnimation.AnimationState.ClearTrack(1); // 공격 끝나면 상체 애니 지워줌
                isAttacking = false;
            };
        }
    }

    private IEnumerator AttackEndCoroutine()
    {
        yield return new WaitForSeconds(0.5f); // 공격 애니메이션 길이에 맞게
        isAttacking = false;
    }
}
