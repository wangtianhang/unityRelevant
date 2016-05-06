using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

public class MessageClient
{
    NetlayerClient m_netlayerClient = null;

    public delegate void ReceiveMsgCallback(int msgId, Stream data);

    public delegate void SendMsgEvent(int msgId);
    public event SendMsgEvent m_sendMsgEvent;

    public delegate void ReceiveMsgEvent(int msgId);
    public event ReceiveMsgEvent m_receiveMsgEvent;

    ConnectCallback m_connectCallback = null;

    public MessageClient()
    {
        m_netlayerClient = new NetlayerClient();
    }

    public void OnUpdate()
    {
        m_netlayerClient.OnUpdate();
    }

    public void Connect(string ip, int port, ConnectCallback callback)
    {
        m_connectCallback = callback;

        m_netlayerClient.ConnectAsync(ip, port, ConnectCallback);
    }

    public void ConnectCallback(SocketError error)
    {
        m_connectCallback(error);
    }

    public void SendMessage(int msgId, Stream data)
    {

    }

    public void ReceiveMessage(int msgId, ReceiveMsgCallback callback)
    {

    }
}
