using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class EntityManager : MonoBehaviour
{
    private static EntityManager _singleton;
    
    public GameObject playerPrefab;
    
    private Dictionary<uint, SyncPlayerCharacter> _entities;
    private SyncPlayerController _syncPlayerController;

    private void Start()
    {
        Assert.IsNull(_singleton);
        _singleton = this;
        _entities = new Dictionary<uint, SyncPlayerCharacter>();
        _syncPlayerController = GameObject.Find("SyncPlayerController").GetComponent<SyncPlayerController>();
        
        NetworkManager.RequestJoin();
    }
    
    public static void SpawnEntity(uint entityId, Vector2 position)
    { 
        var spawned = Instantiate(_singleton.playerPrefab, position, Quaternion.identity);
        _singleton._entities.Add(entityId, spawned.GetComponent<SyncPlayerCharacter>());
    }

    public static void DespawnEntity(uint entityId)
    {
        if (!_singleton._entities.TryGetValue(entityId, out var entity))
        {
            return;
        }
        
        Destroy(entity.ConvertTo<GameObject>());
    }

    public static void PossessEntity(uint entityId)
    {
        if (!_singleton._entities.TryGetValue(entityId, out var entity))
        {
            throw new InvalidDataException($"Entity {entityId}(을)를 찾을 수 없습니다.");
        }

        var syncPlayerCharacterComponent = entity.ConvertTo<GameObject>().GetComponent<SyncPlayerCharacter>();
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
            character.GetComponent<SpriteRenderer>().color = Color.white;
            character.DetachController();
        }
        _singleton._syncPlayerController.DetachCharacter();
    }

    public static void MoveEntity(uint entityId, float x, float y)
    {
        if (!_singleton._entities.TryGetValue(entityId, out var entity))
        {
            return;
        }
        
        entity.MoveTo(x, y);
    }

    public static void AssignEntityColor(uint entityId, float r, float g, float b)
    {
        if (!_singleton._entities.TryGetValue(entityId, out var entity))
        {
            return;
        }
        entity.ConvertTo<GameObject>().GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }
}
