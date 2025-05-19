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
        skeleton.initialSkinName = "S01";
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

            // TODO : ĳ���� ������ �����ͼ� ����
            int maxHp = charData.maxHp != 0 ? charData.maxHp : 100;
            playerInfo.maxHp = maxHp;
            playerInfo.hp = maxHp;

            // �÷��̾� �� ����
            playerInfo.SetTeam(playerId);
        }


        // ��Ʈ�ѷ� ���� & MatchManager ����
        if (UserDataManager.Instance._userInfo.userId == playerId) 
        {
            player.AddComponent<MyPlayerController>();
            MatchManager.Instance.myPlayer = player;

            // ī�޶� ����
            Camera.main.transform.SetParent(player.transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, -10);

            // ����ü ���̵� ����
            GameObject attackGuide = Instantiate(charData.attackGuidePrefab, player.transform);
            attackGuide.transform.localPosition = new Vector3(0, 2, 0);
            player.GetComponent<MyPlayerController>().attackGuide = attackGuide;
            player.GetComponent<MyPlayerController>().attackGuide.SetActive(false);

        }
        else
        {
            player.AddComponent<NetworkPlayerController>();
            MatchManager.Instance.otherPlayers.Add(player);
        }

        return player;
    }
}
