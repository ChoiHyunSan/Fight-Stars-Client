using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private void Awake()
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

    public void Start()
    {
        LoadMap(GameManager.Instance.currentGamemode);
    }

    public void LoadMap(Gamemode mode)
    {
        string mapName = GamemodeHelper.GetGamemodeName(mode);
        GameObject mapPrefab = Resources.Load<GameObject>($"Prefabs/Map/{mapName}");
        if(mapPrefab == null)
        {
            Debug.LogError($"Map {mapName} not found!");
            return;
        }

        GameObject mapInstance = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
        Debug.Log($"Loaded map: {mapName}");
        mapInstance.transform.position = Vector3.zero;
    }
}
