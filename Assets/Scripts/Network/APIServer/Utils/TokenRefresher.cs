using System;
using System.Collections;
using UnityEngine;

public static class TokenRefresher
{
    public static IEnumerator TryRefreshToken(Action onSuccess, Action<string> onFail)
    {
        if (!PlayerPrefs.HasKey("refresh_token"))
        {
            onFail?.Invoke("No refresh token found");
            yield break;
        }

        var refreshToken = PlayerPrefs.GetString("refresh_token");
        var request = new RefreshRequest { refreshToken = refreshToken };

        yield return ApiClient.Post("auth/refresh", request,
            (RefreshResponse res) =>
            {
                PlayerPrefs.SetString("jwt", res.accessToken);
                PlayerPrefs.SetString("refresh_token", res.refreshToken);
                onSuccess?.Invoke();
            },
            (string err) =>
            {
                onFail?.Invoke("Refresh token failed: " + err);
            });
    }


    // 요청을 한 번 재시도하는 래퍼
    public static IEnumerator RetryAfterRefresh<T>(Func<IEnumerator> retryFunc, Action<T> onSuccess, Action<string> onError)
    {
        bool done = false;
        string error = null;

        yield return TryRefreshToken(
            () => { done = true; },
            (err) => { error = err; });

        if (!done)
        {
            onError?.Invoke(error);
            yield break;
        }

        yield return retryFunc.Invoke();
    }
}
