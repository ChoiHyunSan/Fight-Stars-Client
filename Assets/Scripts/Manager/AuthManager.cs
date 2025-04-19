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

                // ������ �ε� ȭ������ �̵�
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
                Debug.Log("�α��� ����: " + res.userName);

                // �α��� ���� �� ���� ������ �̵� (��: �κ�)

            },
            err =>
            {
                Debug.LogError("�α��� ����: " + err);
            }));
    }

    public void Register(string email, string password, string username)
    {
        var request = new RegisterRequest { email = email, password = password, username = username };

        StartCoroutine(AuthApi.Register(request,
            res =>
            {
                Debug.Log("ȸ������ ����: " + res.username);

                // ȸ������ �� �ڵ� �α��� or �α��� ������ �̵�

            },
            err =>
            {
                Debug.LogError("ȸ������ ����: " + err);
            }));
    }
}
