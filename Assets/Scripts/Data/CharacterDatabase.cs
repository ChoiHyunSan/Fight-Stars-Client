using Spine.Unity;
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
}