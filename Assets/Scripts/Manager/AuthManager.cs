using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

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

    public void SaveLogin()
    {
        StartCoroutine(AuthApi.SaveLogin(
            (LoginResponse res) =>
            {
                Debug.Log("Login successful");

                // 데이터 로딩 화면으로 이동
                SceneManager.LoadScene("LoadingPage");
            },
            (string err) =>
            {
                Debug.Log($"Login failed: {err}");
            }));
    }

    public void Login(string email, string password)
    {
        var request = new LoginRequest { email = email, password = password };

        StartCoroutine(AuthApi.Login(request,
            res =>
            {
                PlayerPrefs.SetString("jwt", res.accessToken);
                PlayerPrefs.SetString("refresh_token", res.refreshToken);
                Debug.Log("로그인 성공: " + res.userName);

                // 로그인 성공 시 다음 씬으로 이동 (예: 로비)

            },
            err =>
            {
                Debug.LogError("로그인 실패: " + err);
            }));
    }

    public void Register(string email, string password, string username)
    {
        var request = new RegisterRequest { email = email, password = password, username = username };

        StartCoroutine(AuthApi.Register(request,
            res =>
            {
                Debug.Log("회원가입 성공: " + res.username);

                // 회원가입 후 자동 로그인 or 로그인 페이지 이동

            },
            err =>
            {
                Debug.LogError("회원가입 실패: " + err);
            }));
    }
}
