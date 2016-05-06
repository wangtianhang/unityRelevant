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

}

public class Packet
{
    public int m_msgId;
    public int m_seq;
    public byte[] m_data;
}

