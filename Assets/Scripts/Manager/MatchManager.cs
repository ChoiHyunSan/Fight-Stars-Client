using Google.Protobuf.Collections;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
    public Score score = new Score();
    public List<GameObject> projectiles = new List<GameObject>();

    public class Score
    {
        int red;
        int blue;
    }

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

    public void UpdateFire(S_Fire firePacket)
    {
        Vector2 spawnPos = new Vector2(firePacket.X, firePacket.Y);
        Vector2 direction = new Vector2(firePacket.Vx, firePacket.Vy);

        // 투사체 생성
        GameObject projectile = ProjectileFactory.Instance.CreateProjectile(
            firePacket.ProjectileInfo.Type,
            firePacket.ProjectileInfo.ProjectileId,
            spawnPos, 
            direction,
            firePacket.ProjectileInfo.Speed);
        if(projectile == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Failed to create projectile.");
#endif
            return;
        }

        projectiles.Add(projectile);
    }

    public void UpdateAttack(S_Attack attackPacket)
    {
        // 피격당한 플레이어 찾기
        GameObject targetPlayer = FindPlayerById(attackPacket.UserId);
        if(targetPlayer == null)
        {
            return;
        }

        // 플레이어 체력 업데이트


        // 투사체 삭제


    }

    public void UpdateDie(S_Die diePacket)
    {
        // 죽은 플레이어 찾기
        GameObject deadPlayer = FindPlayerById(diePacket.DieUserId);
        if (deadPlayer == null)
        {
            return;
        }

        // 플레이어 사망 처리


        // 사망 애니메이션 재생
    

        // 스코어 갱신
    }

    public void UpdateRespawn(S_Respawn respawnPacket)
    {
        // 사망한 플레이어 찾기
        GameObject respawnPlayer = FindPlayerById(respawnPacket.UserId);
        if (respawnPlayer == null)
        {
            return;
        }

        // 플레이어 위치 초기화


        // 플레이어 체력 초기화


        // 플레이어 활성화

    }

    public void UpdateDestroyProjectile(S_DestroyProjectile destroyProjectilePacket)
    {
        int projectileId = destroyProjectilePacket.ProjectileId;
        foreach(GameObject projectile in projectiles)
        {
            if (projectile.GetComponent<Projectile>()._id == projectileId)
            {
#if UNITY_EDITOR
                Debug.Log($"Destroying projectile with ID: {projectileId}");
#endif
                Destroy(projectile);
                projectiles.Remove(projectile);
                break;
            }
        }
    }
}
