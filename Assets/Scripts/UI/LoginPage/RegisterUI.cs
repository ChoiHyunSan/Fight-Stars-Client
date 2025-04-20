using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        if(emailInput == null || nicknameInput == null || passwordInput == null || signupButton == null || loginButton == null)
        { 
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            Debug.LogError($"emailInput: {emailInput}, nicknameInput: {nicknameInput}, passwordInput: {passwordInput}, signupButton: {signupButton}, loginButton: {loginButton}");
            return;
        }

        signupButton.onClick.AddListener(OnSignupButtonClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        Debug.Log("Login button clicked. Move To LoginUI");

        LoginUIManager.Instance.ShowLoginPopup();
    }

    private void OnSignupButtonClicked()
    {
        Debug.Log("Signup button clicked");

        // TODO : 입력값 검증

        AuthManager.Instance.Register(emailInput.text, passwordInput.text, nicknameInput.text);
    }
}
