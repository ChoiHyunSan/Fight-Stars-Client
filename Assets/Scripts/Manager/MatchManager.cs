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
        // 플레이어가 이동 가능하도록 설정
        myPlayer.GetComponent<MyPlayerController>().ActiveControll();
        myPlayer.GetComponent<PlayerController>().isServerUpdateReceived = true;
        foreach (var player in otherPlayers)
        {
            player.GetComponent<PlayerController>().isServerUpdateReceived = true;
        }

        // 관련 UI나 상태를 업데이트
        GameObject.Find("Timer").GetComponent<TimerUI>().StartGame(100f);
    }

    public void OnUpdatePosition(RepeatedField<S_PositionUpdate.Types.PlayerPosUpdate> playerPosUpdates)
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

    public void OnFire(S_Fire firePacket)
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
        PlayerInfo playerInfo = targetPlayer.GetComponent<PlayerInfo>();
        if(playerInfo != null)
        {
            playerInfo.UpdateHp(attackPacket.Hp);
        }
    }

    public void OnDie(S_Die diePacket)
    {
        // 죽은 플레이어 찾기
        GameObject deadPlayer = FindPlayerById(diePacket.DieUserId);
        if (deadPlayer == null)
        {
            return;
        }

        // 플레이어 사망 처리
#if UNITY_EDITOR
        Debug.Log($"Player {diePacket.DieUserId} has died.");
#endif
        // 내 플레이어가 죽은 경우엔 그 자리에 반투명으로 처리하고 조작하지 못하도록 세팅
        if (myPlayer != null && deadPlayer == myPlayer)
        {
            // 사망 처리
            deadPlayer.GetComponent<MyPlayerController>().DeactiveControll();
        }
        deadPlayer.GetComponent<PlayerController>().OnDead();


        UpdateScore(diePacket.Score.Red, diePacket.Score.Blue);
    }

    public TMP_Text redScore;
    public TMP_Text blueScore;
    private void UpdateScore(int red, int blue)
    {
        // 스코어 갱신
        score.red = red;
        score.blue = blue;

        // UI 업데이트
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
        // 사망한 플레이어 찾기
        GameObject respawnPlayer = FindPlayerById(respawnPacket.UserId);
        if (respawnPlayer == null)
        {
            return;
        }

        // 플레이어 위치 초기화
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
        // 게임 종료 팝업 띄우기
        GameObject popup = GameObject.Find("Popup_Result");
        if(popup != null)
        {
            popup.SetActive(true);
        }

        // 클라이언트 데이터 정보를 갱신
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
