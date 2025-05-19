using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // TODO : �� ������ S_EnterRoom ��Ŷ���� ���� ó���Ǿ� ��
    public Team team;
    public string nickname;
    public int userId;

    // TODO : ĳ���� ������ ���� �ÿ� CharacterDatabase���� �����;� �� (������ UI���̹Ƿ� Ŭ���̾�Ʈ���� �����͵� ��� X)
    public int maxHp;
    public int hp;

    public HpBar hpBar;

    public void SetTeam(int playerId)
    {
        this.team = playerId % 2 == 0 ? Team.Red : Team.Blue;

        if (team == Team.Red)
        {
            hpBar.fillImage.color = Color.red;
        }
        else
        {
            hpBar.fillImage.color = Color.blue;
        }
        hpBar.SetNickname(nickname);
    }

    public void UpdateHp(int newHp)
    {
        hp = newHp;
        if(hp < 0)
        {
            hp = 0;
        }

        hpBar.SetRatio((float)hp / maxHp);
    }
}
