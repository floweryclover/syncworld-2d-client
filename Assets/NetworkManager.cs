using System;
using System.Net;
using System.Net.Sockets;
using SyncWorld2DProtocol;
using SyncWorld2DProtocol.Stc;
using UnityEngine;
using UnityEngine.Assertions;

public partial class NetworkManager : MonoBehaviour, IStcHandler
{
    private static NetworkManager _singleton;

    private TcpClient _tcpClient;
    private RingBuffer _receiveRingBuffer;
    private RingBuffer _sendRingBuffer;
    
    private void Start()
    {
        Assert.IsNull(_singleton );
        _singleton = this;
        _receiveRingBuffer = new RingBuffer(Protocol.MaxMessageSize * 1024);
        _sendRingBuffer = new RingBuffer(Protocol.MaxMessageSize * 1024);
        
        ConnectToServer("127.0.0.1", 31415);
    }

    private void ConnectToServer(string serverAddress, int serverPort)
    {
        if (_tcpClient != null)
        {
            throw new InvalidOperationException("이미 TCP클라이언트 객체가 존재합니다.");
        }
        
        _receiveRingBuffer.Clear();
        _sendRingBuffer.Clear();
        _tcpClient = new TcpClient();
        var ipAddress = IPAddress.Parse(serverAddress);
        _tcpClient.Connect(ipAddress, serverPort);
    }

    private void Update()
    {
        ProcessReceive();
        ProcessSend();
    }

    private void OnApplicationQuit()
    {
        if (_tcpClient != null)
        {
            _tcpClient.Close();
        }
    }
}
