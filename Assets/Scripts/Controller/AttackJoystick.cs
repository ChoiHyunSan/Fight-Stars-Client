using UnityEngine.EventSystems;

public class AttackJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }


    private MyPlayerController playerController;

    public void SetPlayerController(MyPlayerController controller)
    {
        playerController = controller;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);

        // TODO : 투사체 방향이 조이스틱 방향으로 가이드가 보이게 설정
        if(playerController != null)
        {
            playerController.DisplayGuide(true);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        // 방향 갱신
        if (playerController != null && Direction.sqrMagnitude > 0.01f)
        {
            playerController.CacheAttackDirection(Direction.normalized);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);

        // TODO : 투사체가 조이스틱 방향으로 발사되도록 구현
        if (playerController != null)
        {
            playerController.DisplayGuide(false);
            playerController.SendAttackCommandToServer();
        }
    }
}
