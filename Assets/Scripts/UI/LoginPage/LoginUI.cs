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
        passwordInput = transform.Find("Popup/InputField_Password")?.GetComponent< TMP_InputField>();
        loginButton = transform.Find("Popup/Button_Login")?.GetComponent<Button>();
        registerButton = transform.Find("Popup/Button_SignUp")?.GetComponent<Button>();

        if(nicknameInput == null || passwordInput == null || loginButton == null || registerButton == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            return;
        }   

        loginButton.onClick.AddListener(OnLoginButtonClicked);
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
    }

    private void OnRegisterButtonClicked()
    {
        Debug.Log("Register button clicked");

        LoginUIManager.Instance.ShowRegisterPopup();
    }

    private void OnLoginButtonClicked()
    {
        Debug.Log("Login button clicked");

        // TODO : 입력값 검증

        AuthManager.Instance.Login(nicknameInput.text, passwordInput.text);
    }
}
