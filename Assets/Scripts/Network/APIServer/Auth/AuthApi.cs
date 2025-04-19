using System.Collections;
using UnityEngine;

public static class AuthApi
{
    /**
     * SaveLogin
     * Ÿ��Ʋ ȭ�鿡�� �������� �α��� ������ �ִ��� Ȯ���ϰ�, �ִٸ� �ڵ����� �α��� �õ�
     */
    public static IEnumerator SaveLogin(System.Action<LoginResponse> onSuccess, System.Action<string> onError)
    {
        yield return ApiClient.Post("auth/login/save", null,
            (LoginResponse res) =>
        {
            onSuccess?.Invoke(res);
        },
        (string err) =>
        {
            onError?.Invoke(err);
        });
    }

    /**
     * Login
     * �α��� ��û
     */
    public static IEnumerator Login(LoginRequest request, System.Action<LoginResponse> onSuccess, System.Action<string> onError)
    {
        var body = request;
        yield return ApiClient.Post("auth/login", body,
            (LoginResponse res) =>
            {
                onSuccess?.Invoke(res);
            },
            (string err) =>
            {
                onError?.Invoke(err);
            });
    }

    /**
     * Logout
     * �α׾ƿ� ��û
     */
    public static IEnumerator Register(RegisterRequest request, System.Action<RegisterResponse> onSuccess, System.Action<string> onError)
    {
        var body = request;
        yield return ApiClient.Post("auth/register", body,
            (RegisterResponse res) =>
            {
                onSuccess?.Invoke(res);
            },
            (string err) =>
            {
                onError?.Invoke(err);
            });
    }
}
