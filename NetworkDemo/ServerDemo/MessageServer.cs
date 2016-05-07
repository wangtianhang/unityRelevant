using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Concurrent;

class MessageServer
{
    NetlayerServer m_netlayerServer = null;

    public delegate void ClientConnectCallback(int clientId);
    ClientConnectCallback m_clientConnectCallback = null;

    class ClientChannelInfo
    {
        public bool m_isConnected = false;
        public int m_closeAccTimer = 0;

        public int m_clientId = 0;
        public int m_channelId = 0;
        public int m_lastSeq = 0;
    }

    int m_closeTimeSpan = 1800 * 1000;

    ConcurrentDictionary<int, ClientChannelInfo> m_channelConcurrentDic = new ConcurrentDictionary<int, ClientChannelInfo>();
    ConcurrentDictionary<int, ClientChannelInfo> m_clientConcurrentDic = new ConcurrentDictionary<int, ClientChannelInfo>();

    ConcurrentQueue<int> m_closeChannelConcurrentQueue = new ConcurrentQueue<int>();

    public MessageServer()
    {
        m_netlayerServer = new NetlayerServer();
    }

    public void Bind(string ip, int port)
    {
        m_netlayerServer.Bind(ip, port);
    }

    public void Listen()
    {
        m_netlayerServer.Listen();
    }

    public void AcceptBegin(ClientConnectCallback callback)
    {
        m_clientConnectCallback = callback;

        m_netlayerServer.AcceptAsync(AcceptCallback, ConcurrentCloseCallback, ConcurrentReceiveCallback);
    }

    void AcceptCallback(int channelId)
    {

    }

    void ConcurrentCloseCallback(int channelId)
    {
        //ClientChannelInfo clientInfo = null;
        //m_channelConcurrentDic.TryRemove(channelId, out clientInfo);
        //clientInfo.m_isConnected = false;
        m_closeChannelConcurrentQueue.Enqueue(channelId);
    }

    public void SendAsync(int clientId, int msgId, MemoryStream data)
    {
        ClientChannelInfo channelInfo = null;
        m_clientConcurrentDic.TryGetValue(clientId, out channelInfo);

        Packet packet = new Packet();
        packet.m_msgId = msgId;
        packet.m_data = data.ToArray();

        m_netlayerServer.SendAsync(channelInfo.m_channelId, packet);
    }

    void ConcurrentReceiveCallback(int channelId, Packet packet)
    {
        
    }

    public void OnUpdate(int delta)
    {
        m_netlayerServer.OnUpdate(delta);
//         foreach(var iter in m_clientConcurrentDic)
//         {
//             if()
//             {
// 
//             }
//         }
    }
}

