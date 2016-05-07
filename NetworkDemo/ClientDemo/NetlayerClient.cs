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

    public delegate void ReceiveCallback(SocketError error, int msgId, MemoryStream stream);
    ReceiveCallback m_receiveCallback = null;

    public delegate void OnDisconnectEvent();
    public event OnDisconnectEvent m_disconnectEvent;

    int m_reconnectCounter = 5;
    bool m_setReconnect = false;

    EndPoint m_remoteEP;

    bool m_setNotifyDisconnect = false;

    EncodeHelper m_encodeHelper = new EncodeHelper();
    DecodeHelper m_decodeHelper = new DecodeHelper();

    //Object m_receivelock = new object();
    Object m_sendLock = new object();
    List<Packet> m_oneFramePacketList = new List<Packet>();

    public NetlayerClient()
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

        //m_encodeHelper = 
        //m_decodeHelper = 

        //m_lock = 
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

    public void OnUpdate(int delta)
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

        for (int i = 0; i < m_oneFramePacketList.Count; ++i )
        {
            Packet iter = m_oneFramePacketList[i];
            MemoryStream stream = new MemoryStream(iter.m_data);
            m_receiveCallback(SocketError.Success, iter.m_msgId, stream);
        }
        m_oneFramePacketList.Clear();
    }

    void OnConnectCompleted(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError == SocketError.Success)
        {
            m_connectCallback(e.SocketError);

            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += OnReceiveComplete;
            //receiveArgs.RemoteEndPoint = m_remoteEP;
            m_socket.ReceiveAsync(receiveArgs);
        }
        else if(e.SocketError == SocketError.WouldBlock)
        {
            if(m_socket.Poll(-1, SelectMode.SelectWrite))
            {
                m_connectCallback(SocketError.Success);

                SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
                receiveArgs.Completed += OnReceiveComplete;
                //receiveArgs.RemoteEndPoint = m_remoteEP;
                m_socket.ReceiveAsync(receiveArgs);
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

    void OnReceiveComplete(object sender, SocketAsyncEventArgs e)
    {
        //lock (m_receivelock)
        {
            if(e.BytesTransferred == 0)
            {
                m_socket.Close();
                //m_receiveCallback(SocketError.Shutdown, 0, null);
                m_setNotifyDisconnect = true;
            }
            else if(e.SocketError == SocketError.Success)
            {
                byte[] receiveBytes = new byte[e.BytesTransferred];
                Buffer.BlockCopy(e.Buffer, 0, receiveBytes, 0, e.BytesTransferred);
                List<Packet> receivePackets = m_decodeHelper.Decode(receiveBytes);
                m_oneFramePacketList.AddRange(receivePackets);

                SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
                receiveArgs.Completed += OnReceiveComplete;
                //receiveArgs.RemoteEndPoint = m_remoteEP;
                m_socket.ReceiveAsync(receiveArgs);
            }
            else
            {
                m_receiveCallback(e.SocketError, 0, null);
            }
        }
    }

    public void SendAsync(Packet packet, SendCallback callback)
    {
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
        sendArgs.Completed += OnSendComplete;
        //sendArgs.RemoteEndPoint = m_remoteEP;
        //byte[] bytes = data.get
        byte[] data = m_encodeHelper.Encode(packet);

        sendArgs.SetBuffer(data, 0, data.Length);

        m_socket.SendAsync(sendArgs);
    }

    void OnSendComplete(object sender, SocketAsyncEventArgs e)
    {
        lock(m_sendLock)
        {
            if (e.BytesTransferred == 0)
            {
                m_socket.Close();

                m_setNotifyDisconnect = true;
            }
            else if (e.SocketError != SocketError.Success)
            {
                m_sendCallback(e.SocketError);
            }
        }
    }

    public void ReceiveAsync(ReceiveCallback callback)
    {
        m_receiveCallback = callback;
    }

    public bool IsConnected()
    {
        return m_socket.Connected;
    }

    void OnDisconnect()
    {
        m_disconnectEvent();
    }
}

