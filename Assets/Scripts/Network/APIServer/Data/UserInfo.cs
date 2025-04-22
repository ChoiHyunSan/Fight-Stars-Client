using System;
using System.Collections.Generic;

[System.Serializable]
public class UserInfo
{
    public string nickname;
    public string? avatar;
    public UserCurrency currency;
    public UserStats stats  ;
    public List<UserInventory> inventory;
    public List<UserBrawler> brawlers;

    public void SetData(UserLoadDataResponse res)
    {
        nickname = res.Nickname;
        avatar = res.Avatar;
        currency = new UserCurrency
        {
            gold = res.Currency.Gold,
            gems = res.Currency.Gems,
            energy = res.Currency.Energy,
            exp = res.Currency.Exp
        };
        stats = new UserStats
        {
            level = res.Stats.Level,
            winCount = res.Stats.WinCount,
            loseCount = res.Stats.LoseCount,
            totalPlayCount = res.Stats.TotalPlayCount,
            highestRank = res.Stats.HighestRank,
            currentTrophy = res.Stats.CurrentTrophy
        };
        inventory = new List<UserInventory>();
        foreach (var item in res.Inventory)
        {
            inventory.Add(new UserInventory
            {
                itemId = item.ItemId,
                quantity = item.Quantity
            });
        }
        brawlers = new List<UserBrawler>();
        foreach (var brawler in res.Brawlers)
        {
            brawlers.Add(new UserBrawler
            {
                brawlerId = brawler.BrawlerId,
                level = brawler.Level,
                trophy = brawler.Trophy,
                powerPoint = brawler.PowerPoint
            });
        }
    }
}

[System.Serializable]
public class UserCurrency
{
    public int gold { get; set; }
    public int gems { get; set; }
    public int energy { get; set; }
    public int exp { get; set; }
}

[System.Serializable]
public class UserStats
{
    public int level;
    public int winCount;
    public int loseCount;
    public int totalPlayCount;
    public int highestRank;
    public int currentTrophy;
}

[System.Serializable]
public class UserInventory
{
    public int itemId;
    public int quantity;
}

[System.Serializable]
public class UserBrawler
{
    public int brawlerId;
    public int level;
    public int trophy;
    public int powerPoint;
}   
