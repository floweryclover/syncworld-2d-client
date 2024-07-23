using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EntityManager : MonoBehaviour
{
    private static EntityManager _singleton;
    
    public GameObject playerPrefab;
    
    private Dictionary<uint, GameObject> _entities;

    private void Start()
    {
        Assert.IsNull(_singleton);
        _singleton = this;
        _entities = new Dictionary<uint, GameObject>();
    }
    
    public static void SpawnEntity(uint entityId, Vector2 position)
    { 
        var spawned = Instantiate(_singleton.playerPrefab, position, Quaternion.identity);
        _singleton._entities.Add(entityId, spawned);
    }

    public static void DespawnEntity(uint entityId)
    {
        if (!_singleton._entities.TryGetValue(entityId, out var entity))
        {
            return;
        }
        
        Destroy(entity);
    }
}
