﻿using SyncWorld2DProtocol;
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

    public static void RequestJoin()
    {
        var message = new RequestJoinMessage();
        Serializer.TrySerializeTo(_singleton._headerSerializationBuffer, Protocol.CtsRequestJoin, ref message, _singleton._sendRingBuffer);
    }

    public static void SendCurrentPosition(float positionX, float positionY, float velocityX, float velocityY, float accelerationX, float accelerationY)
    {
        var message = new SendCurrentPositionMessage() { PositionX = positionX, PositionY = positionY, VelocityX = velocityX, VelocityY = velocityY, AccelerationX = accelerationX, AccelerationY = accelerationY};
        Serializer.TrySerializeTo(_singleton._headerSerializationBuffer, Protocol.CtsSendCurrentPosition, ref message,
            _singleton._sendRingBuffer);
    }
}