
using UnityEngine;

public class Entity
{
    public Vector3 m_lastFrameLogicPos;
    public Quaternion m_lastFrameLogicQua;
    private Transform m_logicTransform = null;
    public float m_logicAcc = 0;
    public float m_renderAcc = 0;
    private Transform m_renderGo = null;

    public float m_rotateSpeed = 0.3f;
    
    public void Init()
    {
        m_logicTransform = new GameObject("logicGo").transform;
        m_renderGo = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        m_renderGo.name = "renderGo";
    }
    
    public void OnTick(float delta)
    {
        m_logicAcc += delta;
        m_lastFrameLogicPos = m_logicTransform.position;
        m_lastFrameLogicQua = m_logicTransform.rotation;
        
        m_logicTransform.transform.position = new Vector3(5 * Mathf.Sin(m_logicAcc * m_rotateSpeed), 0, 5 * Mathf.Cos(m_logicAcc * m_rotateSpeed));
    }

    public void OnRenderUpdate(float delta)
    {
        m_renderAcc += delta;
        if (m_renderAcc > m_logicAcc)
        {
            Debug.LogWarning("render over logic renderAcc " + m_renderAcc + " m_logicAcc " + m_logicAcc);
        }
        m_renderAcc = Mathf.Clamp(m_renderAcc, m_logicAcc - SyncFrameMgr.m_tickSpan, m_logicAcc);
        float curOverTime = m_renderAcc - (m_logicAcc - SyncFrameMgr.m_tickSpan);
        float weight = curOverTime / SyncFrameMgr.m_tickSpan;
        
        Vector3 newRenderPos = Vector3.Lerp(m_lastFrameLogicPos, m_logicTransform.position, weight);
        Debug.DrawLine(m_renderGo.position, newRenderPos, Color.green, 3600);
        m_renderGo.position = newRenderPos;
        m_renderGo.rotation = Quaternion.Slerp(m_lastFrameLogicQua, m_logicTransform.rotation, weight);
    }
}
