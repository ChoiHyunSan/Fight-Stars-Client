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
                GameSceneManager.Instance.LoadScene(SceneType.Loading);
            },
            (string err) =>
            {
                Debug.Log($"Login failed: {err}");
            }));
    }

    public void Login(string email, string password)
    {
        var request = new LoginRequest { Username = email, Password = password };

        StartCoroutine(AuthApi.Login(request,
            (LoginResponse res) =>
            {
                Debug.Log("Login successful");

                // JWT 토큰 저장
                PlayerPrefs.SetString("jwt", res.accessToken);
                PlayerPrefs.SetString("refresh_token", res.refreshToken);

                // 데이터 로딩 화면으로 이동
                GameSceneManager.Instance.LoadScene(SceneType.Loading);
            },
            (string err) =>
            {
                Debug.Log($"Login failed: {err}");

                // TODO : 실패 이유에 따라서 UI에 에러 메시지 출력
                // 예: "유효하지 않은 ID입니다. 다시 시도해주세요."
                LoginUIManager.Instance.ShowNoticePopup("Login failed");
            }));
    }

    public void Register(string email, string password, string username)
    {
        var request = new RegisterRequest { Email = email, Password = password, Username = username };

        StartCoroutine(AuthApi.Register(request,
            (RegisterResponse res) =>
            {
                Debug.Log("Login successful");

                LoginUIManager.Instance.ShowNoticePopup("Register success");
                LoginUIManager.Instance.ShowLoginPopup();
            },
            (string err) =>
            {
                Debug.Log($"Register failed: {err}");

                // TODO : 오류 메시지를 좀 더 상세하게 출력하도록 수정
                LoginUIManager.Instance.ShowNoticePopup("Register failed");
            }));
    }
}
