using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Server
{
    MessageServer m_messageServer = null;

    MultiThreadMgr m_multiThradMgr = null;

    public Server()
    {
        m_messageServer = new MessageServer();
    }

    public void Init(string ip, int port)
    {

    }

    void ClientConnectCallback(int clientId)
    {

    }
}

