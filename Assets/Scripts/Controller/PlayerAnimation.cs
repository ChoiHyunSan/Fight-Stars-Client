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
        if (!isAttacking) // ���� �߿��� �̵� ���� ������Ʈ�� ��� ����
        {
            Vector2 velocity = rb.velocity;
            if (velocity.magnitude > 0.01f)
            {
                ChangeState(PlayerState.Move);

                // �̵� ���� ����
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
                skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 0f); // ���� ������ Idle ����
                break;
        }
    }

    public void PlayAttack()
    {
        Debug.Log("Attack Animation Triggered!");

        if (isAttacking)
            return;

        isAttacking = true;

        // ��ü(Track 1)�� Attack �ִϸ��̼Ǹ� ��� (Loop = false)
        skeletonAnimation.AnimationState.SetAnimation(1, "Attack", false);

        // ��ü ���� �ִ� ������ Track 1 ����� �ȱ⸸ ���� ��
        TrackEntry attackEntry = skeletonAnimation.AnimationState.GetCurrent(1);
        if (attackEntry != null)
        {
            attackEntry.Complete += entry =>
            {
                skeletonAnimation.AnimationState.ClearTrack(1); // ���� ������ ��ü �ִ� ������
                isAttacking = false;
            };
        }
    }

    private IEnumerator AttackEndCoroutine()
    {
        yield return new WaitForSeconds(0.5f); // ���� �ִϸ��̼� ���̿� �°�
        isAttacking = false;
    }
}
