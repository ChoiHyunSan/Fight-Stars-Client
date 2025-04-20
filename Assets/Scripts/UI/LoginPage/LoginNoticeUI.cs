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

        if (okButton == null || messageText == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            return;
        }

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
            Debug.LogError("Message Text is not assigned.");
            return;
        }
        messageText.text = message;
    }
}
