using Google.Protobuf.Collections;
using Google.Protobuf.Protocol;
using System;
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
        myPlayer.GetComponent<PlayerController>().isServerUpdateReceived = true;
        foreach (var player in otherPlayers)
        {
            player.GetComponent<PlayerController>().isServerUpdateReceived = true;
        }

        // 관련 UI나 상태를 업데이트
    }

    public void UpdatePosition(RepeatedField<S_PositionUpdate.Types.PlayerPosUpdate> playerPosUpdates)
    {
        foreach (var playerPosUpdate in playerPosUpdates)
        {
            GameObject player = FindPlayerById(playerPosUpdate.UserId);
            if (player != null)
            {
                // 플레이어의 위치 & 속도 업데이트
                Vector3 newPosition = new Vector3(playerPosUpdate.X, playerPosUpdate.Y, 0);
                Vector2 newVelocity = new Vector2(playerPosUpdate.Vx, playerPosUpdate.Vy);
                player.transform.GetComponent<PlayerController>().SetPosition(newPosition, newVelocity);
            }
        }
    }

    private GameObject FindPlayerById(int userId)
    {
        if(myPlayer != null)
        {
            PlayerInfo playerController = myPlayer.GetComponent<PlayerInfo>();
            if (playerController != null && playerController.userId == userId)
            {
                return myPlayer;
            }
        }

        if(otherPlayers != null)
        {
            foreach (var player in otherPlayers)
            {
                PlayerInfo playerController = player.GetComponent<PlayerInfo>();
                if (playerController != null && playerController.userId == userId)
                {
                    return player;
                }
            }
        }

        return null;
    }

    internal void UpdateFire(S_Fire firePacket)
    {
        throw new NotImplementedException();
    }

    internal void UpdateAttack(S_Attack attackPacket)
    {
        throw new NotImplementedException();
    }

    internal void UpdateDie(S_Die diePacket)
    {
        throw new NotImplementedException();
    }

    internal void UpdateRespawn(S_Respawn respawnPacket)
    {
        throw new NotImplementedException();
    }
}
