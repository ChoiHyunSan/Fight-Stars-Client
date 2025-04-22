using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserDataManager : MonoBehaviour
{
    [SerializeField]
    private UserInfo _userInfo  = new UserInfo();

    public static UserDataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 초기화 코드
        Debug.Log("UserDataManager initialized");
        LoadData();

        // _userInfo 디버깅
        Debug.Log("UserInfo initialized");
        Debug.Log($"UserInfo: {_userInfo.nickname}");
    }

    private void LoadData()
    {
        StartCoroutine(DataApi.GetUserData(
            (UserLoadDataResponse res) =>
            {
                Debug.Log("User data loaded successfully");
                SetUserData(res);
                LoggindUserInfo();
                GameSceneManager.Instance.LoadScene(SceneType.Lobby);
            },
            (string err) =>
            {
                Debug.Log($"Failed to load user data: {err}");
                GameSceneManager.Instance.LoadScene(SceneType.Title);
            }));
    }

    private void LoggindUserInfo()
    {
        Debug.Log($"Nickname: {_userInfo.nickname}");
        Debug.Log($"Avatar: {_userInfo.avatar}");
        Debug.Log($"Gold: {_userInfo.currency.gold}");
        Debug.Log($"Level: {_userInfo.stats.level}");
        Debug.Log($"Wincount: {_userInfo.stats.winCount}");
        Debug.Log($"Losecount: {_userInfo.stats.loseCount}");
        Debug.Log($"Totalplaycount: {_userInfo.stats.totalPlayCount}");
        Debug.Log($"Highestrank: {_userInfo.stats.highestRank}");
        Debug.Log($"Currenttrophy: {_userInfo.stats.currentTrophy}");
        Debug.Log($"Inventorycount: {_userInfo.inventory.Count}");
        foreach (var item in _userInfo.inventory)
        {
            Debug.Log($"Item: {item.itemId}, {item.quantity}");
        }
        Debug.Log($"UserInfo: {_userInfo.brawlers.Count}");
        foreach (var brawler in _userInfo.brawlers)
        {
            Debug.Log($"Brawler: {brawler.brawlerId}, {brawler.level}, {brawler.trophy}, {brawler.powerPoint}");
        }
    }

    private void SetUserData(UserLoadDataResponse res)
    {
        _userInfo.SetData(res);
    }
}