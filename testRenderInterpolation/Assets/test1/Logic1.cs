using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic1 
{
    public Vector3 m_oldLogicPos = Vector3.zero;
    public Vector3 m_curLogicPos = Vector3.zero;
    public Quaternion m_oldLogicQua = Quaternion.identity;
    public Quaternion m_curLogicQua = Quaternion.identity;

    public float m_logicAccTime = 0;

    public void Init(Vector3 initPos, Quaternion initQua)
    {
        m_oldLogicPos = initPos;
        m_curLogicPos = initPos;
        m_oldLogicQua = initQua;
        m_curLogicQua = initQua;

        m_logicAccTime += Test1.m_tickSpan;
    }

    public void Tick()
    {
        // todo 计算新的pos和qua
        m_logicAccTime += Test1.m_tickSpan;

        float testRotateSpeed = 90;
        Vector3 euler = m_curLogicQua.eulerAngles;
        euler.y += testRotateSpeed * Test1.m_tickSpan;
        //euler.y += Mathf.Sin(testRotateSpeed * Test1.m_tickSpan);
        m_oldLogicQua = m_curLogicQua;
        m_curLogicQua = Quaternion.Euler(euler);
    }
}
