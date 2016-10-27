/********************************************************************
	created:	2014/07/17
	created:	17:7:2014   11:10
	author:		wth(690879430@qq.com)
	
	purpose:	输入事件分发
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Tuner.Module;
using Tuner.Event;

enum InputMode
{
    InputMode_NoFinger,
    InputMode_OneFinger,
    InputMode_TwoFinger,
}

// public class OneFingerTouchUpdateEvt
// {
//     public void Init(Vector3 pos)
//     {
//         m_pos = pos;
//     }
//     public Vector3 m_pos = new Vector3();
// }

// public class TwoFingerTouchDownEvent
// {
//     public TwoFingerTouchDownEvent(Vector3 pos1, Vector3 pos2)
//     {
//         m_pos1 = pos1;
//         m_pos2 = pos2;
//     }
//     public Vector3 m_pos1 = Vector3.zero;
//     public Vector3 m_pos2 = Vector3.zero;
// }
// 
// public class TwoFingerTouchUpdateEvent
// {
//     public TwoFingerTouchUpdateEvent(Vector3 pos1, Vector3 pos2)
//     {
//         m_pos1 = pos1;
//         m_pos2 = pos2;
//         //m_distance = distance;
//     }
// 
//     public void Init(Vector3 pos1, Vector3 pos2)
//     {
//         m_pos1 = pos1;
//         m_pos2 = pos2;
//     }
// 
//     public Vector3 m_pos1 = Vector3.zero;
//     public Vector3 m_pos2 = Vector3.zero;
// }

abstract class InputModeBase
{
    protected InputEventCacher m_inputEventDispater = null;

    public InputModeBase(InputEventCacher inputEventDispather)
    {
        //m_game_round = game_round;
        m_inputEventDispater = inputEventDispather;
    }

    public abstract void enter();

    public abstract void update();

    public abstract void leave();

    /*  
    x'=x*cos(角度)+y*sin(角度)
    y'=y*cos(角度)-x*sin(角度)
    */
//     protected Vector3 pointYaxisRotate(float x, float z, float angel)
//     {
//         float rad = Mathf.Deg2Rad * angel;
//         float cos = Mathf.Cos(rad);
//         float sin = Mathf.Sin(rad);
//         float rotate_x = x * cos + z * sin;
//         float rotate_z = z * cos - x * sin;
//         return new Vector3(rotate_x, 0, rotate_z);
//     }

    //protected GameRound m_game_round = null;
}

class NoFingerInputMode : InputModeBase
{

    public NoFingerInputMode(InputEventCacher inputEventDispather)
        : base(inputEventDispather)
    {

    }

    public override void enter()
    {

    }

    public override void update()
    {

    }

    public override void leave()
    {

    }
}

class OneFingerInputMode : InputModeBase
{
    Vector3 m_lastPos = Vector3.zero;

    //OneFingerTouchUpdateEvt m_updatePos = new OneFingerTouchUpdateEvt();

    public OneFingerInputMode(InputEventCacher inputEventDispather)
        : base(inputEventDispather)
    {

    }

    public override void enter()
    {
        Vector3 tap_pos = new Vector3();
        if (Application.platform == RuntimePlatform.WindowsEditor
        || Application.platform == RuntimePlatform.OSXEditor)
        {
            tap_pos = Input.mousePosition;
        }
        else
        {
            Touch touch = Input.touches[0];
            tap_pos = touch.position;
        }

        //Tuner.Event.TunerEventMgr.Instance.DispatchEvent((int)ClientEventId.OneFingerTouchDown, tap_pos);
        m_inputEventDispater.AddInputEvent(new OneFingerTouchDownEvent(tap_pos));
    }

    public override void update()
    {
        Vector3 tap_pos = new Vector3();
        if (Application.platform == RuntimePlatform.WindowsEditor
        || Application.platform == RuntimePlatform.OSXEditor)
        {
            tap_pos = Input.mousePosition;
        }
        else
        {
            Touch touch = Input.touches[0];
            tap_pos = touch.position;

            m_lastPos = tap_pos;
        }

        //m_updatePos.Init(tap_pos);
        //Tuner.Event.TunerEventMgr.Instance.DispatchEvent((int)ClientEventId.OneFingerTouchDrag, tap_pos);
        m_inputEventDispater.AddOrReplaceLastSameEvent(new OneFingerTouchDragEvent(tap_pos));
    }

