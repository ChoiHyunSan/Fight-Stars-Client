using Spine.Unity;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public int characterId;
    public int skinId;
    public Sprite profileImage;
    public SkeletonDataAsset skeletonDataAsset;
    public GameObject attackEffectPrefab;
    public GameObject attackProjectilePrefab;
    public GameObject attackGuidePrefab;
}