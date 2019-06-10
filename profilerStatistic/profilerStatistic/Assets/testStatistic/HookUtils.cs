using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class HookUtils
{
    public struct StackData
    {
        public string m_tag;
        public float m_span;
        public long m_alloc;

        public float m_childUseTime;
        public long m_childAlloc;
    }

    public struct StatisticData
    {
        public string m_tag;
        public float m_totalSpan;
        public long m_totalAlloc;
    }

    public static List<StackData> m_stack = new List<StackData>();
    public static Dictionary<string, StatisticData> m_statisticDic = new Dictionary<string, StatisticData>();
    public static bool m_init = true;

    public static void Init()
    {
        m_stack.Clear();
        m_statisticDic.Clear();
        m_init = true;
    }

    public static void Begin(string tag)
    {
        if (m_init)
        {
            m_init = false;
            Debug.Log("HookUtils开始工作");
        }

        if(tag == "SingletonMgr.Update")
        {
            // 打印日志 避免多次hook的情况
            //Debug.Log("HookUtil begin " + tag);
            if(m_stack.Count > 0 && m_stack[m_stack.Count - 1].m_tag == tag)
            {
                Debug.LogError("可能出现了反复hook");
            }
        }
        //Debug.Log("HookUtil begin " + tag);
        StackData data = new StackData();
        data.m_tag = tag;
        data.m_span = Time.realtimeSinceStartup;
        data.m_alloc = Profiler.GetTotalAllocatedMemoryLong();
        data.m_childUseTime = 0;
        data.m_childAlloc = 0;
        m_stack.Add(data);
    }

    public static void End(string tag)
    {
        //         if (tag == "SingletonMgr.Update")
        //         {
        //             // 打印日志 避免多次hook的情况
        //             Debug.Log("HookUtil begin " + tag);
        //         }
        //Debug.Log("HookUtil end " + tag);
        StackData stackData = m_stack[m_stack.Count - 1];
        m_stack.RemoveAt(m_stack.Count - 1);
        if (stackData.m_tag == tag)
        {
            stackData.m_span = Time.realtimeSinceStartup - stackData.m_span;
            stackData.m_alloc = Profiler.GetTotalAllocatedMemoryLong() - stackData.m_alloc;
            stackData.m_span -= stackData.m_childUseTime;
            stackData.m_alloc -= stackData.m_childAlloc;

            AddStatistic(stackData);

            if(m_stack.Count > 0)
            {
                StackData parentStack = m_stack[m_stack.Count - 1];
                parentStack.m_childAlloc += stackData.m_alloc;
                parentStack.m_childUseTime += stackData.m_span;
                m_stack[m_stack.Count - 1] = parentStack;
            }
        }
        else
        {
            Debug.LogError("堆栈不匹配");
        }
    }

    static void AddStatistic(StackData data)
    {
        StatisticData statisticData;
        if (m_statisticDic.TryGetValue(data.m_tag, out statisticData))
        {
            statisticData.m_totalAlloc += data.m_alloc;
            statisticData.m_totalSpan += data.m_span;
            m_statisticDic[data.m_tag] = statisticData;
        }
        else
        {
            statisticData = new StatisticData();
            statisticData.m_tag = data.m_tag;
            statisticData.m_totalSpan = data.m_alloc;
            statisticData.m_totalSpan = data.m_span;
            m_statisticDic.Add(data.m_tag, statisticData);
        }
    }

    public static void ToMessage()
    {
        // todo 未来考虑根据时间和空间排序
        foreach(var iter in m_statisticDic)
        {
            Debug.Log("tag " + iter.Key + " span " + iter.Value.m_totalSpan + " alloc " + iter.Value.m_totalAlloc);
        }

        Debug.Log("HookUtils.ToMessage end");
    }
}
