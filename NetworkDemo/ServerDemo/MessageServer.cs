using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Concurrent;

class MessageServer
{

    public delegate void ClientConnectCallback(int clientId);
    ClientConnectCallback m_clientConnectCallback = null;

    public delegate void ClientDisconnectCallback(int clientId);
    ClientDisconnectCallback m_clientDisconnectCallback = null;

    public delegate void ClientMessageCallback(int clientId, int msgId, MemoryStream stream);
    ClientMessageCallback m_clientMessageCallback = null;

    NetlayerServer m_netlayerServer = null;

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
    //List<ClientChannelInfo> m_timerList = new List<ClientChannelInfo>();

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

    public void AcceptBegin(ClientConnectCallback connectCallback, 
        ClientDisconnectCallback disconnectCallback, ClientMessageCallback msgCallback)
    {
        m_clientConnectCallback = connectCallback;
        m_clientDisconnectCallback = disconnectCallback;
        m_clientMessageCallback = msgCallback;

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
        clientInfo.m_channelId = 0;

        m_clientConnectCallback(clientInfo.m_clientId);
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
            MemoryStream stream = new MemoryStream(packet.m_data);
            MemoryStream cloneStream = Util.DeepClone<MemoryStream>(stream);
            MessageLogin login = MessageLogin.Deserilization(cloneStream);

            if(m_clientDic.ContainsKey(login.m_clientId))
            {
                // 还未清除会话
                ClientChannelInfo clientChannelInfo = m_clientDic[login.m_clientId];
                clientChannelInfo.m_channelId = channelId;
                
                clientChannelInfo.m_isConnected = true;
                clientChannelInfo.m_closeAccTimer = 0;

                m_channelDic.Add(channelId, clientChannelInfo);

            }
            else
            {
                ClientChannelInfo clientChannelInfo = new ClientChannelInfo();

                clientChannelInfo.m_clientId = login.m_clientId;
                clientChannelInfo.m_channelId = channelId;
                clientChannelInfo.m_isConnected = true;

                m_clientDic.Add(login.m_clientId, clientChannelInfo);

                m_channelDic.Add(channelId, clientChannelInfo);
            }

            m_clientConnectCallback(login.m_clientId);
            m_clientMessageCallback(login.m_clientId, packet.m_msgId, stream);
        }
        else if (packet.m_msgId == (int)MessageId.ExchangeSeq)
        {
            ClientChannelInfo clientInfo = null;
            m_channelDic.TryGetValue(channelId, out clientInfo);

            MessageExchangeSeqS2C s2c = new MessageExchangeSeqS2C();
            s2c.m_seq = clientInfo.m_lastSeq;
            MemoryStream stream = MessageExchangeSeqS2C.Serilization(s2c);

            SendAsync(clientInfo.m_clientId, (int)MessageId.ExchangeSeqS2C, stream);
        }
        else
        {
            ClientChannelInfo clientInfo = null;
            m_channelDic.TryGetValue(channelId, out clientInfo);

            clientInfo.m_lastSeq = packet.m_seq;

            MemoryStream stream = new MemoryStream(packet.m_data);
            m_clientMessageCallback(clientInfo.m_clientId, packet.m_msgId, stream);
        }
    }

    public void OnUpdate(int delta)
    {
        m_netlayerServer.OnUpdate(delta);

        SendData sendData = null;
        m_sendDataQueue.TryDequeue(out sendData);
        while(sendData != null)
        {
            m_netlayerServer.SendAsync(sendData.m_clientId, sendData.m_packet);
            m_sendDataQueue.TryDequeue(out sendData);
        }

        List<int> needRemoveList = new List<int>();
        foreach(var iter in m_clientDic)
        {
            ClientChannelInfo clientInfo = iter.Value;
            if (!clientInfo.m_isConnected)
            {
                clientInfo.m_closeAccTimer += delta;
                if (clientInfo.m_closeAccTimer >= m_closeTimeSpan)
                {
                    needRemoveList.Add(clientInfo.m_clientId);
                }
            }
        }
        foreach(var iter in needRemoveList)
        {
            m_clientDic.Remove(iter);
        }
        needRemoveList.Clear();
    }
}

