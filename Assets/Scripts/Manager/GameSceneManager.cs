
using System;
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
            case SceneType.Gamemode:
                SceneManager.LoadScene("GamemodePage");
                break;
            case SceneType.Matching:
                SceneManager.LoadScene("MatchingPage");
                break;
            default:
#if UNITY_EDITOR
                Debug.LogError("Invalid scene type");
#endif
                break;
        }
    }

    public void LoadScene(Gamemode gamemode)
    {
        switch(gamemode)
        {
            case Gamemode.Deathmatch:
                SceneManager.LoadScene("Deathmatch");
                break;
            case Gamemode.Occupation:
                SceneManager.LoadScene("Occupation");
                break;
            default:
#if UNITY_EDITOR
                Debug.LogError("Invalid gamemode");
#endif
                break;
        }
    }
}