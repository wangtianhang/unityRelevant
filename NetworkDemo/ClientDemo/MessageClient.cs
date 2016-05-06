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

    public MessageClient()
    {
        m_netlayerClient = new NetlayerClient();
        m_netlayerClient.m_disconnectEvent += this.OnDisconnct;
    }

    public void OnUpdate(float delta)
    {
        m_netlayerClient.OnUpdate(delta);

        m_accSeqTimer += delta;
        if(m_accSeqTimer >= m_getSeqTimeSpan)
        {
            m_accSeqTimer = 0;
            SendMessage((int)MessageId.ExchangeSeq, null);
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
            SendMessage((int)MessageId.ExchangeSeq, null);
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

        m_netlayerClient.SendAsync(packet, SendCallback);
    }

    void SendCallback(SocketError error)
    {
        Console.WriteLine("SendCallback " + error);
    }

    public void ReceiveMessage(int msgId, ReceiveMsgCallback callback)
    {

    }

    void OnDisconnct()
    {
        m_netlayerClient.ConnectAsync(m_ip, m_port, ConnectCallback);
    }
}
