using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopNoticeUI : MonoBehaviour
{
    private Button okButton;
    private TMP_Text messageText;

    void Awake()
    {
        okButton = transform.Find("Popup/Button_Ok").GetComponent<Button>();
        messageText = transform.Find("Popup/Text_Message").GetComponent<TMP_Text>();

#if UNITY_EDITOR
        if (okButton == null || messageText == null)
        {
            Debug.Log("UI elements are not assigned properly");
            return;
        }
#endif
        okButton.onClick.AddListener(OnOkButtonClicked);
    }

    private void OnOkButtonClicked()
    {
        ShopUIController.Instance.HideNoticePopup();
    }

    public void SetText(string text)
    {
        if(messageText == null)
        {
            Debug.Log("messageText is null");
            return;
        }

        messageText.text = text;
    }
}
