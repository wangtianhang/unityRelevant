﻿using System;
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

public class MessageExchangeSeq
{
    public Int32 m_seq = 0;

    public static Stream Serilization(MessageExchangeSeq msg)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
        writer.Write(msg.m_seq);
        return memoryStream;
    }

    public static MessageExchangeSeq Deserilization(Stream stream)
    {
        MemoryStream memoryStram = stream as MemoryStream;
        if(memoryStram == null)
        {
            throw new InvalidOperationException();
        }

        MessageExchangeSeq msg = new MessageExchangeSeq();
        BinaryReader reader = new BinaryReader(stream);
        msg.m_seq = reader.ReadInt32();
        return msg;
    }
}

public class MessageExchangeSeqS2C
{
    public Int32 m_seq = 0;

    public static Stream Serilization(MessageExchangeSeqS2C msg)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
        writer.Write(msg.m_seq);
        return memoryStream;
    }

    public static MessageExchangeSeqS2C Deserilization(Stream stream)
    {
        MemoryStream memoryStram = stream as MemoryStream;
        if (memoryStram == null)
        {
            throw new InvalidOperationException();
        }

        MessageExchangeSeqS2C msg = new MessageExchangeSeqS2C();
        BinaryReader reader = new BinaryReader(stream);
        msg.m_seq = reader.ReadInt32();
        return msg;
    }
}

public class MessageLogin
{
    public string m_content = "";

    public static Stream Serilization(MessageLogin msg)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
        writer.Write(msg.m_content);
        return memoryStream;
    }

    public static MessageLogin Deserilization(Stream stream)
    {
        MemoryStream memoryStram = stream as MemoryStream;
        if (memoryStram == null)
        {
            throw new InvalidOperationException();
        }

        MessageLogin msg = new MessageLogin();
        BinaryReader reader = new BinaryReader(stream);
        msg.m_content = reader.ReadString();
        return msg;
    }
}

public class MessageLoginS2C
{
    public string m_content = "";

    public static Stream Serilization(MessageLoginS2C msg)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
        writer.Write(msg.m_content);
        return memoryStream;
    }

    public static MessageLoginS2C Deserilization(Stream stream)
    {
        MemoryStream memoryStram = stream as MemoryStream;
        if (memoryStram == null)
        {
            throw new InvalidOperationException();
        }

        MessageLoginS2C msg = new MessageLoginS2C();
        BinaryReader reader = new BinaryReader(stream);
        msg.m_content = reader.ReadString();
        return msg;
    }
}
