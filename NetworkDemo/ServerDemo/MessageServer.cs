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

    //ConcurrentDictionary<int, ClientChannelInfo> m_channelConcurrentDic = new ConcurrentDictionary<int, ClientChannelInfo>();
    //ConcurrentDictionary<int, ClientChannelInfo> m_clientConcurrentDic = new ConcurrentDictionary<int, ClientChannelInfo>();
    Dictionary<int, ClientChannelInfo> m_channelDic = new Dictionary<int, ClientChannelInfo>();
    Dictionary<int, ClientChannelInfo> m_clientDic = new Dictionary<int, ClientChannelInfo>();

    class SendData
    {
        public int m_clientId = 0;
        //public int m_msgId = 0;
        //public MemoryStream m_stream = null;
        public Packet m_packet = null;
    }

    ConcurrentQueue<SendData> m_sendDataQueue = new ConcurrentQueue<SendData>();

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

        m_netlayerServer.AcceptAsync(AcceptCallback, CloseCallback, ReceiveCallback);
    }

    void AcceptCallback(int channelId)
    {

    }

    void CloseCallback(int channelId)
    {
        //ClientChannelInfo clientInfo = null;
        //m_channelConcurrentDic.TryRemove(channelId, out clientInfo);
        //clientInfo.m_isConnected = false;
        ClientChannelInfo clientInfo = null;
        m_channelDic.TryGetValue(channelId, out clientInfo);
        m_channelDic.Remove(channelId);
        clientInfo.m_isConnected = false;
    }

    public void SendAsync(int clientId, int msgId, MemoryStream data)
    {
        //ClientChannelInfo channelInfo = null;
        //m_clientConcurrentDic.TryGetValue(clientId, out channelInfo);

        Packet packet = new Packet();
        packet.m_msgId = msgId;
        packet.m_data = data.ToArray();

        SendData sendData = new SendData();
        sendData.m_clientId = clientId;
        sendData.m_packet = packet;
        m_sendDataQueue.Enqueue(sendData);

        //m_netlayerServer.SendAsync(channelInfo.m_channelId, packet);
    }

    void ReceiveCallback(int channelId, Packet packet)
    {
        if(packet.m_msgId == (int)MessageId.Login)
        {

        }
        else if (packet.m_msgId == (int)MessageId.ExchangeSeq)
        {

        }
        else
        {

        }
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

