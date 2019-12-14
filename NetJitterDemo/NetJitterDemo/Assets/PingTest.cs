using System;
using UnityEngine;
using System.Net.NetworkInformation;
using Ping = UnityEngine.Ping;

public class PingTest : MonoBehaviour
{
    public void Start()
    {
        //IPAddress address = new IPAddress("www.baidu.com");
        //System.Net.NetworkInformation.Ping ping2 = new System.Net.NetworkInformation.Ping();
        //Debug.Log(ping2.Send("39.156.69.79").Status);
        //Debug.Log(ping2.Send("www.baidu.com").Status);
        
        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        ping.PingCompleted += Callback;
        ping.SendAsync("39.156.69.79", null);
        
    }

    void Callback(object sender, PingCompletedEventArgs e)
    {
        Debug.Log(e.Reply.Status);
        System.Net.NetworkInformation.Ping ping = sender as System.Net.NetworkInformation.Ping;
        ping.Dispose();
    }
}
