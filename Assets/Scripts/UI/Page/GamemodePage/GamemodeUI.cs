using UnityEngine;
using UnityEngine.UI;

public class GamemodeUI : MonoBehaviour
{
    private Button backButton;
    private Button Mode01Button;
    private Button Mode02Button;

    void Start()
    {
        backButton = transform.Find("Top/Button_Back")?.GetComponent<Button>();
        Mode01Button = transform.Find("ScrollRect/Gamemode/Mode01")?.GetComponent<Button>();
        Mode02Button = transform.Find("ScrollRect/Gamemode/Mode02")?.GetComponent<Button>();

#if UNITY_EDITOR
        if (backButton == null || Mode01Button == null || Mode02Button == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            return;
        }
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager Instance is null");
            return;
        }
        if(GameSceneManager.Instance == null)
        {
            Debug.LogError("SceneManager Instance is null");
            return;
        }
#endif

        backButton.onClick.AddListener(OnBackButtonClicked);
        Mode01Button.onClick.AddListener(OnMode01ButtonClicked);
        Mode02Button.onClick.AddListener(OnMode02ButtonClicked);

        Mode02Button.interactable = true;
    }

    private void OnMode02ButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Mode02Button Clicked");
#endif
        GameManager.Instance.SetGamemode(Gamemode.Deathmatch);
        GameSceneManager.Instance.LoadScene(SceneType.Lobby);
    }

    private void OnMode01ButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Mode01Button Clicked");
#endif
        GameManager.Instance.SetGamemode(Gamemode.Occupation);
        GameSceneManager.Instance.LoadScene(SceneType.Lobby);
    }

    private void OnBackButtonClicked()
    {
        GameSceneManager.Instance.LoadScene(SceneType.Lobby);
    }
}
