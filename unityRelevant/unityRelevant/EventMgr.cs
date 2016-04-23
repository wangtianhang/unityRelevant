using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum EventId
{
    None = 0,
}

public class EventMgr
{
    public delegate void EventCallback(int id, System.Object param);

    Dictionary<int, List<WeakReference>> m_eventWeakMap = new Dictionary<int, List<WeakReference>>();

    //Dictionary<int, WeakReference> m_callbackToWeakRefMap = new Dictionary<int, WeakReference>();

    //List<WeakReference> m_curDispatherList = null;
    int m_curDispatherId = 0;
    int m_curDispatherListIndex = 0;

    public void SyncAdd(int eventId, EventCallback callback)
    {
        List<WeakReference> callbackList = null;
        if(m_eventWeakMap.TryGetValue(eventId, out callbackList))
        {
            
        }
        else
        {
            callbackList = new List<WeakReference>();
            m_eventWeakMap.Add(eventId, callbackList);
        }

        WeakReference weakCallback = new WeakReference(callback);
        callbackList.Add(weakCallback);

        //int hashCode = callback.GetHashCode();
        //m_callbackToWeakRefMap.Add(hashCode, weakCallback);
    }

    public void SyncRemove(int eventId, EventCallback callback)
    {
        List<WeakReference> callbackList = null;
//        WeakReference weakCallback = null;

//         if (m_callbackToWeakRefMap.TryGetValue(callback.GetHashCode(), out weakCallback))
//         {
// 
//         }
//         else
//         {
//             Console.WriteLine("not exist callback " + eventId + " " + callback);
//             return;
//         }

        if (m_eventWeakMap.TryGetValue(eventId, out callbackList))
        {
            if (eventId == m_curDispatherId)
            {
                for (int i = 0; i < callbackList.Count; ++i)
                {
                    WeakReference iter = callbackList[i];
                    if (iter.Target.Equals(callback))
                    {
                        if(i <= m_curDispatherId)
                        {
                            callbackList.Remove(iter);
                            m_curDispatherListIndex--;
                        }
                        else
                        {
                            callbackList.Remove(iter);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < callbackList.Count; ++i )
                {
                    WeakReference iter = callbackList[i];
                    if (iter.Target.Equals(callback))
                    {
                        callbackList.Remove(iter);
                    }
                }
                
            }
        }
    }

    public void Dispather(int eventId, System.Object param)
    {
        //m_curDispatherList = null;
        m_curDispatherId = eventId;
        List<WeakReference> callbackList = null;
        if (m_eventWeakMap.TryGetValue(eventId, out callbackList))
        {
            for (m_curDispatherListIndex = 0; m_curDispatherListIndex < callbackList.Count; ++m_curDispatherListIndex)
            {
                WeakReference iter = callbackList[m_curDispatherListIndex];
                if (iter.IsAlive)
                {
                    EventCallback callback = iter.Target as EventCallback;
                    callback(eventId, param);
                }
                else
                {
                    Console.WriteLine("Dispather " + eventId + " target object has been gc");
                }
            }
        }
        m_curDispatherId = 0;
        m_curDispatherListIndex = 0;
    }


}

class TestRef
{
    public void Callback4(int id, System.Object param)
    {
        Console.WriteLine("Callback4");
    }
}

public class TestEventMgrClass
{
    EventMgr m_eventMgr = new EventMgr();
    public void Test()
    {
        // 测试用例
        // 测试正常使用
        Console.WriteLine("测试正常使用");
        m_eventMgr.SyncAdd(1, Callback1);
        m_eventMgr.Dispather(1, null);
        m_eventMgr.SyncRemove(1, Callback1);
        m_eventMgr.Dispather(1, null);
        Console.WriteLine("测试正常使用end");
        // 测试用例
        // 同步删除
        // 即在callbacklist调用中也能删除自己在list中的callback
        Console.WriteLine("测试同步删除自己");
        m_eventMgr.SyncAdd(2, Callback2);
        m_eventMgr.Dispather(2, null);
        m_eventMgr.Dispather(2, null);
        Console.WriteLine("测试同步删除自己end");

        // 测试用例
        // 同步删除
        // 测试删除当前信号队列后面的callback


        // 测试用例
        // 弱引用
        // 即在调用时检查gc情况，不调用已经被gc的对象
        Console.WriteLine("测试弱引用");
        TestRef tmp = new TestRef();
        m_eventMgr.SyncAdd(4, tmp.Callback4);
        m_eventMgr.Dispather(4, null);
        tmp = null;
        GC.Collect();
        m_eventMgr.Dispather(4, null);
        Console.WriteLine("测试弱引用end");

        Console.Read();
    }

    void Callback1(int id, System.Object param)
    {
        Console.WriteLine("Callback1");
    }

    void Callback2(int id, System.Object param)
    {
        Console.WriteLine("Callback2");
        m_eventMgr.SyncRemove(2, Callback2);
    }


}

