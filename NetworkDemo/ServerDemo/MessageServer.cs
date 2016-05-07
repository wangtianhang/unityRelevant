using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

class MessageServer
{
    NetlayerServer m_netlayerServer = null;

//     class ClientChannelInfo
//     {
//         public int m_lastSeq = 0;
//     }
// 
//     Dictionary<int, ClientChannelInfo> m_channelInfoDic = new Dictionary<int, ClientChannelInfo>();

    public MessageServer()
    {
        m_netlayerServer = new NetlayerServer();
    }

    public void Bind(string ip, int port)
    {
        m_netlayerServer.Bind(ip, port);
    }

    public void Listen(int backlog)
    {
        m_netlayerServer.Listen(backlog);
    }

    public void AcceptBegin()
    {
        m_netlayerServer.AcceptAsync(AcceptCallback, ConcurrentReceiveCallback);
    }

    void AcceptCallback(int channelId)
    {

    }

    void SendAsync(int clientId, int msgId, MemoryStream data)
    {

    }

    void ConcurrentReceiveCallback(int channelId, Packet packet)
    {

    }
}

