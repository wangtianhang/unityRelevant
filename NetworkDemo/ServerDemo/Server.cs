using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



class Server
{
    MessageServer m_messageServer = null;

    MultiThreadMgr m_multiThradMgr = null;

    class ClientData
    {
        public int m_clientId = 0;
    }

    Dictionary<int, ClientData> m_clientDic = new Dictionary<int, ClientData>();

    public Server()
    {
        m_messageServer = new MessageServer();

        m_multiThradMgr = new MultiThreadMgr();
    }

    public void Init(string ip, int port)
    {
        m_messageServer.Bind(ip, port);
        m_messageServer.Listen();
        m_messageServer.AcceptBegin(ClientConnectCallback, ClientDisconnectCallback, OnClientMessage);
    }

    public void OnUpdate(int delta)
    {
        m_messageServer.OnUpdate(delta);
    }

    void ClientConnectCallback(int clientId)
    {
        ClientData clientData = new ClientData();
        clientData.m_clientId = clientId;
        m_clientDic.Add(clientId, clientData);
    }

    void ClientDisconnectCallback(int clientId)
    {
        m_clientDic.Remove(clientId);
    }

    void OnClientMessage(int client, int msgId, MemoryStream data)
    {
        m_multiThradMgr.ProcessWork(client, msgId, data, m_messageServer.SendAsync);
    }
}

