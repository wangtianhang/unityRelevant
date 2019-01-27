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

        //         if (m_renderAccTime >= m_logic.m_logicAccTime)
        //         {
        //             //Debug.LogError("表现层超过逻辑层 进行矫正 可能造成抖动");
        //             m_renderAccTime = m_logic.m_logicAccTime;
        //         }
        
        // 有暂停的系统 表现计时器应该大于逻辑计时器 做下截断保护 在边界下会抖动一下 但是大部分情况下应该插值稳定
        m_renderAccTime = Mathf.Clamp(m_renderAccTime, m_logic.m_logicAccTime - Config.m_tickSpan, m_logic.m_logicAccTime);

        float curOverTime = m_renderAccTime - (m_logic.m_logicAccTime - Config.m_tickSpan);
        float weight = curOverTime / Config.m_tickSpan;

        gameObject.transform.rotation = Quaternion.Slerp(m_logic.m_oldLogicQua, m_logic.m_curLogicQua, weight);
    }
}

