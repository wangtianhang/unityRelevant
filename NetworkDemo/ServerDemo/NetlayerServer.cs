using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Collections.Concurrent;
using System.IO;

class NetlayerServer
{
    Socket m_listenSocket = null;

    public delegate void AcceptCallback(int channelId);
    AcceptCallback m_accpetCallback = null;

    public delegate void ReceiveCallback(int channelId, Packet packet);
    ReceiveCallback m_receiveCallback = null;

    int m_channelIdGenerator = 0;
    class ChannelInfo
    {
        public int m_channelId = 0;
        public Socket m_socket = null;
        public DecodeHelper m_decodeHelper = new DecodeHelper(); 
    }

    EncodeHelper m_encodeHelper = new EncodeHelper();

    ConcurrentDictionary<int, ChannelInfo> m_channelConcurrentDic = new ConcurrentDictionary<int, ChannelInfo>();

    public NetlayerServer()
    {
        m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_listenSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        m_listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
    }

    public void Bind(string ip, int port)
    {
        EndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);

        try
        {
            m_listenSocket.Bind(remoteEP);

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
            m_listenSocket.Listen(backlog);

            Console.WriteLine("Listen end");
        }
        catch(SocketException ex)
        {
            Console.WriteLine("Listen " + ex);
            throw ex;
        }
    }

    public void AcceptAsync(AcceptCallback acceptCallback, ReceiveCallback receiveCallback)
    {
        Console.WriteLine("AcceptAsync ");

        m_accpetCallback = acceptCallback;
        m_receiveCallback = receiveCallback;

        _AcceptAsync();
    }

    void _AcceptAsync()
    {
        SocketAsyncEventArgs acceptArgs = new SocketAsyncEventArgs();
        acceptArgs.Completed += OnAcceptComplete;
        //acceptArgs.RemoteEndPoint = m_remoteEP;
        m_listenSocket.ConnectAsync(acceptArgs);

        m_listenSocket.AcceptAsync(acceptArgs);
    }

    void OnAcceptComplete(object sender, SocketAsyncEventArgs e)
    {
        Console.WriteLine("OnAcceptComplete " + e.SocketError);
        
        if(e.SocketError == SocketError.Success)
        {
            m_channelIdGenerator++;
            ChannelInfo channelInfo = new ChannelInfo();
            channelInfo.m_channelId = m_channelIdGenerator;
            channelInfo.m_socket = e.AcceptSocket;
            m_channelConcurrentDic.TryAdd(channelInfo.m_channelId, channelInfo);

            m_accpetCallback(m_channelIdGenerator);

            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += OnConcurrentReceiveComplete;
            receiveArgs.UserToken = channelInfo.m_channelId;
            channelInfo.m_socket.ReceiveAsync(receiveArgs);
        }

        _AcceptAsync();
    }

    void OnConcurrentReceiveComplete(object sender, SocketAsyncEventArgs e)
    {
        int channelId = (int)e.UserToken;

        Console.WriteLine("OnConcurrentReceiveComplete " + channelId + " " + e.SocketError);

        if(e.BytesTransferred == 0)
        {
            ChannelInfo channelInfo = null;
            m_channelConcurrentDic.TryRemove(channelId, out channelInfo);
            channelInfo.m_socket.Close();
            
        }
        else if(e.SocketError == SocketError.Success)
        {
            ChannelInfo channelInfo = null;
            m_channelConcurrentDic.TryGetValue(channelId, out channelInfo);

            byte[] receiveBytes = new byte[e.BytesTransferred];
            Buffer.BlockCopy(e.Buffer, 0, receiveBytes, 0, e.BytesTransferred);

            List<Packet> packet = channelInfo.m_decodeHelper.Decode(receiveBytes);
            for (int i = 0; i < packet.Count; ++i )
            {
                Packet iter = packet[i];
                m_receiveCallback(channelId, iter);
            }

            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += OnConcurrentReceiveComplete;
            receiveArgs.UserToken = channelInfo.m_channelId;
            channelInfo.m_socket.ReceiveAsync(receiveArgs);
        }
//         else
//         {
//             Console.WriteLine("OnConcurrentReceiveComplete " + channel);
//         }
    }

    void SendAsync(int channel, Packet packet)
    {
        ChannelInfo channelInfo = null;
        m_channelConcurrentDic.TryGetValue(channel, out channelInfo);

        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
        sendArgs.UserToken = channel;
        sendArgs.Completed += OnConcurrentSendComplete;

        byte[] data = m_encodeHelper.Encode(packet);
        sendArgs.SetBuffer(data, 0, data.Length);

        channelInfo.m_socket.SendAsync(sendArgs);
    }

    void OnConcurrentSendComplete(object sender, SocketAsyncEventArgs e)
    {
        int channelId = (int)e.UserToken;
        Console.WriteLine("OnConcurrentSendComplete " + channelId + " " +e.SocketError);
//         if(e.SocketError != SocketError.Success)
//         {
//             Console.WriteLine("OnConcurrentSendComplete");
//         }
        if(e.BytesTransferred == 0)
        {
            ChannelInfo channelInfo = null;
            //m_channelConcurrentDic.TryGetValue(channelId, out channelInfo);
            m_channelConcurrentDic.TryRemove(channelId, out channelInfo);
            channelInfo.m_socket.Close();
        }
    }
}

