using SyncWorld2DProtocol;
using SyncWorld2DProtocol.Cts;

public partial class NetworkManager
{
    private byte[] _headerSerializationBuffer = new byte[Protocol.HeaderSize];
    private void ProcessSend()
    {
        if (_tcpClient is { Connected: false })
        {
            return;
        }

        if (_sendRingBuffer.ReadableTotalSize == 0)
        {
            return;
        }
        
        var sentSize = _tcpClient.Client.Send(_sendRingBuffer.ReadableOnceSpan);
        _sendRingBuffer.UpdateRead(sentSize);
    }

    public void RequestJoin()
    {
        var message = new RequestJoinMessage();
        Serializer.TrySerializeTo(_headerSerializationBuffer, Protocol.CtsRequestJoin, ref message, _sendRingBuffer);
    }
}