using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginNoticeUI : MonoBehaviour
{
    private Button okButton;
    [SerializeField] private TMP_Text messageText;

    void Awake()
    {
        okButton = transform.Find("Popup/Button_Ok")?.GetComponent<Button>();
        messageText = transform.Find("Popup/Text_Message")?.GetComponent<TMP_Text>();

#if UNITY_EDITOR
        if (okButton == null || messageText == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            return;
        }
#endif

        okButton.onClick.AddListener(OnOkButtonClicked);
    }

    private void OnOkButtonClicked()
    {
        LoginUIManager.Instance.HideNoticePopup();

    }

    public void UpdateMessage(string message)
    {
        if (messageText == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Message Text is not assigned.");
#endif
            return;
        }

        messageText.text = message;
    }
}
