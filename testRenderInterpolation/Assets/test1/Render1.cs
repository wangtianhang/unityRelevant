using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Render1 : MonoBehaviour
{
    public Logic1 m_logic = null;

    public float m_renderAccTime = 0;

    public void Init(Logic1 logic)
    {
        m_logic = logic;
    }

    public void RenderUpdate(float delta)
    {
        m_renderAccTime += delta;

        if (m_logic.m_logicAccTime <= m_renderAccTime)
        {
            Debug.LogError("表现层超过逻辑层");
        }

        float curOverTime = m_renderAccTime - (m_logic.m_logicAccTime - Config.m_tickSpan);
        float weight = curOverTime / Config.m_tickSpan;

        gameObject.transform.rotation = Quaternion.Slerp(m_logic.m_oldLogicQua, m_logic.m_curLogicQua, weight);
    }
}