    public override void leave()
    {
        Vector3 tap_pos = new Vector3();
        if (Application.platform == RuntimePlatform.WindowsEditor
        || Application.platform == RuntimePlatform.OSXEditor)
        {
            tap_pos = Input.mousePosition;
        }
        else
        {
            //Touch touch = Input.touches[0];
            tap_pos = m_lastPos;
        }

        //Tuner.Event.TunerEventMgr.Instance.DispatchEvent((int)ClientEventId.OneFingerTouchUp, tap_pos);
        m_inputEventDispater.AddInputEvent(new OneFingerTouchUpEvent(tap_pos));
    }
}

class TwoFingerInputMode : InputModeBase
{
    //TwoFingerTouchUpdateEvent m_updateParam = new TwoFingerTouchUpdateEvent(Vector3.zero, Vector3.zero);
    Vector3 m_pos1 = Vector3.zero;
    Vector3 m_pos2 = Vector3.zero;

    public TwoFingerInputMode(InputEventCacher inputEventDispather)
        : base(inputEventDispather)
    {
        
    }

    public override void enter()
    {
        Vector3 pos1 = Input.touches[0].position;
        Vector3 pos2 = Input.touches[1].position;

        m_pos1 = pos1;
        m_pos2 = pos2;

        //Tuner.Event.TunerEventMgr.Instance.DispatchEvent((int)ClientEventId.TwoFingerTouchDown, new TwoFingerTouchDownEvent(pos1, pos2));
        m_inputEventDispater.AddInputEvent(new TwoFingerTouchDownEvent(pos1, pos2));
    }

    public override void update()
    {
        Vector3 pos1 = Input.touches[0].position;
        Vector3 pos2 = Input.touches[1].position;
        //Vector3 distance = pos1 - pos2;
        //m_updateParam.Init(pos1, pos2);
        m_pos1 = pos1;
        m_pos2 = pos2;

        //Tuner.Event.TunerEventMgr.Instance.DispatchEvent((int)ClientEventId.TwoFingerTouchDrag, m_updateParam);
        m_inputEventDispater.AddOrReplaceLastSameEvent(new TwoFingerTouchDragEvent(pos1, pos2));
    }

    public override void leave()
    {
        //Tuner.Event.TunerEventMgr.Instance.DispatchEvent((int)ClientEventId.TwoFingerTouchUp, null);
        m_inputEventDispater.AddInputEvent(new TwoFingerTouchUpEvent(m_pos1, m_pos2));
    }

}

public enum InputEventType
{
    None,
    OneFingerTouchDown,
    OneFingerTouchDrag,
    OneFingerTouchUp,
    TwoFingerTouchDown,
    TwoFingerTouchDrag,
    TwoFingerTouchUp,
}

public class InputEvent
{
    public InputEventType m_type = InputEventType.None;
}

public class OneFingerTouchDownEvent : InputEvent
{
    Vector3 m_tapPos = Vector3.zero;
    public OneFingerTouchDownEvent(Vector3 tapPos)
    {
        m_type = InputEventType.OneFingerTouchDown;
        m_tapPos = tapPos;
    }
}

public class OneFingerTouchDragEvent : InputEvent
{
    Vector3 m_tapPos = Vector3.zero;
    public OneFingerTouchDragEvent(Vector3 tapPos)
    {
        m_type = InputEventType.OneFingerTouchDrag;
        m_tapPos = tapPos;
    }
}

public class OneFingerTouchUpEvent : InputEvent
{
    Vector3 m_tapPos = Vector3.zero;
    public OneFingerTouchUpEvent(Vector3 tapPos)
    {
        m_type = InputEventType.OneFingerTouchUp;
        m_tapPos = tapPos;
    }
}

public class TwoFingerTouchDownEvent : InputEvent
{
    Vector3 m_pos1 = Vector3.zero;
    Vector3 m_pos2 = Vector3.zero;
    public TwoFingerTouchDownEvent(Vector3 pos1, Vector3 pos2)
    {
        m_type = InputEventType.TwoFingerTouchDown;
        m_pos1 = pos1;
        m_pos2 = pos2;
    }
}

public class TwoFingerTouchDragEvent : InputEvent
{
    Vector3 m_pos1 = Vector3.zero;
    Vector3 m_pos2 = Vector3.zero;
    public TwoFingerTouchDragEvent(Vector3 pos1, Vector3 pos2)
    {
        m_type = InputEventType.TwoFingerTouchDrag;
        m_pos1 = pos1;
        m_pos2 = pos2;
    }
}

public class TwoFingerTouchUpEvent : InputEvent
{
    Vector3 m_pos1 = Vector3.zero;
    Vector3 m_pos2 = Vector3.zero;
    public TwoFingerTouchUpEvent(Vector3 pos1, Vector3 pos2)
    {
        m_type = InputEventType.TwoFingerTouchUp;
        m_pos1 = pos1;
        m_pos2 = pos2;
    }
}

