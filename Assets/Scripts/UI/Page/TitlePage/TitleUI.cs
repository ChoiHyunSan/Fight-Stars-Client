using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button pressToStart;
    public Button testButton;

    private void Start()
    {
        pressToStart.onClick.AddListener(OnPressToStart);
        testButton.onClick.AddListener(OnTestButtonClicked);
    }

    private void OnTestButtonClicked()
    {
        // JWT 토큰을 PlayerPrefs에서 지웁니다.
        PlayerPrefs.DeleteKey("jwt");
        PlayerPrefs.DeleteKey("refresh_token");
    }

    private void OnPressToStart()
    {
#if UNITY_EDITOR
        Debug.Log("Press to Start clicked");
#endif

        AuthManager.Instance.SaveLogin();
    }
}
