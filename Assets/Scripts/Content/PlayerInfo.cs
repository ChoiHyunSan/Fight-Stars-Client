using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // TODO : 팀 정보는 S_EnterRoom 패킷에서 같이 처리되야 함
    public Team team;
    public string nickname;

    // TODO : 캐릭터 정보는 생성 시에 CharacterDatabase에서 가져와야 함 (어차피 UI용이므로 클라이언트에서 가져와도 상관 X)
    public int maxHp;
    public int hp;

    public void SetTeam(string team)
    {
        this.team = string.Equals(team, "Red") ? Team.Red : Team.Blue;
    }
}
