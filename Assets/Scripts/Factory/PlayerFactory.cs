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

        // 외형 설정
        var skeleton = player.transform.Find("Visual").GetComponent<SkeletonAnimation>();
        skeleton.skeletonDataAsset = charData.skeletonDataAsset;
        skeleton.initialSkinName = "S01";
        skeleton.Initialize(true);

        // 이펙트 프리팹 설정
        var animationFSM = player.transform.Find("Visual").GetComponent<PlayerAnimationFSM>();
        if(animationFSM != null)
        {
            animationFSM.enabled = true;
            animationFSM.attackEffectPrefab = charData.attackEffectPrefab;
        }

        // PlayerInfo 설정
        var playerInfo = player.GetComponent<PlayerInfo>();
        if(playerInfo != null)
        { 
            playerInfo.userId = playerId;
            playerInfo.nickname = nickname;

            // TODO : 캐릭터 정보를 가져와서 설정
            int maxHp = charData.maxHp != 0 ? charData.maxHp : 100;
            playerInfo.maxHp = maxHp;
            playerInfo.hp = maxHp;

            // 플레이어 팀 설정
            playerInfo.SetTeam(playerId);
        }


        // 컨트롤러 부착 & MatchManager 설정
        if (UserDataManager.Instance._userInfo.userId == playerId) 
        {
            player.AddComponent<MyPlayerController>();
            MatchManager.Instance.myPlayer = player;

            // 카메라 설정
            Camera.main.transform.SetParent(player.transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, -10);

            // 투사체 가이드 설정
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
