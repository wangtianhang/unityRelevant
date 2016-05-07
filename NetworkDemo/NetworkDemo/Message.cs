using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public enum MessageId
{
    None,
    ExchangeSeq,
    ExchangeSeqS2C,
    Login,
    LoginS2C,
}

/// <summary>
/// 实际项目中可以跟着心跳交互
/// 服务器不缓存消息队列，重新连接时重新拉取数据
/// </summary>
public class MessageExchangeSeq
{
    //public Int32 m_seq = 0;

    public static MemoryStream Serilization(MessageExchangeSeq msg)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
        //writer.Write(msg.m_seq);
        return memoryStream;
    }

    public static MessageExchangeSeq Deserilization(MemoryStream stream)
    {
//         MemoryStream memoryStram = stream as MemoryStream;
//         if(memoryStram == null)
//         {
//             throw new InvalidOperationException();
//         }

        MessageExchangeSeq msg = new MessageExchangeSeq();
        BinaryReader reader = new BinaryReader(stream);
        //msg.m_seq = reader.ReadInt32();
        return msg;
    }
}

/// <summary>
/// 实际项目中可以跟着心跳交互
/// 客户端收到seq后才清理自己的消息队列,清理队列中seq小于等于服务器seq的消息
/// 如果收到0，说明服务器刚初始化与客户端的会话或者服务器与客户端的会话已经严重超时，清空消息队列
/// </summary>
public class MessageExchangeSeqS2C
{
    public Int32 m_seq = 0;

    public static MemoryStream Serilization(MessageExchangeSeqS2C msg)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
        writer.Write(msg.m_seq);
        return memoryStream;
    }

    public static MessageExchangeSeqS2C Deserilization(MemoryStream stream)
    {
//         MemoryStream memoryStram = stream as MemoryStream;
//         if (memoryStram == null)
//         {
//             throw new InvalidOperationException();
//         }

        MessageExchangeSeqS2C msg = new MessageExchangeSeqS2C();
        BinaryReader reader = new BinaryReader(stream);
        msg.m_seq = reader.ReadInt32();
        return msg;
    }
}

public class MessageLogin
{
    public Int32 m_clientId = 0;

    public static MemoryStream Serilization(MessageLogin msg)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
        writer.Write(msg.m_clientId);
        return memoryStream;
    }

    public static MessageLogin Deserilization(MemoryStream stream)
    {
//         MemoryStream memoryStram = stream as MemoryStream;
//         if (memoryStram == null)
//         {
//             throw new InvalidOperationException();
//         }

        MessageLogin msg = new MessageLogin();
        BinaryReader reader = new BinaryReader(stream);
        msg.m_clientId = reader.ReadInt32();
        return msg;
    }
}

public class MessageLoginS2C
{
    public string m_content = "";

    public static MemoryStream Serilization(MessageLoginS2C msg)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
        writer.Write(msg.m_content);
        return memoryStream;
    }

    public static MessageLoginS2C Deserilization(MemoryStream stream)
    {
//         MemoryStream memoryStram = stream as MemoryStream;
//         if (memoryStram == null)
//         {
//             throw new InvalidOperationException();
//         }

        MessageLoginS2C msg = new MessageLoginS2C();
        BinaryReader reader = new BinaryReader(stream);
        msg.m_content = reader.ReadString();
        return msg;
    }
}


