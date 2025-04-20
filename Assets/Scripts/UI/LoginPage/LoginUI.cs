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
        Debug.Log($"Nickname: {nicknameInput.text}, Password: {passwordInput.text}");

        LoginRequest loginRequest = new LoginRequest
        {
            Username = nicknameInput.text,
            Password = passwordInput.text
        };

        StartCoroutine(AuthApi.Login(loginRequest,
            (LoginResponse res) =>
            {
                Debug.Log("Login successful");

                // JWT 토큰 저장
                PlayerPrefs.SetString("jwt", res.accessToken);
                PlayerPrefs.SetString("refresh_token", res.refreshToken);

                // 데이터 로딩 화면으로 이동
                SceneManager.LoadScene("LoadingPage");
            },
            (string err) =>
            {
                Debug.Log($"Login failed: {err}");

                // TODO : 실패 이유에 따라서 UI에 에러 메시지 출력
                // 예: "유효하지 않은 ID입니다. 다시 시도해주세요."
            }));
    }
}
