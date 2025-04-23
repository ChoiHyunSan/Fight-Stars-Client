using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject userInfoPopup;
    public GameObject settingPopup;
    public Button settingButton;

    public Button stageButton;
    public Button matchingButton;

    public Button shopButton;
    public Button inventoryButton;
    public Button brawlerButton;

    public static LobbyUIController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
#if UNITY_EDITOR
        if(UserDataManager.Instance == null || UserDataManager.Instance._userInfo == null)
        {
            Debug.LogError("UserDataManager or UserInfo is null");
            return;
        }
#endif

        settingButton.onClick.AddListener(OnSettingButtonClicked);
        stageButton.onClick.AddListener(OnStageButtonClicked);
        matchingButton.onClick.AddListener(OnMatchingButtonClicked);

    }

    private void OnSettingButtonClicked()
    {
        ShowSettingPopup();
    }

    public void ShowUserInfoPopup()
    {
        userInfoPopup.SetActive(true);
    }

    public void HideUserInfoPopup()
    {
        userInfoPopup.SetActive(false);
    }

    public void ShowSettingPopup()
    {
        settingPopup.SetActive(true);
    }

    public void HideSettingPopup()
    {
        settingPopup.SetActive(false);
    }

    public void OnStageButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Stage button clicked");
        GameSceneManager.Instance.LoadScene(SceneType.Gamemode);
#endif

        // TODO : 현재 선택한 게임 모드가 표현되도록 처리
    }

    public void OnMatchingButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Matching button clicked");
#endif

        // TODO : 매칭 처리
        // GameSceneManager.Instance.LoadScene(SceneType.Loading);
    }
}
