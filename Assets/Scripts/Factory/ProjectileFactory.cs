using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public static ProjectileFactory Instance;

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

    [Header("Data")]
    public ProjectileDatabase projectileDatabase;

    public GameObject CreateProjectile(int projectileType, int projectileId, Vector2 spawnPosition, Vector2 direction, float speed)
    {
        if (projectileType < 0)
        {
#if UNITY_EDITOR
            Debug.LogError($"Invalid projectileId: {projectileType}");
#endif
            return null;
        }
        GameObject projectile = Instantiate(projectileDatabase.projectileList[projectileType], spawnPosition, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(projectileId, direction, speed);
        return projectile;
    }
}