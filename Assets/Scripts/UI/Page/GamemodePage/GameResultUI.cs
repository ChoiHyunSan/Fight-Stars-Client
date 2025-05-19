using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    private TMP_Text resultText;
    private Button backToLobbyButton;

    public GameObject resultUI;

    private void Start()
    {
        resultText = transform.Find("UI/Popup/Text_Message").GetComponent<TMP_Text>();
        backToLobbyButton = transform.Find("UI/Popup/Button_BackToLobby").GetComponent<Button>();
        if(resultText == null || backToLobbyButton == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Failed to find UI elements in GameResultUI.");
#endif
            return;
        }
        backToLobbyButton.onClick.AddListener(OnBackToLobbyButtonClicked);
    }

    private void OnBackToLobbyButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Back to Lobby button clicked.");
#endif

        // TODO : API 서버로부터 갱신된 유저 정보를 받아야 한다. (ex. 승패 정보, 랭크 정보, 경험치 등)


        // TODO : 받아온 정보를 UserDataManager에 저장한다.

        GameSceneManager.Instance.LoadScene(SceneType.Lobby);
    }

    public void ShowResult(int redScore, int blueScore)
    {
        resultUI.SetActive(true);   

        resultText.text = redScore > blueScore 
            ? "Red Team Wins!" 
            : "Blue Team Wins!";
    }
}