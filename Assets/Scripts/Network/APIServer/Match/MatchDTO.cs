[System.Serializable]
public class MatchRequest
{
    public long UserId;
    public string JwtToken;
    public int CharacterId;
    public int SkinId;
    public string Mode;
}

public class MatchResponse
{
    public string roomId;
    public string password;
    public string ip;
    public int port;
}