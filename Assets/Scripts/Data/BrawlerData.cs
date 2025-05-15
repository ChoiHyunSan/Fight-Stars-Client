[System.Serializable]
public class BrawlerData
{
    public int Id;
    public string Name;
    public string Description;
    public int goldPrice;
}

[System.Serializable]
public class SkinData
{
    public int Id;
    public int BrawlerId;
    public string Name;
    public string Description;
    public int zemPrice;
}
