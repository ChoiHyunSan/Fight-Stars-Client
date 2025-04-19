using System.Collections;
using UnityEngine;

public static class AuthApi
{
    /**
     * SaveLogin
     * 타이틀 화면에서 저장중인 로그인 정보가 있는지 확인하고, 있다면 자동으로 로그인 시도
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
     * 로그인 요청
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
     * 로그아웃 요청
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
