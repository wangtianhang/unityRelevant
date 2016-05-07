using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class UIListenr
{
    public void SendingCallback(int msgId)
    {
        Console.WriteLine("满足过滤条件，开始阻止ui操作 " + (MessageId)msgId);
    }

    public void ReceiveCallback(int msgId)
    {
        Console.WriteLine("满足过滤条件，解除阻止ui操作" + (MessageId)msgId);
    }
}

