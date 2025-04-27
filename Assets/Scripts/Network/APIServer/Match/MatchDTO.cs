public class MatchRequest
{
    public long userId;
    public int characterId;
    public int skinId;
    public string mode;
}

public class MatchResponse
{
    public string roomId;
    public string password;
    public string ip;
    public int port;
}