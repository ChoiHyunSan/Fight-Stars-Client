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

        // TODO : API �����κ��� ���ŵ� ���� ������ �޾ƾ� �Ѵ�. (ex. ���� ����, ��ũ ����, ����ġ ��)


        // TODO : �޾ƿ� ������ UserDataManager�� �����Ѵ�.

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