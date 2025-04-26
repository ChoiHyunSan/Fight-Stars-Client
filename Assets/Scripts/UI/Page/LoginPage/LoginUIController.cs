using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject loginPopup;
    public GameObject registerPopup;
    public GameObject noticePopup;
    public static LoginUIController Instance { get; private set; }

    private void Awake()
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

    public void ShowLoginPopup()
    {
        loginPopup.SetActive(true);
        registerPopup.SetActive(false);
    }

    public void ShowRegisterPopup()
    {
        loginPopup.SetActive(false);
        registerPopup.SetActive(true);
    }

    public void ShowNoticePopup(string message)
    {
        noticePopup.SetActive(true);
        noticePopup.GetComponent<LoginNoticeUI>().UpdateMessage(message);
    }

    public void HideNoticePopup()
    {
        noticePopup.SetActive(false);
    }
}
