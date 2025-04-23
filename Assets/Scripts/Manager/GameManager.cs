using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Gamemode currentGamemode;

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
        currentGamemode = Gamemode.Deathmatch; // 기본값 설정
    }

    public void SetGamemode(Gamemode mode)
    {
        currentGamemode = mode;
    }
}