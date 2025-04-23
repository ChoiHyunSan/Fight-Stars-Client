using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoUI : MonoBehaviour
{
    private Slider expSlider;
    private TMP_Text expText;
    private TMP_Text levelText;
    private TMP_Text nicknameText;

    private TMP_Text trophyText;
    private TMP_Text rankText;
    private TMP_Text winCountText;
    private TMP_Text loseCountText;

    private Button closeButton;

    void Start()
    {
        expSlider = transform.Find("Popup/Group_Middle/Slider_Exp")?.GetComponent<Slider>();
        expText = transform.Find("Popup/Group_Middle/Slider_Exp/Text_Exp")?.GetComponent<TMP_Text>();
        levelText = transform.Find("Popup/Group_Middle/Slider_Exp/Level/Text_Level")?.GetComponent<TMP_Text>();
        nicknameText = transform.Find("Popup/Group_Top/UserName/Text_UserName")?.GetComponent<TMP_Text>();
        trophyText = transform.Find("Popup/Group_Bottom/Stats/Stat_Highest/HighestTrophies/Text_Value")?.GetComponent<TMP_Text>();
        rankText = transform.Find("Popup/Group_Bottom/Stats/Stat_Highest/HighestRank/Text_Value")?.GetComponent<TMP_Text>();
        winCountText = transform.Find("Popup/Group_Bottom/Stats/Stat_WinRate/WinCount/Text_Value")?.GetComponent<TMP_Text>();
        loseCountText = transform.Find("Popup/Group_Bottom/Stats/Stat_WinRate/LoseCount/Text_Value")?.GetComponent<TMP_Text>();
        closeButton = transform.Find("Popup/Button_Close")?.GetComponent<Button>();

#if UNITY_EDITOR
        if (expSlider == null || expText == null || levelText == null || nicknameText == null ||
            trophyText == null || rankText == null || winCountText == null || loseCountText == null || closeButton == null)
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

        closeButton.onClick.AddListener(OnCloseButtonClicked);
        UserDataManager.Instance._userInfo.OnUserInfoChanged += RefreshUI;
        RefreshUI();
    }

    private void OnCloseButtonClicked()
    {
        LobbyUIController.Instance.HideUserInfoPopup();
    }

    private void RefreshUI()
    {
        expSlider.value = UserDataManager.Instance._userInfo.currency.exp / 400f;
        expText.text = $"{UserDataManager.Instance._userInfo.currency.exp} / 400";
        levelText.text = $"{UserDataManager.Instance._userInfo.stats.level}";
        nicknameText.text = UserDataManager.Instance._userInfo.nickname;
        trophyText.text = $"{UserDataManager.Instance._userInfo.stats.currentTrophy}";
        rankText.text = $"{UserDataManager.Instance._userInfo.stats.highestRank}";
        winCountText.text = $"{UserDataManager.Instance._userInfo.stats.winCount}";
        loseCountText.text = $"{UserDataManager.Instance._userInfo.stats.loseCount}";
    }
}
