using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

public class MessageClient
{
    NetlayerClient m_netlayerClient = null;

    public delegate void ReceiveMsgCallback(int msgId, MemoryStream data);

    public delegate void SendMsgEvent(int msgId);
    public event SendMsgEvent m_sendMsgEvent;

    public delegate void ReceiveMsgEvent(int msgId);
    public event ReceiveMsgEvent m_receiveMsgEvent;

    ConnectCallback m_connectCallback = null;

    string m_ip = "";
    int m_port = 0;
    int m_seq = 0;

    float m_accSeqTimer = 0;
    float m_getSeqTimeSpan = 15000;

    Dictionary<int, ReceiveMsgCallback> m_receiveCallbackDic = new Dictionary<int, ReceiveMsgCallback>();

    List<Packet> m_cachePacketList = new List<Packet>();

    public MessageClient()
    {
        m_netlayerClient = new NetlayerClient();
        m_netlayerClient.m_disconnectEvent += this.OnDisconnct;
    }

    public void OnUpdate(int delta)
    {
        m_netlayerClient.OnUpdate(delta);

        m_accSeqTimer += delta;
        if(m_accSeqTimer >= m_getSeqTimeSpan)
        {
            m_accSeqTimer = 0;
            if(m_netlayerClient.IsConnected())
            {
                SendMessage((int)MessageId.ExchangeSeq, null);
            }
        }
    }

    public void Connect(string ip, int port, ConnectCallback callback)
    {
        m_ip = ip;
        m_port = port;

        m_connectCallback = callback;

        m_netlayerClient.ConnectAsync(ip, port, ConnectCallback);
    }

    public void ConnectCallback(SocketError error)
    {
        if (error != SocketError.Success)
        {
            m_connectCallback(error);
        }
        else
        {
            m_netlayerClient.ReceiveAsync(this.OnReceiveMessage);
            RegisterReceiveMessage((int)MessageId.ExchangeSeqS2C, ServerSeqCallback);
            //SendMessage((int)MessageId.ExchangeSeq, null);
            RegisterReceiveMessage((int)MessageId.LoginS2C, LoginCallback);
        }
    }

    public void SendMessage(int msgId, MemoryStream data)
    {
        m_sendMsgEvent(msgId);

        Packet packet = new Packet();
        packet.m_msgId = msgId;
        m_seq++;
        packet.m_seq = m_seq;
        packet.m_data = data.ToArray();

        m_cachePacketList.Add(packet);

        m_netlayerClient.SendAsync(packet, SendCallback);
    }

    void SendCallback(SocketError error)
    {
        Console.WriteLine("SendCallback " + error);
    }

    public void RegisterReceiveMessage(int msgId, ReceiveMsgCallback callback)
    {
        //ReceiveMsgCallback callback = null;
        //m_receiveCallbackDic.TryGetValue(msgId, out callback);
        if(!m_receiveCallbackDic.ContainsKey(msgId))
        {
            m_receiveCallbackDic.Add(msgId, callback);
        }
    }

    void OnReceiveMessage(SocketError error, int msgId, MemoryStream data)
    {
        if(error != SocketError.Success)
        {
            Console.WriteLine("OnReceiveMessage " + error);
        }
        else
        {
            ReceiveMsgCallback callback = null;
            m_receiveCallbackDic.TryGetValue(msgId, out callback);
            callback(msgId, data);

            m_receiveMsgEvent(msgId);
        }
    }

    void ServerSeqCallback(int msgId, MemoryStream data)
    {
        MessageExchangeSeqS2C s2c = MessageExchangeSeqS2C.Deserilization(data);
        int seq = s2c.m_seq;
        if(seq == 0)
        {
            m_cachePacketList.Clear();
        }
        else
        {
            List<Packet> removeList = new List<Packet>();
            for (int i = 0; i < m_cachePacketList.Count; ++i )
            {
                Packet iter = m_cachePacketList[i];
                if(iter.m_seq <= seq)
                {
                    removeList.Add(iter);
                }
            }
            for (int i = 0; i < removeList.Count; ++i )
            {
                m_cachePacketList.Remove(removeList[i]);
            }
        }
    }

    void LoginCallback(int msgId, MemoryStream data)
    {
        SendMessage((int)MessageId.ExchangeSeq, null);
    }

    void OnDisconnct()
    {
        m_netlayerClient.ConnectAsync(m_ip, m_port, ConnectCallback);
    }
}
