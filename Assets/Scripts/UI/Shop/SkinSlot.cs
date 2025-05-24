using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SkinSlot : MonoBehaviour
{
    public TMP_Text nameText;
    public Image profileImage;
    public Button payButton;
    public Button selectButton;
    public TMP_Text selectText;
    public TMP_Text priceText;

    [SerializeField]
    private int skinId;
    private int skinPrice;
    private int characterId;

    internal void Initialize(SkinData skin, Sprite sprite)
    {
        skinId = skin.Id;
        skinPrice = skin.zemPrice;
        nameText.text = skin.Name;
        characterId = skin.BrawlerId;
        if (sprite != null)
        {
            profileImage.sprite = sprite;
        }

        UpdateText();

        if(payButton == null || selectButton == null)
        {
            Debug.LogError("PayButton or SelectButton is not assigned in the inspector.");
            return;
        }

        payButton.onClick.AddListener(() => OnPayButtonClicked());
        UserDataManager.Instance._userInfo.OnUserInfoChanged += RefreshUI;

        selectButton.onClick.AddListener(() => OnSelectButtonClicked());
    }

    private void OnSelectButtonClicked()
    {
        GameManager.Instance.currentSkinId = skinId;

        GameObject.Find("Shop").GetComponent<ShopUI>().UpdateSlot();
    }

    private void UpdateText()
    {
        if(priceText == null || selectText == null || selectButton == null)
        { 
            return;
        }

        if (UserDataManager.Instance._userInfo.skins.Contains(skinId) || skinPrice == 0)
        {
            priceText.text = "Owned";
            payButton.interactable = false;
        }
        else
        {
            priceText.text = $"{skinPrice} Gem";
            payButton.interactable = true;
        }

        // 현재 선택된 캐릭터와 비교하여, 선택된 캐릭터 UI 업데이트
        if (skinId == GameManager.Instance.currentSkinId)
        {
            selectText.text = "Selected";
            selectButton.interactable = false;
        }
        // 현재 선택된 캐릭터와 다른 경우
        else if(GameManager.Instance.currentCharacterId != characterId)
        {
            selectText.text = "Enable";
            selectButton.interactable = false;
        }
        else
        {
            selectText.text = "Choose";
            selectButton.interactable = true;
        }
    }

    public void RefreshUI()
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
