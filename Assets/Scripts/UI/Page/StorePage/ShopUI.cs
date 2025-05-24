using Spine.Unity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [Header("UI Elements")]
    private TMP_Text goldText;
    private TMP_Text gemText;
    private GameObject characterScrollRectContent;
    private GameObject skinScrollRectContent;

    [Header("Shop Prefabs")]
    public GameObject characterSlotPrefabs;
    public GameObject skinSlotPrefabs;

    public CharacterDatabase characterDatabase;

    // UI 갱신을 위해 생성한 Prefabs를 저장하는 변수
    List<GameObject> characterSlots = new List<GameObject>();
    List<GameObject> skinSlots = new List<GameObject>();

    void Start()
    {
        // Find the UI elements in the hierarchy
        goldText = transform.Find("Top/StatusBar_Group/Text_Gold/Text_Value").GetComponent<TMP_Text>();
        gemText = transform.Find("Top/StatusBar_Group/Text_Gem/Text_Value").GetComponent<TMP_Text>();
        characterScrollRectContent = transform.Find("ScrollRect/SR_Character/Content").gameObject;
        skinScrollRectContent = transform.Find("ScrollRect/SR_Skin/Content").gameObject;

#if UNITY_EDITOR
        if (goldText == null || gemText == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            return;
        }

        if (UserDataManager.Instance == null || UserDataManager.Instance._userInfo == null)
        {
            Debug.LogError("UserDataManager or UserInfo is null");
            return;
        }
#endif

        // Add listeners to the UserInfo object
        UserDataManager.Instance._userInfo.OnUserInfoChanged += RefreshUI;
        RefreshUI();

        // Initialize the character and skin slots
        InitializeCharacterSlots();
        InitializeSkinSlots();
    }

    private void InitializeSkinSlots()
    {
        ShopData shopData = UserDataManager.Instance._shopData;
        // 프리팹 생성 후, Content 하위에 추가
        foreach(var skin in shopData.Skins)
        {
            //if(skin.zemPrice == 0)
            //{
            //    continue;
            //}

            GameObject skinSlot = Instantiate(skinSlotPrefabs, skinScrollRectContent.transform);
            SkinSlot slot = skinSlot.GetComponent<SkinSlot>();
            Sprite profileImage = characterDatabase.GetSkinImage(skin.Id);
            slot.Initialize(skin, profileImage);
            skinSlots.Add(skinSlot);
        }
    }

    private void InitializeCharacterSlots()
    {
        ShopData shopData = UserDataManager.Instance._shopData;
        // 프리팹 생성 후, Content 하위에 추가
        foreach (var brawler in shopData.Brawlers)
        {
            GameObject characterSlot = Instantiate(characterSlotPrefabs, characterScrollRectContent.transform);
            CharacterSlot slot = characterSlot.GetComponent<CharacterSlot>();
            Sprite profileImage = characterDatabase.GetProfileImage(brawler.Id);
            slot.Initialize(brawler, profileImage);
            characterSlots.Add(characterSlot);
        }
    }

    public void UpdateSlot()
    {
        foreach (var slot in characterSlots)
        {
            CharacterSlot characterSlot = slot.GetComponent<CharacterSlot>();
            if (characterSlot != null)
            {
                characterSlot.RefreshUI();
            }
        }

        foreach (var slot in skinSlots)
        {
            SkinSlot skinSlot = slot.GetComponent<SkinSlot>();
            if (skinSlot != null)
            {
                skinSlot.RefreshUI();
            }
        }
    }

    private void RefreshUI()
    {
        goldText.text = $"{UserDataManager.Instance._userInfo.currency.gold}";
        gemText.text = $"{UserDataManager.Instance._userInfo.currency.gems}";
    }

}
