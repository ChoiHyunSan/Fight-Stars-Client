using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        
    }

    public GameObject myPlayer;
    public List<GameObject> otherPlayers = new List<GameObject>();

    public void StartGame()
    {
        // �÷��̾ �̵� �����ϵ��� ����
        myPlayer.GetComponent<MyPlayerController>().ActiveControll();

        // ���� UI�� ���¸� ������Ʈ
    }
}
