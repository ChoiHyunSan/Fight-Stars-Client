using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject userInfoPopup;
    public GameObject settingPopup;
    public Button settingButton;

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

    private void Start()
    {
        settingButton.onClick.AddListener(OnSettingButtonClicked);
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
}
