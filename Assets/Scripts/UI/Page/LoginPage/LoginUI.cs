using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoginUI : MonoBehaviour
{
    private TMP_InputField nicknameInput;
    private TMP_InputField passwordInput;
    private Button loginButton;
    private Button registerButton;

    void Start()
    {
        nicknameInput = transform.Find("Popup/InputField_ID")?.GetComponent<TMP_InputField>();
        passwordInput = transform.Find("Popup/InputField_Password")?.GetComponent<TMP_InputField>();
        loginButton = transform.Find("Popup/Button_Login")?.GetComponent<Button>();
        registerButton = transform.Find("Popup/Button_SignUp")?.GetComponent<Button>();

#if UNITY_EDITOR
        if (nicknameInput == null || passwordInput == null || loginButton == null || registerButton == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            return;
        }
#endif

        loginButton.onClick.AddListener(OnLoginButtonClicked);
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
    }

    private void OnRegisterButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Register button clicked");
#endif

        LoginUIController.Instance.ShowRegisterPopup();
    }

    private void OnLoginButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Login button clicked");
#endif

        TrimInputFields();

        // TODO : 입력값 검증
        if (ValidateNicknameInput() && ValidatePasswordInput())
        {
            AuthManager.Instance.Login(nicknameInput.text, passwordInput.text);
        }

        ClearInputFields();
    }

    private void TrimInputFields()
    {
        nicknameInput.text = nicknameInput.text.Trim();
        passwordInput.text = passwordInput.text.Trim();
    }

    private void ClearInputFields()
    {
        nicknameInput.text = string.Empty;
        passwordInput.text = string.Empty;
    }

    private bool ValidateNicknameInput()
    {
        if (string.IsNullOrEmpty(nicknameInput.text))
        {
            LoginUIController.Instance.ShowNoticePopup("Please enter your nickname.");
            return false;
        }
        return true;
    }

    private bool ValidatePasswordInput()
    {
        if (string.IsNullOrEmpty(passwordInput.text))
        {
            LoginUIController.Instance.ShowNoticePopup("Please enter your password.");
            return false;
        }
        return true;
    }
}
