using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserDataManager : MonoBehaviour
{
    [SerializeField]
    public UserInfo _userInfo  = new UserInfo();
        
    public ShopData _shopData = new ShopData();

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
        LoadData();

#if UNITY_EDITOR
        // _userInfo 디버깅
        Debug.Log("UserInfo initialized");
        Debug.Log($"UserInfo: {_userInfo.nickname}");
#endif
    }

    private void LoadData()
    {
        LoadUserData();
        LoadGameData();
    }

    private void LoadGameData()
    {
        string brawlerJsonPath = Path.Combine(Application.streamingAssetsPath, "Data/brawlers.json");
        string skinJsonPath = Path.Combine(Application.streamingAssetsPath, "Data/skins.json");

        if (File.Exists(brawlerJsonPath))
        {
            string json = File.ReadAllText(brawlerJsonPath);
            _shopData.Brawlers = JsonUtilityWrapper.FromJsonList<BrawlerData>(json);
        }

        if (File.Exists(skinJsonPath))
        {
            string json = File.ReadAllText(skinJsonPath);
            _shopData.Skins = JsonUtilityWrapper.FromJsonList<SkinData>(json);
        }
    }

    private void LoadUserData()
    {
        // User Data 로드
        StartCoroutine(DataApi.GetUserData(
            (UserLoadDataResponse res) =>
            {
#if UNITY_EDITOR
                Debug.Log("User data loaded successfully");
#endif
                SetUserData(res);
                GameSceneManager.Instance.LoadScene(SceneType.Lobby);
            },
            (string err) =>
            {
#if UNITY_EDITOR
                Debug.Log($"Failed to load user data: {err}");
#endif
                GameSceneManager.Instance.LoadScene(SceneType.Title);
            }));


    }

    private void SetUserData(UserLoadDataResponse res)
    {
        _userInfo.SetData(res);
    }
}