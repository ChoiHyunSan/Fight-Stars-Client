using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiClient
{
    public static string BaseUrl = "http://localhost:5001/api";

    public static IEnumerator Post<T>(string path, object body, Action<T> onSuccess, Action<string> onError, bool retrying = false)
    {
        Debug.Log($"POST: {BaseUrl}/{path}");

        string url = $"{BaseUrl}/{path}";
        string jsonData = JsonUtility.ToJson(body);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        AddJwtHeader(request);

        yield return request.SendWebRequest();
        yield return HandleResponse(request, () => Post(path, body, onSuccess, onError, true), onSuccess, onError, retrying);
    }

    public static IEnumerator Get<T>(string path, Action<T> onSuccess, Action<string> onError, bool retrying = false)
    {
        string url = $"{BaseUrl}/{path}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");
        AddJwtHeader(request);

        yield return request.SendWebRequest();
        yield return HandleResponse(request, () => Get(path, onSuccess, onError, true), onSuccess, onError, retrying);
    }

    public static IEnumerator Put<T>(string path, object body, Action<T> onSuccess, Action<string> onError, bool retrying = false)
    {
        string url = $"{BaseUrl}/{path}";
        string jsonData = JsonUtility.ToJson(body);

        UnityWebRequest request = UnityWebRequest.Put(url, jsonData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        AddJwtHeader(request);

        yield return request.SendWebRequest();
        yield return HandleResponse(request, () => Put(path, body, onSuccess, onError, true), onSuccess, onError, retrying);
    }

    public static IEnumerator Delete<T>(string path, Action<T> onSuccess, Action<string> onError, bool retrying = false)
    {
        string url = $"{BaseUrl}/{path}";

        UnityWebRequest request = UnityWebRequest.Delete(url);
        request.SetRequestHeader("Content-Type", "application/json");
        AddJwtHeader(request);

        yield return request.SendWebRequest();
        yield return HandleResponse(request, () => Delete(path, onSuccess, onError, true), onSuccess, onError, retrying);
    }

    private static void AddJwtHeader(UnityWebRequest request)
    {
        if (PlayerPrefs.HasKey("jwt"))
        {
            string token = PlayerPrefs.GetString("jwt");
            request.SetRequestHeader("Authorization", $"Bearer {token}");
        }
    }

    private static IEnumerator HandleResponse<T>(UnityWebRequest request, Func<IEnumerator> retryFunc, Action<T> onSuccess, Action<string> onError, bool retrying)
    {
        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                T result = JsonUtility.FromJson<T>(request.downloadHandler.text);
                onSuccess?.Invoke(result);
            }
            catch (Exception e)
            {
                onError?.Invoke("응답 파싱 실패: " + e.Message);
            }
        }
        else if (request.responseCode == 401 && !retrying)
        {
            yield return TokenRefresher.RetryAfterRefresh(
                retryFunc,
                onSuccess,
                err =>
                {
                    LogoutAndRedirectToLogin();
                    onError?.Invoke("세션이 만료되었습니다. 다시 로그인해주세요.");
                });
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

    private static void LogoutAndRedirectToLogin()
    {
        PlayerPrefs.DeleteKey("jwt");
        PlayerPrefs.DeleteKey("refresh_token");
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginPage");
    }
}
