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

        // TODO : ����ü ������ ���̽�ƽ �������� ���̵尡 ���̰� ����
        if(playerController != null)
        {
            playerController.DisplayGuide(true);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);

        // TODO : ����ü�� ���̽�ƽ �������� �߻�ǵ��� ����
        if (playerController != null)
        {
            playerController.DisplayGuide(false);
            playerController.SendAttackCommandToServer(eventData);
        }
    }
}