public class InputEventCacher : MonoBehaviour {

    InputMode m_input_mode_type = InputMode.InputMode_NoFinger;
    InputModeBase m_cur_mode = null;
    Dictionary<InputMode, InputModeBase> m_input_mode_map = new Dictionary<InputMode, InputModeBase>();

    public bool m_isEnable = true;

    bool m_isNewFrame = true; 
    List<InputEvent> m_inputEventList = new List<InputEvent>();

    // 每帧只处理一个缓存的input事件，这些事件是每帧互斥的
    public InputEvent GetNextInputEvent()
    {
        if(m_isNewFrame)
        {
            if (m_inputEventList.Count != 0)
            {
                InputEvent ret = m_inputEventList[0];
                m_inputEventList.RemoveAt(0);
                m_isNewFrame = false;
                return ret;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }

    }

    public void AddInputEvent(InputEvent evt)
    {
        m_inputEventList.Add(evt);
    }

    public void AddOrReplaceLastSameEvent(InputEvent evt)
    {
        if (m_inputEventList.Count != 0)
        {
            if (m_inputEventList[m_inputEventList.Count - 1].m_type == evt.m_type)
            {
                m_inputEventList[m_inputEventList.Count - 1] = evt;
            }
            else
            {
                m_inputEventList.Add(evt);
            }
        }
        else
        {
            m_inputEventList.Add(evt);
        }
    }

    void Awake()
    {
        TunerEventMgr.Instance.addEventCallBack((int)ClientEventId.EnalbeEventDispather, OnEnableEventDispather);

        m_input_mode_map.Add(InputMode.InputMode_NoFinger, new NoFingerInputMode(this));
        m_input_mode_map.Add(InputMode.InputMode_OneFinger, new OneFingerInputMode(this));
        m_input_mode_map.Add(InputMode.InputMode_TwoFinger, new TwoFingerInputMode(this));
        m_cur_mode = m_input_mode_map[m_input_mode_type];
    }

    public void LogicUpdate()
    {
        if(!m_isNewFrame)
        {
            m_isNewFrame = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isEnable)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.OSXEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_cur_mode.leave();
                    m_input_mode_type = InputMode.InputMode_OneFinger;
                    m_cur_mode = m_input_mode_map[m_input_mode_type];
                    m_cur_mode.enter();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    m_cur_mode.leave();
                    m_input_mode_type = InputMode.InputMode_NoFinger;
                    m_cur_mode = m_input_mode_map[m_input_mode_type];
                    m_cur_mode.enter();
                }

                m_cur_mode.update();
            }
            else
            {
                if (Input.touches.Length == 1)
                {
                    if (m_input_mode_type != InputMode.InputMode_OneFinger)
                    {
                        m_input_mode_type = InputMode.InputMode_OneFinger;
                        m_cur_mode.leave();
                        m_cur_mode = m_input_mode_map[m_input_mode_type];
                        m_cur_mode.enter();
                    }
                }
                else if (Input.touches.Length == 2)
                {
                    if (m_input_mode_type != InputMode.InputMode_TwoFinger)
                    {
                        m_input_mode_type = InputMode.InputMode_TwoFinger;
                        m_cur_mode.leave();
                        m_cur_mode = m_input_mode_map[m_input_mode_type];
                        m_cur_mode.enter();
                    }
                }
                else
                {
                    if (m_input_mode_type != InputMode.InputMode_NoFinger)
                    {
                        m_input_mode_type = InputMode.InputMode_NoFinger;
                        m_cur_mode.leave();
                        m_cur_mode = m_input_mode_map[m_input_mode_type];
                        m_cur_mode.enter();
                    }
                }

                m_cur_mode.update();
            }
        }
        else
        {
            if (m_input_mode_type != InputMode.InputMode_NoFinger)
            {
                m_input_mode_type = InputMode.InputMode_NoFinger;
                m_cur_mode.leave();
                m_cur_mode = m_input_mode_map[m_input_mode_type];
                m_cur_mode.enter();
            }
        }
    }

    void OnDestroy()
    {
        TunerEventMgr.Instance.DelayRemoveEventListener((int)ClientEventId.EnalbeEventDispather, OnEnableEventDispather);
    }

    void OnEnableEventDispather(int id, System.Object value)
    {
        m_isEnable = (bool)value;
        //GameConfig.Instance.m_enableInputDispatcher = m_isEnable;
    }
}
