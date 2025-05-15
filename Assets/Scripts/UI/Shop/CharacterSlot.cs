using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public TMP_Text nameText;
    public Image profileImage;
    public Button payButton;
    public TMP_Text priceText;

    private int characterId;
    private int characterPrice;

    public void Initialize(BrawlerData brawler, Sprite sprite)
    {
        characterId = brawler.Id;
        characterPrice = brawler.goldPrice;
        nameText.text = brawler.Name;

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
        if (UserDataManager.Instance._userInfo.brawlers.Find(b => b.brawlerId == characterId) == null)
        {
            priceText.text = $"{characterPrice} Gold";
            payButton.interactable = true;
        }
        else
        {
            priceText.text = "Owned";
            payButton.interactable = false;
        }
    }

    private void RefreshUI()
    {
        UpdateText();
    }

    private void OnPayButtonClicked()
    {
        UserInfo userInfo = UserDataManager.Instance._userInfo;
        int currentGold = userInfo.currency.gold;
        if(currentGold < characterPrice)
        {
            // TODO : 금액 부족 알림 UI 띄우기
#if UNITY_EDITOR
            Debug.Log("Not enough gold!");
#endif
            ShopUIController.Instance.ShowNoticePopup("Not enough gold!");
            return;
        }

        BuyCharacterRequest request = new BuyCharacterRequest
        {
            CharacterId = characterId,
            Price = characterPrice,
            CurrentGold = userInfo.currency.gold
        };

        IEnumerator buyCharacterCoroutine = ShopApi.BuyCharacter(request,
        (BuyCharacterResponse res) =>
        {
            // 구매 완료하면, UI 업데이트
#if UNITY_EDITOR
            Debug.Log("Purchase successful");
            Debug.Log($"New gold amount: {res.ResultGold}");
            Debug.Log($"Brawler purchased: {res.Brawler}");
#endif
            userInfo.UpdateGold(res.ResultGold);
            userInfo.AddBrawler(res.Brawler);
        },
        (string err) =>
        {
            // TODO : 구매 실패 UI 띄우기
            Debug.Log($"Purchase failed: {err}");
        });

        ShopUIController.Instance.ShowBuyPopup(buyCharacterCoroutine);
    }
}
