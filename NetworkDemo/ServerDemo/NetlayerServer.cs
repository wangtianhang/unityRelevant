using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Collections.Concurrent;

class NetlayerServer
{
    Socket m_socket = null;

    public delegate void AcceptCallback(int channelId);
    AcceptCallback m_accpetCallback = null;

    int m_channelId = 0;
    class ChannelInfo
    {
        public Socket m_socket = null;
        //public 
    }

    Dictionary<int, Socket> m_channelDic = new Dictionary<int, Socket>();

    public NetlayerServer()
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
    }

    public void Bind(string ip, int port)
    {
        EndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);

        try
        {
            m_socket.Bind(remoteEP);

            Console.WriteLine("bind end");
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Bind " + ex);
            throw ex;
        }
    }

    public void Listen(int backlog)
    {
        try
        {
            m_socket.Listen(backlog);

            Console.WriteLine("Listen end");
        }
        catch(SocketException ex)
        {
            Console.WriteLine("Listen " + ex);
            throw ex;
        }
    }

    public void AcceptAsync(AcceptCallback callback)
    {
        Console.WriteLine("AcceptAsync ");

        SocketAsyncEventArgs acceptArgs = new SocketAsyncEventArgs();
        acceptArgs.Completed += OnAcceptComplete;
        //acceptArgs.RemoteEndPoint = m_remoteEP;
        m_socket.ConnectAsync(acceptArgs);

        m_socket.AcceptAsync(acceptArgs);
    }

    void OnAcceptComplete(object sender, SocketAsyncEventArgs e)
    {
        Console.WriteLine("OnAcceptComplete " + e.SocketError);
        
        if(e.SocketError == SocketError.Success)
        {
            m_channelId++;
            m_channelDic.Add(m_channelId, e.AcceptSocket);

            m_accpetCallback(m_channelId);
        }

        AcceptAsync(m_accpetCallback);
    }
}

