using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // TODO : �� ������ S_EnterRoom ��Ŷ���� ���� ó���Ǿ� ��
    public Team team;
    public string nickname;

    // TODO : ĳ���� ������ ���� �ÿ� CharacterDatabase���� �����;� �� (������ UI���̹Ƿ� Ŭ���̾�Ʈ���� �����͵� ��� X)
    public int maxHp;
    public int hp;

    public void SetTeam(string team)
    {
        this.team = string.Equals(team, "Red") ? Team.Red : Team.Blue;
    }
}
