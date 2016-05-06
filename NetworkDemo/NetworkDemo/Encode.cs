using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

public class EncodeHelper
{
    public byte[] Encode(Packet packet)
    {
        int header = packet.m_msgId;
        int seq = packet.m_seq;
        int length = packet.m_data == null ? 0 : packet.m_data.Length;
        byte[] bytes = new byte[length + 12];

        if (BitConverter.IsLittleEndian)
        {
            header = IPAddress.HostToNetworkOrder(header);
            seq = IPAddress.HostToNetworkOrder(seq);
            length = IPAddress.HostToNetworkOrder(length);
        }
        byte[] msgIdBytes = BitConverter.GetBytes(header);
        byte[] seqBytes = BitConverter.GetBytes(seq);
        byte[] netDataSizeBytes = BitConverter.GetBytes(length);

        Buffer.BlockCopy(msgIdBytes, 0, bytes, 0, msgIdBytes.Length);
        Buffer.BlockCopy(msgIdBytes, 0, bytes, 4, seqBytes.Length);
        Buffer.BlockCopy(netDataSizeBytes, 0, bytes, 8, netDataSizeBytes.Length);
        if (length != 0)
            Buffer.BlockCopy(packet.m_data, 0, bytes, 12, packet.m_data.Length);
        return bytes;
    }
}

public class DecodeHelper
{
    List<byte>  m_lastBytes = new List<byte>();

    public List<Packet> Decode(byte[] data)
    {
        m_lastBytes.AddRange(data);
        
        List<Packet> packets = new List<Packet>();

        while (m_lastBytes.Count > 12)
        {
            int offset = 0;
            byte[] executeBytes = m_lastBytes.ToArray();
            int msgId = BitConverter.ToInt32(executeBytes, offset);
            offset += 4;
            int seq = BitConverter.ToInt32(executeBytes, offset);
            offset += 4;
            int dataSize = BitConverter.ToInt32(executeBytes, offset);
            offset += 4;

            if (BitConverter.IsLittleEndian)
            {
                msgId = IPAddress.NetworkToHostOrder(msgId);
                seq = IPAddress.NetworkToHostOrder(seq);
                dataSize = IPAddress.NetworkToHostOrder(dataSize);
            }

//             if (data_size < 0)
//             {
//                 return null;
//             }

            if (executeBytes.Length >= (dataSize + 12))
            {
                byte[] msgBytes = new byte[dataSize];
                Buffer.BlockCopy(executeBytes, offset, msgBytes, 0, dataSize);
                Packet packet = new Packet();
                packet.m_msgId = msgId;
                packet.m_seq = seq;
                packet.m_data = msgBytes;
                packets.Add(packet);
                offset += dataSize;
                m_lastBytes.RemoveRange(0, offset);
            }
            else
            {
                break;
            }
        }
        return packets;
    }
}

public class Packet
{
    public int m_msgId;
    public int m_seq;
    public byte[] m_data;
}

