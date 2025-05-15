
public class BuyCharacterRequest
{
    public int CharacterId;
    public int Price;
    public int CurrentGold;
}

public class BuyCharacterResponse
{
    public bool Success;
    public int ResultGold;

    public UserBrawlerDto Brawler;
}
