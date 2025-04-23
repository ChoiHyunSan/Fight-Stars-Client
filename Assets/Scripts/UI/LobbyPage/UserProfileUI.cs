using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserProfileUI : MonoBehaviour
{
    private Slider expSlider;
    private TMP_Text expText;
    private TMP_Text levelText;
    private TMP_Text nicknameText;
    private Button userInfoButton;

    void Start()
    {
        expSlider = transform.Find("Slider_Exp")?.GetComponent<Slider>();
        expText = transform.Find("Slider_Exp/Bg/Text_Exp")?.GetComponent<TMP_Text>();
        levelText = transform.Find("Level_Frame/Text_Level")?.GetComponent<TMP_Text>();
        nicknameText = transform.Find("Text_Name")?.GetComponent<TMP_Text>();
        userInfoButton = transform.Find("Button_UserInfo")?.GetComponent<Button>();

#if UNITY_EDITOR
        if (expSlider == null || expText == null || levelText == null || nicknameText == null || userInfoButton == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            return;
        }

        if(UserDataManager.Instance == null || UserDataManager.Instance._userInfo == null)
        {
            Debug.LogError("UserDataManager or UserInfo is null");
            return;
        }
#endif
        userInfoButton.onClick.AddListener(OnUserInfoButtonClicked);

        UserDataManager.Instance._userInfo.OnUserInfoChanged += RefreshUI;
        RefreshUI();
    }

    private void OnUserInfoButtonClicked()
    {
        LobbyUIController.Instance.ShowUserInfoPopup();
    }

    void RefreshUI()
    {
        expSlider.value = UserDataManager.Instance._userInfo.currency.exp / 400f; 
        expText.text = $"{UserDataManager.Instance._userInfo.currency.exp} / 400";
        levelText.text = $"{UserDataManager.Instance._userInfo.stats.level}";
        nicknameText.text = UserDataManager.Instance._userInfo.nickname;
    }

}
