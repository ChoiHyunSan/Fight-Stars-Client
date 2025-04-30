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
        // 플레이어가 이동 가능하도록 설정
        myPlayer.GetComponent<MyPlayerController>().ActiveControll();

        // 관련 UI나 상태를 업데이트
    }
}
