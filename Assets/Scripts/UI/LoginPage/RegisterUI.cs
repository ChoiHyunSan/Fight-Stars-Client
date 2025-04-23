using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUI : MonoBehaviour
{

    private TMP_InputField emailInput;
    private TMP_InputField nicknameInput;
    private TMP_InputField passwordInput;

    private Button signupButton;
    private Button loginButton;

    void Start()
    {
        emailInput = transform.Find("Popup/InputFields/InputField_Email")?.GetComponent<TMP_InputField>();
        nicknameInput = transform.Find("Popup/InputFields/InputField_ID")?.GetComponent<TMP_InputField>();
        passwordInput = transform.Find("Popup/InputFields/InputField_Password")?.GetComponent<TMP_InputField>();

        signupButton = transform.Find("Popup/Button_SignUp")?.GetComponent<Button>();
        loginButton = transform.Find("Popup/Button_Login")?.GetComponent<Button>();

#if UNITY_EDITOR
        if(emailInput == null || nicknameInput == null || passwordInput == null || signupButton == null || loginButton == null)
        { 
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            Debug.LogError($"emailInput: {emailInput}, nicknameInput: {nicknameInput}, passwordInput: {passwordInput}, signupButton: {signupButton}, loginButton: {loginButton}");
            return;
        }
#endif

        signupButton.onClick.AddListener(OnSignupButtonClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Login button clicked. Move To LoginUI");
#endif

        LoginUIController.Instance.ShowLoginPopup();
    }

    private void OnSignupButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Signup button clicked");
#endif

        TrimInputFields();

        if (ValidateEmailInput() && ValidateNicknameInput() && ValidatePasswordInput())
        {
            AuthManager.Instance.Register(emailInput.text, passwordInput.text, nicknameInput.text);
        }

        ClearInputFields();
    }

    private void TrimInputFields()
    {
        emailInput.text = emailInput.text.Trim();
        nicknameInput.text = nicknameInput.text.Trim();
        passwordInput.text = passwordInput.text.Trim();
    }

    private void ClearInputFields()
    {
        emailInput.text = string.Empty;
        nicknameInput.text = string.Empty;
        passwordInput.text = string.Empty;
    }

    private bool ValidatePasswordInput()
    {
        if (string.IsNullOrEmpty(passwordInput.text))
        {
            LoginUIController.Instance.ShowNoticePopup("Password is required.");
            return false;
        }

        if(passwordInput.text.Length < 8 || passwordInput.text.Length > 20)
        {
            LoginUIController.Instance.ShowNoticePopup("Password must be 8–20 characters.");
            return false;
        }

        return true;
    }

    private bool ValidateNicknameInput()
    {
        if (string.IsNullOrEmpty(nicknameInput.text))
        {
            LoginUIController.Instance.ShowNoticePopup("Nickname is required.");
            return false;
        }

        if(nicknameInput.text.Length < 3 || nicknameInput.text.Length > 20)
        {
            LoginUIController.Instance.ShowNoticePopup("Nickname must be 3–20 characters.");
            return false;
        }

        return true;
    }

    private bool ValidateEmailInput()
    {
        if (string.IsNullOrEmpty(emailInput.text))
        {
            LoginUIController.Instance.ShowNoticePopup("Email is required.");
            return false;
        }

        if(!emailInput.text.Contains("@") || !emailInput.text.Contains("."))
        {
            LoginUIController.Instance.ShowNoticePopup("Invalid email format.");
            return false;
        }

        if(emailInput.text.Length < 5 || emailInput.text.Length > 30)
        {
            LoginUIController.Instance.ShowNoticePopup("Email must be 5–30 characters.");
            return false;
        }

        return true;
    }
}
