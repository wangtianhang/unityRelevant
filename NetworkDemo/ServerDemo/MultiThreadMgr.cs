using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

class MultiThreadMgr
{
    public delegate void SendMsg(int clientId, int msdId, MemoryStream stream);


    class WorkData
    {
        public int m_clientId = 0;
        public int m_msgId = 0;
        public MemoryStream m_data = null;
        public SendMsg m_sendMessage = null;
    }

    static void ThreadOperation(object param)
    {
        WorkData workData = param as WorkData;
        Console.WriteLine("ThreadOperation " + workData.m_clientId + " " + workData.m_msgId);
        switch(workData.m_msgId)
        {

        }
    }

    public void ProcessWork(int clientId, int msgID, MemoryStream data, SendMsg sendMessage)
    {
        WorkData workData = new WorkData();
        workData.m_clientId = clientId;
        workData.m_msgId = msgID;
        workData.m_data = data;
        workData.m_sendMessage = sendMessage;
        ThreadPool.QueueUserWorkItem(ThreadOperation, workData);
    }
}

