using System.Collections;

public static class DataApi
{
    /**
     * GetUserData
     * 유저 데이터를 요청한다.
     * API 서버에서는 JWT 정보를 통해 유저를 식별하고, 해당 유저의 데이터를 반환한다.
     */
    public static IEnumerator GetUserData(System.Action<UserLoadDataResponse> onSuccess, System.Action<string> onError)
    {
        yield return ApiClient.Get("data/load",
            (UserLoadDataResponse res) =>
        {
            onSuccess?.Invoke(res);
        },
        (string err) =>
        {
            onError?.Invoke(err);
        });
    }
}