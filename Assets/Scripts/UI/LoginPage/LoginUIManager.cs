using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject loginPopup;
    public GameObject registerPopup;

    public static LoginUIManager Instance { get; private set; }

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
}
