using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    private Button closeButton;

    private void Start()
    {
        closeButton = transform.Find("Popup/Button_Close")?.GetComponent<Button>();
#if UNITY_EDITOR
        if (closeButton == null)
        {
            Debug.LogError("Close button is not assigned in the inspector.");
            return;
        }
        if (LobbyUIController.Instance == null)
        {
            Debug.LogError("LobbyUIController is null");
            return;
        }
#endif
        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void OnCloseButtonClicked()
    {
        LobbyUIController.Instance.HideSettingPopup();
    }
}
