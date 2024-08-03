using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

internal interface IMoveableEntity
{
    void MoveTo(float x, float y);
    void Teleport(float x, float y);
}

public class EntityManager : MonoBehaviour
{
    private static EntityManager _singleton;
    
    public GameObject playerPrefab;
    public GameObject soccerballPrefab;
    
    private Dictionary<uint, GameObject> _entities;
    private Dictionary<uint, IMoveableEntity> _entityMovable;
    private SyncPlayerController _syncPlayerController;

    private void Start()
    {
        Assert.IsNull(_singleton);
        _singleton = this;
        _entities = new Dictionary<uint, GameObject>();
        _entityMovable = new Dictionary<uint, IMoveableEntity>();
        _syncPlayerController = GameObject.Find("SyncPlayerController").GetComponent<SyncPlayerController>();
        
        NetworkManager.RequestJoin();
    }
    
    public static void SpawnEntity(uint entityId, uint entityType, Vector2 position)
    {
        var prefab = entityType == 2 ? _singleton.playerPrefab : _singleton.soccerballPrefab;
        var spawned = Instantiate(prefab, position, Quaternion.identity);
        _singleton._entities.Add(entityId, spawned);
        
        var moveable = spawned.ConvertTo<IMoveableEntity>();
        if (moveable != null)
        {
            _singleton._entityMovable.Add(entityId, moveable);
        }
    }

    public static void DespawnEntity(uint entityId)
    {
        if (!_singleton._entities.TryGetValue(entityId, out var entity))
        {
            return;
        }
        
        Destroy(entity.ConvertTo<GameObject>());
        _singleton._entityMovable.Remove(entityId);
        _singleton._entities.Remove(entityId);
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
        if (!_singleton._entityMovable.TryGetValue(entityId, out var moveableEntity))
        {
            return;
        }
        
        moveableEntity.MoveTo(x, y);
    }

    public static void AssignEntityColor(uint entityId, float r, float g, float b)
    {
        if (!_singleton._entities.TryGetValue(entityId, out var entity))
        {
            return;
        }
        entity.ConvertTo<GameObject>().GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }

    public static void TeleportEntity(uint entityId, float x, float y)
    {
        if (!_singleton._entityMovable.TryGetValue(entityId, out var moveableEntity))
        {
            return;
        }
        moveableEntity.Teleport(x, y);
    }
}
