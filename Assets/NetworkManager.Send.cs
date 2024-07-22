public partial class NetworkManager
{
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

        int sentSize = _tcpClient.Client.Send(_sendRingBuffer.ReadableOnceSpan);
        _sendRingBuffer.UpdateRead(sentSize);
    }
}