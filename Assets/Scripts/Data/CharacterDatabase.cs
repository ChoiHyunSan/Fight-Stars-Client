using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Game/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    public List<CharacterData> characterList;

    public CharacterData GetCharacterData(int characterId, int skinId)
    {
        return characterList.Find(c => c.characterId == characterId && c.skinId == skinId);
    }

    public Sprite GetProfileImage(int characterId)
    {
        var characterData = characterList.Find(c => c.characterId == characterId);
        if (characterData != null)
        {
            return characterData.profileImage;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"Profile image not found for characterId: {characterId}");
#endif
            return null;
        }
    }

    internal Sprite GetSkinImage(int id)
    {
        var characterData = characterList.Find(c => c.skinId == id);
        if (characterData != null)
        {
            return characterData.profileImage;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"Skin image not found for skinId: {id}");
#endif
            return null;
        }
    }
}