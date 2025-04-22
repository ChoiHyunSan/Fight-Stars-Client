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

                // JWT ��ū ����
                PlayerPrefs.SetString("jwt", res.accessToken);
                PlayerPrefs.SetString("refresh_token", res.refreshToken);

                // ������ �ε� ȭ������ �̵�
                GameSceneManager.Instance.LoadScene(SceneType.Loading);
            },
            (string err) =>
            {
                Debug.Log($"Login failed: {err}");

                // TODO : ���� ������ ���� UI�� ���� �޽��� ���
                // ��: "��ȿ���� ���� ID�Դϴ�. �ٽ� �õ����ּ���."
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

                // TODO : ���� �޽����� �� �� ���ϰ� ����ϵ��� ����
                LoginUIManager.Instance.ShowNoticePopup("Register failed");
            }));
    }
}
