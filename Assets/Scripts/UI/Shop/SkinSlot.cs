using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinSlot : MonoBehaviour
{
    public TMP_Text nameText;
    public Image profileImage;
    public Button payButton;
    public TMP_Text priceText;

    private int skinId;
    private int skinPrice;

    internal void Initialize(SkinData skin, Sprite sprite)
    {
        skinId = skin.Id;
        skinPrice = skin.zemPrice;
        nameText.text = skin.Name;
        if (sprite != null)
        {
            profileImage.sprite = sprite;
        }

        UpdateText();

        payButton.onClick.AddListener(() => OnPayButtonClicked());
        UserDataManager.Instance._userInfo.OnUserInfoChanged += RefreshUI;
    }

    private void UpdateText()
    {
        if (UserDataManager.Instance._userInfo.skins.Contains(skinId))
        {
            priceText.text = "Owned";
            payButton.interactable = false;
        }
        else
        {
            priceText.text = $"{skinPrice} Gem";
            payButton.interactable = true;
        }
    }

    private void RefreshUI()
    {
        UpdateText();
    }

    private void OnPayButtonClicked()
    {
        UserInfo userInfo = UserDataManager.Instance._userInfo;
        int currentGem = userInfo.currency.gems;
        if (currentGem < skinPrice)
        {
#if UNITY_EDITOR
            Debug.Log("Not enough gem!");
#endif
            ShopUIController.Instance.ShowNoticePopup("Not enough gem!");
            return;
        }

        BuySkinRequest request = new BuySkinRequest
        {
            SkinId = skinId,
            Price = skinPrice,
            CurrentGem = userInfo.currency.gems
        };

        IEnumerator buySkinCoroutine = ShopApi.BuySkin(request,
        (BuySkinResponse res) =>
        {
            // 구매 완료하면, UI 업데이트
#if UNITY_EDITOR
            Debug.Log("Purchase successful");
            Debug.Log($"New gems amount: {res.ResultGem}");
#endif
            userInfo.UpdateGems(res.ResultGem);
            userInfo.UpdateSkin(skinId);
        },
        (string err) =>
        {
            // TODO : 구매 실패 UI 띄우기
            Debug.Log($"Purchase failed: {err}");
        });

        ShopUIController.Instance.ShowBuyPopup(buySkinCoroutine);
    }
}
