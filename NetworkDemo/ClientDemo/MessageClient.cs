using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class MessageClient
{
    NetlayerClient m_netlayerClient = null;

    public delegate void ReceiveMsgCallback(int msgId, Stream data);
    public MessageClient()
    {

    }

    public void Connect(string ip, int port)
    {

    }

    public void SendMessage(int msgId, Stream data)
    {

    }

    public void ReceiveMessage(int msgId, ReceiveMsgCallback callback)
    {

    }
}
