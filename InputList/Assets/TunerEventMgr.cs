using System.Collections.Generic;
using UnityEngine;

namespace Tuner.Event
{
    public delegate void EventCallBack(int id, System.Object value);

    public class RemoveEventCallback
    {
        public RemoveEventCallback(int id, EventCallBack callback)
        {
            m_id = id;
            m_callbackList = callback;
        }
        public int m_id = 0;
        public EventCallBack m_callbackList = null;
    }

    public class AddEventCallback
    {
        public AddEventCallback(int id, EventCallBack callback)
        {
            m_id = id;
            m_callbackList = callback;
        }
        public int m_id = 0;
        public EventCallBack m_callbackList = null;
    }

    public class TunerEventMgr : Tuner.Singleton<TunerEventMgr>
    {
        Dictionary<int, List<EventCallBack>> mEventCallBackMap = new Dictionary<int, List<EventCallBack>>();

        List<RemoveEventCallback> mEventRemoveCallbackList = new List<RemoveEventCallback>();
        List<AddEventCallback> mEventAddCallbackList = new List<AddEventCallback>();

        public void addEventCallBack(int id, EventCallBack callback)
        {
            List<EventCallBack> tempCallbackList = null;
            mEventCallBackMap.TryGetValue(id, out tempCallbackList);
            if (tempCallbackList == null)
            {
                tempCallbackList = new List<EventCallBack>();
                mEventCallBackMap.Add(id, tempCallbackList);
            }
            tempCallbackList.Add(callback);
        }

        public void DelayAddEventCallback(int id, EventCallBack callback)
        {
            mEventAddCallbackList.Add(new AddEventCallback(id, callback));
        }

        public void DelayRemoveEventListener(int id, EventCallBack callback)
        {
            mEventRemoveCallbackList.Add(new RemoveEventCallback(id, callback));
        }

        public void DispatchEvent(int id, System.Object value)
        {
            List<EventCallBack> tempCallbackList = null;
            mEventCallBackMap.TryGetValue(id, out tempCallbackList);
            if (tempCallbackList != null)
            {
                for (int i = 0; i < tempCallbackList.Count; ++i)
                {
                    tempCallbackList[i].Invoke(id, value);
                }
                RemoveCallback();
            }
        }

        public void Update()
        {
            RemoveCallback();
            AddCallback();
        }

        void AddCallback()
        {
            if (mEventAddCallbackList.Count != 0)
            {
                for (int i = 0; i < mEventAddCallbackList.Count; ++i)
                {
                    AddEventCallback iter = mEventAddCallbackList[i];
                    List<EventCallBack> tempCallbackList = null;
                    mEventCallBackMap.TryGetValue(iter.m_id, out tempCallbackList);
                    if (tempCallbackList != null)
                    {
                        tempCallbackList.Add(iter.m_callbackList);
                    }
                    else
                    {
                        tempCallbackList = new List<EventCallBack>();
                        tempCallbackList.Add(iter.m_callbackList);
                        mEventCallBackMap.Add(iter.m_id, tempCallbackList);
                    }

                }
                mEventAddCallbackList.Clear();
            }
        }

        public void RemoveCallback()
        {
            if (mEventRemoveCallbackList.Count != 0)
            {
                for (int i = 0; i < mEventRemoveCallbackList.Count; ++i)
                {
                    RemoveEventCallback iter = mEventRemoveCallbackList[i];
                    RemoveCallback(iter.m_id, iter.m_callbackList);
                }
                mEventRemoveCallbackList.Clear();
            }
        }

        void RemoveCallback(int id, EventCallBack callback)
        {
            List<EventCallBack> tempCallbackList = null;
            mEventCallBackMap.TryGetValue(id, out tempCallbackList);
            if (tempCallbackList != null)
            {
                tempCallbackList.Remove(callback);
            }
        }
    }
}

