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
#if UNITY_EDITOR
                Debug.Log("Login successful");
#endif
                GameSceneManager.Instance.LoadScene(SceneType.Loading);
            },
            (string err) =>
            {
#if UNITY_EDITOR
                Debug.Log($"Login failed: {err}");
#endif
            }));
    }

    public void Login(string email, string password)
    {
        var request = new LoginRequest { Username = email, Password = password };

        StartCoroutine(AuthApi.Login(request,
            (LoginResponse res) =>
            {
#if UNITY_EDITOR
                Debug.Log("Login successful");
#endif
                // JWT ��ū ����
                PlayerPrefs.SetString("jwt", res.accessToken);
                PlayerPrefs.SetString("refresh_token", res.refreshToken);

                // ������ �ε� ȭ������ �̵�
                GameSceneManager.Instance.LoadScene(SceneType.Loading);
            },
            (string err) =>
            {
#if UNITY_EDITOR
                Debug.Log($"Login failed: {err}");
#endif

                // TODO : ���� ������ ���� UI�� ���� �޽��� ���
                // ��: "��ȿ���� ���� ID�Դϴ�. �ٽ� �õ����ּ���."
                LoginUIController.Instance.ShowNoticePopup("Login failed");
            }));
    }

    public void Register(string email, string password, string username)
    {
        var request = new RegisterRequest { Email = email, Password = password, Username = username };

        StartCoroutine(AuthApi.Register(request,
            (RegisterResponse res) =>
            {
#if UNITY_EDITOR
                Debug.Log("Login successful");
#endif
                LoginUIController.Instance.ShowNoticePopup("Register success");
                LoginUIController.Instance.ShowLoginPopup();
            },
            (string err) =>
            {
#if UNITY_EDITOR
                Debug.Log($"Register failed: {err}");
#endif

                // TODO : ���� �޽����� �� �� ���ϰ� ����ϵ��� ����
                LoginUIController.Instance.ShowNoticePopup("Register failed");
            }));
    }
}
