using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

class NetlayerClient
{
    Socket m_socket = null;

    public delegate void ConnectCallback(SocketError error);
    ConnectCallback m_connectCallback = null;

    public delegate void SendCallback(SocketError error);
    SendCallback m_sendCallback = null;

    public delegate void ReceiveCallback(SocketError error, Stream stream);
    ReceiveCallback m_receiveCallback = null;

    public delegate void OnDisconnectCallback();
    public event OnDisconnectCallback m_disconnectEvent;

    public NetlayerClient()
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
    }

    public void ConnectAsync(ConnectCallback callback)
    {

    }

    public void SendAsync(Stream data, SendCallback callback)
    {

    }

    public void ReceiveAsync(ReceiveCallback callback)
    {

    }

    void OnDisconnect()
    {

    }
}

