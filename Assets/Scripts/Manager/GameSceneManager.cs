
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    public static SceneType currentScene { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 초기화 코드
    }

    public void LoadScene(SceneType sceneType)
    {
        currentScene = sceneType;

        switch (sceneType)
        {
            case SceneType.Title:
                SceneManager.LoadScene("TitlePage");
                break;
            case SceneType.Login:
                SceneManager.LoadScene("LoginPage");
                break;
            case SceneType.Loading:
                SceneManager.LoadScene("LoadingPage");
                break;
            case SceneType.Lobby:
                SceneManager.LoadScene("LobbyPage");
                break;
            default:
                Debug.LogError("Invalid scene type");
                break;
        }
    }
}

public enum SceneType
{
    Title,
    Login,
    Loading,
    Lobby,
}