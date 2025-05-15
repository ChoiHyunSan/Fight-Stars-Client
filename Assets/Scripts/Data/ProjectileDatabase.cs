

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileDatabase", menuName = "Game/ProjectileDatabase")]
public class ProjectileDatabase : ScriptableObject
{
    public List<GameObject> projectileList;
}