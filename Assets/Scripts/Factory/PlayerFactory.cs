using Spine.Unity;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    public static PlayerFactory Instance;

    public void Awake()
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

    [Header("Prefabs")]
    public GameObject characterBasePrefab;

    [Header("Data")]
    public CharacterDatabase characterDatabase;

    public GameObject CreatePlayer(int playerId, int characterId, int skinId, string nickname, Vector2 spawnPosition)
    {
        var charData = characterDatabase.GetCharacterData(characterId, skinId);
        if (charData == null)
        {
            Debug.LogError($"CharacterData not found: {characterId}");
            return null;
        }

        GameObject player = Instantiate(characterBasePrefab, spawnPosition, Quaternion.identity);
        player.name = $"Player_{playerId}";

        // ���� ����
        var skeleton = player.transform.Find("Visual").GetComponent<SkeletonAnimation>();
        skeleton.skeletonDataAsset = charData.skeletonDataAsset;
        skeleton.Initialize(true);

        // ����Ʈ ������ ����
        var animationFSM = player.transform.Find("Visual").GetComponent<PlayerAnimationFSM>();
        if(animationFSM != null)
        {
            animationFSM.enabled = true;
            animationFSM.attackEffectPrefab = charData.attackEffectPrefab;
        }

        // PlayerInfo ����
        var playerInfo = player.GetComponent<PlayerInfo>();
        if(playerInfo != null)
        { 
            playerInfo.userId = playerId;
            playerInfo.nickname = nickname;
        }

        // ��Ʈ�ѷ� ���� & MatchManager ����
        if (UserDataManager.Instance._userInfo.userId == playerId) 
        {
            player.AddComponent<MyPlayerController>();
            MatchManager.Instance.myPlayer = player;
        }
        else
        {
            player.AddComponent<NetworkPlayerController>();
            MatchManager.Instance.otherPlayers.Add(player);
        }

        return player;
    }
}
