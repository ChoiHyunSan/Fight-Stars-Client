using System.Collections;

public static class DataApi
{
    /**
     * GetUserData
     * ���� �����͸� ��û�Ѵ�.
     * API ���������� JWT ������ ���� ������ �ĺ��ϰ�, �ش� ������ �����͸� ��ȯ�Ѵ�.
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