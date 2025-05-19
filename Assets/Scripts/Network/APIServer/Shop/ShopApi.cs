using System;
using System.Collections;

public static class ShopApi
{     /**
     * BuyCharacter
     * ĳ���� ���� ��û
     */
    public static IEnumerator BuyCharacter(BuyCharacterRequest request, System.Action<BuyCharacterResponse> onSuccess, System.Action<string> onError)
    {
        var body = request;
        yield return ApiClient.Post("shop/buy/character", body,
            (BuyCharacterResponse res) =>
            {
                onSuccess?.Invoke(res);
            },
            (string err) =>
            {
                onError?.Invoke(err);
            });
    }

    internal static IEnumerator BuySkin(BuySkinRequest request, Action<BuySkinResponse> onSuccess, Action<string> onError)
    {
        var body = request;
        yield return ApiClient.Post("shop/buy/skin", body,
            (BuySkinResponse res) =>
            {
                onSuccess?.Invoke(res);
            },
        (string err) =>
        {
                onError?.Invoke(err);
            });
    }
}