using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncFrameMgr : MonoBehaviour
{
	public const float m_tickSpan = 0.033f;
	public float m_acc = 0;
	public int m_tickCount = 0;
	List<Entity> m_entityList = new List<Entity>();
	// Use this for initialization
	void Start () 
	{
		Entity entity = new Entity();
		entity.Init();
		m_entityList.Add(entity);
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_acc += Time.deltaTime;
		
		while (m_acc >= SyncFrameMgr.m_tickSpan)
		{
			m_acc -= SyncFrameMgr.m_tickSpan;
			OnTick(SyncFrameMgr.m_tickSpan);
		}

		OnRenderUpdate(Time.deltaTime);
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
		m_tickCount += 1;
		
		foreach (var iter in m_entityList)
		{
			iter.OnTick(delta);
		}
	}
}
