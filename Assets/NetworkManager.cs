using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton = null;
    public static NetworkManager Instance => _singleton;

    private readonly object _networkThreadLock = new object();
    private Thread _networkThread;
    
    private void Start()
    {
        Assert.IsNull(_singleton );
        _singleton = this;
        
        ConnectToServer("127.0.0.1", 31415);
    }

    public void ConnectToServer(string serverAddress, int serverPort)
    {
        lock (_networkThreadLock)
        {
            if (_networkThread != null)
            {
                return;
            }
            _networkThread = new Thread(() => NetworkThreadBody(serverAddress, serverPort))
            {
                Name = "NetworkThread"
            };
            _networkThread.Start();
        }
    }

    // 이하 NetworkThread 메소드
    private void NetworkThreadBody(string serverAddress, int serverPort)
    {
        var tcpClient = new TcpClient();
        var ipAddress = IPAddress.Parse(serverAddress);
        try
        {
            tcpClient.Connect(ipAddress, serverPort);
            Debug.Log("접속 성공!");
        }
        catch (SocketException e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            tcpClient.Close();
            tcpClient.Dispose();

            lock (_networkThreadLock)
            {
                _networkThread = null;
            }
        }
    }

    private void NetworkIoLoop()
    {
        
    }
}
