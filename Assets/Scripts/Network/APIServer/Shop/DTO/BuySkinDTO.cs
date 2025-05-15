[System.Serializable]
public class BuySkinRequest
{
    public int SkinId;
    public int Price;
    public int CurrentGem;
}

[System.Serializable]
public class BuySkinResponse
{
    public bool Success;
    public int ResultGem;
}