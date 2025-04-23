using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserDataManager : MonoBehaviour
{
    [SerializeField]
    public UserInfo _userInfo  = new UserInfo();

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