using Spine.Unity;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public int characterId;
    public int skinId;
    public string skinName;
    public Sprite profileImage;
    public SkeletonDataAsset skeletonDataAsset;
    public GameObject attackEffectPrefab;
    public GameObject attackProjectilePrefab;
    public GameObject attackGuidePrefab;

    // ���� ������ ���� ��ġ ������
    public int maxHp = 100;
}