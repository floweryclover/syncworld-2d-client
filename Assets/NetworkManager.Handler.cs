using SyncWorld2DProtocol.Stc;
using UnityEngine;

public partial class NetworkManager
{
    public bool OnSpawnEntity(uint entityId, float x, float y)
    {
        EntityManager.SpawnEntity(entityId, new Vector2(x, y));
        return true;
    }

    public bool OnDespawnEntity(uint entityId)
    {
        EntityManager.DespawnEntity(entityId);
        return true;
    }

    public bool OnPossessEntity(uint entityId)
    {
        EntityManager.PossessEntity(entityId);
        return true;
    }

    public bool OnUnpossessEntity()
    {
        EntityManager.UnpossessEntity();
        return true;
    }

    public bool OnMoveEntity(uint entityId, float x, float y)
    {
        EntityManager.MoveEntity(entityId, x, y);
        return true;
    }

    public bool OnAssignEntityColor(uint entityId, float r, float g, float b)
    {
        EntityManager.AssignEntityColor(entityId, r, g, b);
        return true;
    }

    public bool OnTeleportEntity(uint entityId, float x, float y)
    {
        EntityManager.TeleportEntity(entityId, x, y);
        return true;
    }

    private void ProcessReceive()
    {
        if (_tcpClient == null || !_tcpClient.Connected || _tcpClient.Available == 0)
        {
            return;
        }

        var receivedSize = _tcpClient.Client.Receive(_receiveRingBuffer.WritableOnceSpan);
        _receiveRingBuffer.UpdateWritten(receivedSize);
        (this as IStcHandler).Handle(_receiveRingBuffer);
    }
}