using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;

[System.Serializable]
public class UserInfo
{
    public event Action OnUserInfoChanged;

    public int userId;
    public string nickname;
    public string? avatar;
    public UserCurrency currency;
    public UserStats stats;
    public List<UserInventory> inventory;
    public List<UserBrawler> brawlers;
    public List<int> skins;   

    public void SetData(UserLoadDataResponse res)
    {
        userId = res.UserId;
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
        skins = res.Skins.SkinIds;

        OnUserInfoChanged?.Invoke();
    }

    public void UpdateGold(int newGold)
    {
        currency.gold = newGold;
        OnUserInfoChanged?.Invoke();
    }

    public void UpdateGems(int newGems)
    {
        currency.gems = newGems;
        OnUserInfoChanged?.Invoke();
    }
    public void AddBrawler(UserBrawlerDto brawler)
    {
        brawlers.Add(new UserBrawler
        {
            brawlerId = brawler.BrawlerId,
            level = brawler.Level,
            trophy = brawler.Trophy,
            powerPoint = brawler.PowerPoint
        });
        OnUserInfoChanged?.Invoke();
    }

    public void UpdateSkin(int skinId)
    {
        skins.Add(skinId);
        OnUserInfoChanged?.Invoke();
    }
    public void UpdateDataByGameResult(S_Gameover.Types.ResultData resultData)
    {
        currency.gold = resultData.Gold;
        currency.energy = resultData.Energy;
        currency.exp = resultData.Exp;
        stats.level = resultData.Level;
        stats.winCount = resultData.WinCount;
        stats.loseCount = resultData.LoseCount;
        stats.totalPlayCount = resultData.TotalPlayCount;

        OnUserInfoChanged?.Invoke();
    }
}

[System.Serializable]
public class UserCurrency
{
    public int gold;
    public int gems;
    public int energy;
    public int exp;
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
