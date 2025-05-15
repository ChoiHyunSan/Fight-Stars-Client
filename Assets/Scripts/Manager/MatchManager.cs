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
        // �÷��̾ �̵� �����ϵ��� ����
        myPlayer.GetComponent<MyPlayerController>().ActiveControll();
        myPlayer.GetComponent<PlayerController>().isServerUpdateReceived = true;
        foreach (var player in otherPlayers)
        {
            player.GetComponent<PlayerController>().isServerUpdateReceived = true;
        }

        // ���� UI�� ���¸� ������Ʈ
    }

    public void UpdatePosition(RepeatedField<S_PositionUpdate.Types.PlayerPosUpdate> playerPosUpdates)
    {
        foreach (var playerPosUpdate in playerPosUpdates)
        {
            GameObject player = FindPlayerById(playerPosUpdate.UserId);
            if (player != null)
            {
                // �÷��̾��� ��ġ & �ӵ� ������Ʈ
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

        // ����ü ����
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
        // �ǰݴ��� �÷��̾� ã��
        GameObject targetPlayer = FindPlayerById(attackPacket.UserId);
        if(targetPlayer == null)
        {
            return;
        }

        // �÷��̾� ü�� ������Ʈ


        // ����ü ����


    }

    public void UpdateDie(S_Die diePacket)
    {
        // ���� �÷��̾� ã��
        GameObject deadPlayer = FindPlayerById(diePacket.DieUserId);
        if (deadPlayer == null)
        {
            return;
        }

        // �÷��̾� ��� ó��


        // ��� �ִϸ��̼� ���
    

        // ���ھ� ����
    }

    public void UpdateRespawn(S_Respawn respawnPacket)
    {
        // ����� �÷��̾� ã��
        GameObject respawnPlayer = FindPlayerById(respawnPacket.UserId);
        if (respawnPlayer == null)
        {
            return;
        }

        // �÷��̾� ��ġ �ʱ�ȭ


        // �÷��̾� ü�� �ʱ�ȭ


        // �÷��̾� Ȱ��ȭ

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
