using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
        m_netlayerServer.AcceptAsync(AcceptCallback);
    }

    void AcceptCallback(int channelId)
    {

    }

    void ReceiveAsync()
    {

    }
}

