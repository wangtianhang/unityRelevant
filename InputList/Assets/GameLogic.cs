using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class GameLogic : MonoBehaviour
{
    InputEventCacher m_eventCacher = new InputEventCacher();
    float m_accTimer = 0;
    float m_oneFrameSpan = 1 / 30;

    public void Awake()
    {
        GameObject inputGo = new GameObject("inputCacher");
        m_eventCacher = inputGo.AddComponent<InputEventCacher>();
    }

    void Update()
    {
        m_accTimer += Time.deltaTime;
        while (m_accTimer >= m_oneFrameSpan)
        {
            m_accTimer -= m_oneFrameSpan;

            LogicUpdate(m_oneFrameSpan);
        }
    }

    void LogicUpdate(float delta)
    {
        InputEvent evt = m_eventCacher.GetNextInputEvent();
        while(evt != null)
        {
            Debug.Log(evt.m_type);

            evt = m_eventCacher.GetNextInputEvent();
        }

        m_eventCacher.LogicUpdate();
    }
}

