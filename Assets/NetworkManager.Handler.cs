using SyncWorld2DProtocol.Stc;
using UnityEngine;

public partial class NetworkManager
{
    public GameObject playerPrefab;
    public bool OnSpawnEntity(uint entityId, float x, float y)
    {
        Instantiate(playerPrefab, new Vector2(x, y), Quaternion.identity);
        return true;
    }

    public bool OnDespawnEntity(uint entityId)
    {
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