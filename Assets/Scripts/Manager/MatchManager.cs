using Google.Protobuf.Collections;
using Google.Protobuf.Protocol;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        public int red;
        public int blue;
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
        GameObject.Find("Timer").GetComponent<TimerUI>().StartGame(100f);
    }

    public void OnUpdatePosition(RepeatedField<S_PositionUpdate.Types.PlayerPosUpdate> playerPosUpdates)
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

    public void OnFire(S_Fire firePacket)
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
        PlayerInfo playerInfo = targetPlayer.GetComponent<PlayerInfo>();
        if(playerInfo != null)
        {
            playerInfo.UpdateHp(attackPacket.Hp);
        }
    }

    public void OnDie(S_Die diePacket)
    {
        // ���� �÷��̾� ã��
        GameObject deadPlayer = FindPlayerById(diePacket.DieUserId);
        if (deadPlayer == null)
        {
            return;
        }

        // �÷��̾� ��� ó��
#if UNITY_EDITOR
        Debug.Log($"Player {diePacket.DieUserId} has died.");
#endif
        // �� �÷��̾ ���� ��쿣 �� �ڸ��� ���������� ó���ϰ� �������� ���ϵ��� ����
        if (myPlayer != null && deadPlayer == myPlayer)
        {
            // ��� ó��
            deadPlayer.GetComponent<MyPlayerController>().DeactiveControll();
        }
        deadPlayer.GetComponent<PlayerController>().OnDead();


        UpdateScore(diePacket.Score.Red, diePacket.Score.Blue);
    }

    public TMP_Text redScore;
    public TMP_Text blueScore;
    private void UpdateScore(int red, int blue)
    {
        // ���ھ� ����
        score.red = red;
        score.blue = blue;

        // UI ������Ʈ
        if(redScore == null || blueScore == null)
        { 
            redScore = GameObject.Find("Red").GetComponent<TMP_Text>();
            blueScore = GameObject.Find("Blue").GetComponent<TMP_Text>();
        }
        redScore.text = score.red.ToString();
        blueScore.text = score.blue.ToString();
    }

    public void OnRespawn(S_Respawn respawnPacket)
    {
        // ����� �÷��̾� ã��
        GameObject respawnPlayer = FindPlayerById(respawnPacket.UserId);
        if (respawnPlayer == null)
        {
            return;
        }

        // �÷��̾� ��ġ �ʱ�ȭ
        respawnPlayer.transform.position = new Vector3(respawnPacket.SpawnPos.X, respawnPacket.SpawnPos.Y, 0);
        if(myPlayer != null && respawnPlayer == myPlayer)
        {
            respawnPlayer.GetComponent<MyPlayerController>().ActiveControll();
        }
        respawnPlayer.GetComponent<PlayerController>().OnRespawn();
    }

    public void OnDestroyProjectile(S_DestroyProjectile destroyProjectilePacket)
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

    public void OnGameover(S_Gameover gameoverPacket)
    {
        // ���� ���� �˾� ����
        GameObject popup = GameObject.Find("Popup_Result");
        if(popup != null)
        {
            popup.SetActive(true);
        }

        // Ŭ���̾�Ʈ ������ ������ ����
        S_Gameover.Types.ResultData resultData = gameoverPacket.ResultData;
        if(resultData == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Gameover packet does not contain result data.");
#endif
            return;
        }
        UserDataManager.Instance.UpdateData(resultData);

        GameResultUI resultUI = popup.GetComponent<GameResultUI>();
        if (resultUI != null)
        {
            resultUI.ShowResult(gameoverPacket.RedScore, gameoverPacket.BlueScore);
        }
    }
}
