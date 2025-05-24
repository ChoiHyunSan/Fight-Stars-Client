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
        // ĳ���� ����
        GameManager.Instance.currentCharacterId = characterId;

        // ��Ų ��Ͽ��� ���� �տ� �ִ� ĳ������ ��Ų�� ����
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

        // ���� ���õ� ĳ���Ϳ� ���Ͽ�, ���õ� ĳ���� UI ������Ʈ
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
            // TODO : �ݾ� ���� �˸� UI ����
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
            // ���� �Ϸ��ϸ�, UI ������Ʈ
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
            // TODO : ���� ���� UI ����
            Debug.Log($"Purchase failed: {err}");
        });

        ShopUIController.Instance.ShowBuyPopup(buyCharacterCoroutine);
    }
}
