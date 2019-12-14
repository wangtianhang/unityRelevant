using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class SyncFrameMgr : MonoBehaviour
{
	public enum NetMode
	{
		None,
		Local,
		FakeNet,
		//PingNetWork,
	}
	
	public const float m_tickSpan = 0.033f;
	public float m_acc = 0;
	public int m_ClientTickCount = 0;
	public int m_ServerFrameCount = 0;
	List<Entity> m_entityList = new List<Entity>();
	private NetMode m_netMode = NetMode.FakeNet;
	int m_nDriftFactor = 4;
	
	double m_CurPkgDelay;
	double m_AvgFrameDelay;
	
	//=========伪造网络抖动=========
	public const int m_fakeSpanTime = 60;
	public double m_fakeAccTime = m_fakeSpanTime;
	public List<float> m_fakeTimeList = new List<float>();
	//public int m_lastFakeFrameTime = 0;
	//==================
	
	//==========利用ping百度开始==============
	public float m_pingAcc = 0;
	//private System.Net.NetworkInformation.Ping m_ping;
	//==========利用ping百度结束==============
	
	// Use this for initialization
	void Start () 
	{
		Entity entity = new Entity();
		entity.Init();
		m_entityList.Add(entity);

		//m_ping = new System.Net.NetworkInformation.Ping();
		//m_ping.PingCompleted += OnPingReceiveServerCommand;
	}

	// Update is called once per frame
	void Update ()
	{
		float delta = Time.deltaTime;
		
		if (m_netMode == NetMode.Local)
		{
			m_acc += delta;
		
			while (m_acc >= SyncFrameMgr.m_tickSpan)
			{
				m_acc -= SyncFrameMgr.m_tickSpan;
				OnTick(SyncFrameMgr.m_tickSpan);
			}
		}
		else if (m_netMode == NetMode.FakeNet)
		{
			//============伪造抖动网络 begin=======================
			FakeJitterNetwork(delta);
			//============伪造抖动网络 end=======================	
			HandleNetworkRelay(delta);
		}
//		else if (m_netMode == NetMode.PingNetWork)
//		{
//			SendPing(delta);
//			HandleNetworkRelay(delta);
//		}

		OnRenderUpdate(Time.deltaTime);
	}

	void SendPing(float delta)
	{
		m_pingAcc += delta;
		if (m_pingAcc >= SyncFrameMgr.m_tickSpan)
		{
			m_pingAcc -= SyncFrameMgr.m_tickSpan;
			//m_ping.SendAsync("39.156.69.79", null);
		}
	}
	
	void OnPingReceiveServerCommand(object sender, PingCompletedEventArgs e)
	{
		m_ServerFrameCount += 1;
		
		Debug.Log(e.Reply.Status + " m_ServerFrameCount " + m_ServerFrameCount);
		//System.Net.NetworkInformation.Ping ping = sender as System.Net.NetworkInformation.Ping;
		//ping.Dispose();
	}

	void HandleNetworkRelay(float delta)
	{
		{
			m_acc += delta;
			int tryCount = (m_ServerFrameCount - m_ClientTickCount) / m_nDriftFactor;
			tryCount = Mathf.Clamp(tryCount, 1, 100);
			double delayS = m_acc - m_ServerFrameCount * SyncFrameMgr.m_tickSpan;
			double avgDelayS = CalculateJitterDelay(delayS);
			while (tryCount > 0)
			{
				double clientTime = m_ClientTickCount * SyncFrameMgr.m_tickSpan;
				double canUseTime = m_acc - (clientTime + avgDelayS);
				if (canUseTime >= SyncFrameMgr.m_tickSpan)
				{
					if(m_ClientTickCount >= m_ServerFrameCount)
					{
						tryCount = 0;
					}
					else
					{
						OnTick(SyncFrameMgr.m_tickSpan);
						tryCount -= 1;
					}
				}
				else
				{
					tryCount = 0;
				}
			}
		}
	}

	void FakeJitterNetwork(float delta)
	{
		m_fakeAccTime += delta;
		if (m_fakeAccTime >= m_fakeSpanTime
		    && m_fakeTimeList.Count == 0)
		{
			m_fakeAccTime -= m_fakeSpanTime;

			int count = (int)(m_fakeSpanTime / SyncFrameMgr.m_tickSpan);
			Debug.Log("total fake count = " + count);
			for (int i = 0; i < count; ++i)
			{
				float random = m_acc + Random.Range(0f, m_fakeSpanTime);
				m_fakeTimeList.Add(random);
			}
			m_fakeTimeList.Sort();
//				string text = "";
//				foreach(var iter in m_fakeTimeList)
//				{
//					text += iter.ToString() + "\n";
//				}
//				Debug.Log("===============================" + text);
		}
			
		while (m_fakeTimeList.Count != 0)
		{
			if (m_fakeAccTime >= m_fakeTimeList[0])
			{
				//SingletonMgr.GetRealTimeChart().AddSample("NetPkgFakeTime", (m_fakeTimeList[0] * 1000) - m_lastFakeFrameTime);
				//m_lastFakeFrameTime = (int)(m_fakeTimeList[0] * 1000);
				m_fakeTimeList.RemoveAt(0);

				//BattleCommandBase collection = BattleCommandBase.CreateCommand(BattleCommandType.ServerCollection);
				//collection.m_commandList = GetMatchFakeCommand(m_battleModule.GetTimeHelper()._GetBattleStartTime());

				//m_battleModule.GetFrameSyncMgr().AddServerCollection(collection);
				OnFakeReceiveServerCommand();
			}
			else
			{
				break;
			}
		}
	}
	
	public double CalculateJitterDelay(double delayS)
	{
		m_CurPkgDelay = ((delayS <= 0) ? 0 : (delayS));
		if (m_AvgFrameDelay < 0)
		{
			m_AvgFrameDelay = m_CurPkgDelay;
		}
		else
		{
			m_AvgFrameDelay = (29 * m_AvgFrameDelay + m_CurPkgDelay) / 30;
		}
		return m_AvgFrameDelay;
	}

	void OnFakeReceiveServerCommand()
	{
		m_ServerFrameCount += 1;
	}

	void OnRenderUpdate(float delta)
	{
		foreach (var iter in m_entityList)
		{
			iter.OnRenderUpdate(delta);
		}
	}

	void OnTick(float delta)
	{
		m_ClientTickCount += 1;
		
		foreach (var iter in m_entityList)
		{
			iter.OnTick(delta);
		}
	}
}
