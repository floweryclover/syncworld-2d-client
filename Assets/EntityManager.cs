using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class EntityManager : MonoBehaviour
{
    private static EntityManager _singleton;
    
    public GameObject playerPrefab;
    
    private Dictionary<uint, GameObject> _entities;
    private SyncPlayerController _syncPlayerController;

    private void Start()
    {
        Assert.IsNull(_singleton);
        _singleton = this;
        _entities = new Dictionary<uint, GameObject>();
        _syncPlayerController = GameObject.Find("SyncPlayerController").GetComponent<SyncPlayerController>();
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

    public static void PossessEntity(uint entityId)
    {
        if (!_singleton._entities.TryGetValue(entityId, out var entity))
        {
            throw new InvalidDataException($"Entity {entityId}(을)를 찾을 수 없습니다.");
        }

        var syncPlayerCharacterComponent = entity.GetComponent<SyncPlayerCharacter>();
        if (syncPlayerCharacterComponent == null)
        {
            Debug.LogError($"Entity {entityId}(은)는 SyncPlayerCharacter가 아니므로 빙의할 수 없습니다.");
            return;
        }
        
        _singleton._syncPlayerController.AttachCharacter(syncPlayerCharacterComponent);
        syncPlayerCharacterComponent.AttachController(_singleton._syncPlayerController);
    }

    public static void UnpossessEntity()
    {
        if (_singleton._syncPlayerController.TryGetCharacter(out var character))
        {
            character.DetachController();
        }
        _singleton._syncPlayerController.DetachCharacter();
    }
}
