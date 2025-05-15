using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyUI : MonoBehaviour
{
    private TMP_Text messageText;
    private Button okButton;
    private Button cancelButton;

    private IEnumerator enumerator;

    private void Awake()
    {
        messageText = transform.Find("Popup/Text_Message").GetComponent<TMP_Text>();
        okButton = transform.Find("Popup/Button_Ok").GetComponent<Button>();
        cancelButton = transform.Find("Popup/Button_No").GetComponent<Button>();

#if UNITY_EDITOR
        if (messageText == null || okButton == null || cancelButton == null)
        {
            Debug.Log("UI elements are not assigned properly");
            return;
        }
#endif

        okButton.onClick.AddListener(OnOkButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnCancelButtonClicked()
    {
        ShopUIController.Instance.HideBuyPopup();
    }

    private void OnOkButtonClicked()
    {
        if (enumerator == null)
        {
#if UNITY_EDITOR
            Debug.Log("enumerator is null");
#endif
        }
        else
        {
            StartCoroutine(RunAndThenHide(enumerator));
        }
    }

    private IEnumerator RunAndThenHide(IEnumerator original)
    {
        yield return StartCoroutine(original); // 원래 코루틴 실행 완료 대기
        ShopUIController.Instance.HideBuyPopup(); // 끝나고 UI 닫기
    }

    public void SetOkEnumerator(IEnumerator en)
    {
        enumerator = en;
    }
}
