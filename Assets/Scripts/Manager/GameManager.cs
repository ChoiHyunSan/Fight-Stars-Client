using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    public Gamemode currentGamemode;

    [SerializeField]
    public int currentCharacterId;

    [SerializeField]
    public GameServerInfo gameServerInfo;

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

    public void JoinGame(string roomId, string password, string ip, int port)
    {
        // ��Ʈ��ũ ���� �� ���� ���� ����
#if UNITY_EDITOR
        Debug.Log($"Joining game with Room ID: {roomId}, Password: {password}, IP: {ip}, Port: {port}");
#endif
        gameServerInfo = new GameServerInfo
        {
            roomId = roomId,
            password = password,
            ip = ip,
            port = port
        };
        GameSceneManager.Instance.LoadScene(currentGamemode);
    }
}