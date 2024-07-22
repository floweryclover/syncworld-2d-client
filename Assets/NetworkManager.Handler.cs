using SyncWorld2DProtocol.Stc;
using UnityEngine;

public partial class NetworkManager
{
    private class Handler : IStcHandler
    {
        public bool OnHelloClient(string message)
        {
            Debug.Log(message);
            return true;
        }
    }

    private void ProcessReceive()
    {
        if (_tcpClient == null || !_tcpClient.Connected || _tcpClient.Available == 0)
        {
            return;
        }

        var receivedSize = _tcpClient.Client.Receive(_receiveRingBuffer.WritableOnceSpan);
        _receiveRingBuffer.UpdateWritten(receivedSize);
        (_handler as IStcHandler).Handle(_receiveRingBuffer);
    }
}