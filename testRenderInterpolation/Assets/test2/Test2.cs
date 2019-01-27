using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 逻辑帧逐步走（本地delta累加或者系统计时器for网络）
/// 尝试在某个逻辑帧暂停,
/// 假设有一个10秒的大delta 在中间5秒的逻辑帧执行pause命令
/// 表现层可以持续update
/// </summary>
public class Test2 : MonoBehaviour {

    //public static float m_tickSpan = 0.033f;
    public float m_accTime = 0;
    public Logic1 m_logic = null;
    public Render1 m_render = null;
    static bool m_isPause = false;

    void SetPause(bool flag)
    {
        m_isPause = flag;
    }

    public void Start()
    {
        m_logic = new Logic1();
        m_logic.Init(Vector3.zero, Quaternion.identity);
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "render1";
        m_render = go.AddComponent<Render1>();
        m_render.Init(m_logic);
    }

    public void Update()
    {
        EngineUpdate(Time.deltaTime);
    }

    void EngineUpdate(float delta)
    {
        m_accTime += delta;
        while (m_accTime >= Config.m_tickSpan)
        {
            m_accTime -= Config.m_tickSpan;
            Tick();
        }

        m_render.RenderUpdate(delta);
    }

    void Tick()
    {
        if (!m_isPause)
        {
            m_logic.Tick();
        }
    }
}
