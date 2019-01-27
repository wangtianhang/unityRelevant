using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 尝试在某个逻辑帧暂停
/// </summary>
public class Test2 : MonoBehaviour {

    //public static float m_tickSpan = 0.033f;
    public float m_accTime = 0;
    public Logic1 m_logic = null;
    public Render1 m_render = null;
    public bool m_isPause = false;

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
        if (!m_isPause)
        {
            m_accTime += Time.deltaTime;
            while (m_accTime >= Config.m_tickSpan)
            {
                m_accTime -= Config.m_tickSpan;
                m_logic.Tick();
            }
            m_render.RenderUpdate(Time.deltaTime);
        }
    }
}
