using Spine.Unity;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public int characterId;
    public int skinId;
    public SkeletonDataAsset skeletonDataAsset;
    public GameObject attackEffectPrefab;
}