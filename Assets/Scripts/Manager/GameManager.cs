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
        // �ʱ�ȭ �ڵ�
        currentGamemode = Gamemode.Deathmatch; // �⺻�� ����
    }

    public void SetGamemode(Gamemode mode)
    {
        currentGamemode = mode;
    }
}