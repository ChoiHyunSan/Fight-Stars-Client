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
    public Button selectButton;
    public TMP_Text selectText;
    public TMP_Text priceText;

    [SerializeField]
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

        if(payButton == null || selectButton == null || selectText == null)
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
        // 캐릭터 선택
        GameManager.Instance.currentCharacterId = characterId;

        // 스킨 목록에서 가장 앞에 있는 캐릭터의 스킨을 선택
        var skin = UserDataManager.Instance._shopData.Skins
            .Where(s => s.BrawlerId == characterId)
            .OrderBy(s => s.Id)
            .FirstOrDefault();

        if (skin != null)
        {
            GameManager.Instance.currentSkinId = skin.Id;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"No skin found for character ID: {characterId}");
#endif
        }

        GameObject.Find("Shop").GetComponent<ShopUI>().UpdateSlot();
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

        // 현재 선택된 캐릭터와 비교하여, 선택된 캐릭터 UI 업데이트
        if (characterId == GameManager.Instance.currentCharacterId)
        {
            selectText.text = "Selected";
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
