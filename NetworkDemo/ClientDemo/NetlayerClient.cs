using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;

public delegate void ConnectCallback(SocketError error);

class NetlayerClient
{
    Socket m_socket = null;


    ConnectCallback m_connectCallback = null;

    public delegate void SendCallback(SocketError error);
    SendCallback m_sendCallback = null;

    public delegate void ReceiveCallback(SocketError error, Stream stream);
    ReceiveCallback m_receiveCallback = null;

    public delegate void OnDisconnectEvent();
    public event OnDisconnectEvent m_disconnectEvent;

    int m_reconnectCounter = 5;
    bool m_setReconnect = false;

    EndPoint m_remoteEP;

    bool m_setNotifyDisconnect = false;

    EncodeHelper m_encodeHelper = null;
    DecodeHelper m_decodeHelper = null;

    public NetlayerClient()
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
    }

    public void ConnectAsync(string ip, int port, ConnectCallback callback)
    {
        m_remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);

        m_connectCallback = callback;

        m_reconnectCounter = 5;

        _ConnectAsync();
    }

    void _ConnectAsync()
    {
        SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
        connectArgs.Completed += OnConnectCompleted;
        connectArgs.RemoteEndPoint = m_remoteEP;
        m_socket.ConnectAsync(connectArgs);

        m_reconnectCounter--;
    }

    public void OnUpdate(float delta)
    {
        if (m_setReconnect)
        {
            m_setReconnect = false;

            _ConnectAsync();
        }

        if (m_setNotifyDisconnect)
        {
            m_setNotifyDisconnect = false;

            OnDisconnect();
        }
    }

    void OnConnectCompleted(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError == SocketError.Success)
        {
            m_connectCallback(e.SocketError);
        }
        else if(e.SocketError == SocketError.WouldBlock)
        {
            if(m_socket.Poll(-1, SelectMode.SelectWrite))
            {
                m_connectCallback(SocketError.Success);
            }
            else
            {
                if (m_reconnectCounter > 0)
                {
                    m_setReconnect = true;
                }
                else
                {
                    m_connectCallback(e.SocketError);
                }
            }
        }
        else
        {
            if (m_reconnectCounter > 0)
            {
                m_setReconnect = true;
            }
            else
            {
                m_connectCallback(e.SocketError);
            }
        }
    }

    public void SendAsync(Packet packet, SendCallback callback)
    {
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
        sendArgs.Completed += OnSendComplete;
        sendArgs.RemoteEndPoint = m_remoteEP;
        //byte[] bytes = data.get
        byte[] data = m_encodeHelper.Encode(packet);

        sendArgs.SetBuffer(data, 0, data.Length);

        m_socket.SendAsync(sendArgs);
    }

    void OnSendComplete(object sender, SocketAsyncEventArgs e)
    {
        if(e.BytesTransferred == 0)
        {
            m_socket.Close();

            m_setNotifyDisconnect = true;
        }
    }

    public void ReceiveAsync(ReceiveCallback callback)
    {

    }

    void OnDisconnect()
    {
        m_disconnectEvent();
    }
}

